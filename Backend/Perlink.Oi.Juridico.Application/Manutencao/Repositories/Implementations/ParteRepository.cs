using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class ParteRepository : IParteRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IParteRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ParteRepository(IDatabaseContext databaseContext, ILogger<IParteRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Parte> ObterBase(
           ParteSort sort, bool ascending, TipoParte tipoParte, string nome, string documento, int? carteiraDeTrabalho) {
            IQueryable<Parte> partes = DatabaseContext.Partes.AsNoTracking();

            switch (sort) {
                case ParteSort.TipoParte:
                    if(ascending) {
                        partes = partes.OrderBy(x => x.TipoParteValor == null ? 0: 1).ThenBy(x => x.TipoParteValor);
                    } else {
                        partes = partes.OrderByDescending(x => x.TipoParteValor == null ? 0 : 1).ThenByDescending(x => x.TipoParteValor);
                    }                    
                    break;

                case ParteSort.Documento:
                    if (ascending) {
                        partes = partes.OrderBy(x => x.CPF == null ? 0 : 1).ThenBy(x => x.CPF).OrderBy(x => x.CNPJ == null ? 0 : 1).ThenBy(x => x.CNPJ);
                    } else {
                        partes = partes.OrderByDescending(x => x.CPF == null ? 0 : 1).ThenByDescending(x => x.CPF).OrderByDescending(x => x.CNPJ == null ? 0 : 1).ThenByDescending(x => x.CNPJ);
                    }
                    break;

                case ParteSort.CarteiraTrabalho:
                    if(ascending) {
                        partes = partes.OrderBy(x => x.CarteiraDeTrabalho == null ? 0 : 1).ThenBy(x => x.CarteiraDeTrabalho);
                    }else {
                        partes = partes.OrderByDescending(x => x.CarteiraDeTrabalho == null ? 0 : 1).ThenByDescending(x => x.CarteiraDeTrabalho);
                    }                    
                    break;

                case ParteSort.Estado:
                    if (ascending) {
                        partes = partes.OrderBy(x => x.EstadoId == null ? 0 : 1).ThenBy(x => x.EstadoId);
                    } else {
                        partes = partes.OrderByDescending(x => x.EstadoId == null ? 0 : 1).ThenByDescending(x => x.EstadoId);
                    }
                    break;

                case ParteSort.Nome:
                default:
                    partes = partes.SortBy(x => x.Nome, ascending);
                    break;
            }

            return partes
                .WhereIfNotNull(x => x.TipoParteValor == tipoParte.Valor, tipoParte.Valor)
                .WhereIfNotNull(x => x.Nome.Contains(nome.ToUpper()), nome)
                .WhereIfNotNull(x => x.CarteiraDeTrabalho.ToString().StartsWith(carteiraDeTrabalho.ToString()), carteiraDeTrabalho)
                .WhereIfNotNull(x => (x.CPF == documento.ToString() || x.CNPJ == documento.ToString()), documento);
        }

        public CommandResult<IReadOnlyCollection<Parte>> Obter(ParteSort sort, bool ascending, TipoParte tipoParte, string nome, string documento, int? carteiraDeTrabalho) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PARTE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PARTE, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Parte>>.Forbidden();
            }

            string logName = "Partes";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
           
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, tipoParte, nome, documento, carteiraDeTrabalho).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Parte>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<Parte>> ObterPaginado(
            ParteSort sort, bool ascending, int pagina, int quantidade,
            TipoParte tipoParte, string nome, string documento, int? carteiraDeTrabalho) {
            string logName = "Partes";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PARTE)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PARTE, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Parte>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, tipoParte, nome, documento, carteiraDeTrabalho);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Parte>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Parte>>.Valid(resultado);
        }
    }
}