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

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{

    public class ProfissionalRepository : IProfissionalRepository {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IProfissionalRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ProfissionalRepository(IDatabaseContext databaseContext, ILogger<IProfissionalRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<Profissional>> Obter(ProfissionalSort sort, bool ascending, TipoPessoa tipoPessoa, string nome, string documento, bool? advogadoAutor = null) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PROFISSIONAL)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PROFISSIONAL, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Profissional>>.Forbidden();
            }

            string logName = "Profissionais";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, tipoPessoa, nome, documento, advogadoAutor).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Profissional>>.Valid(resultado);
        }

        private IQueryable<Profissional> ObterBase(ProfissionalSort sort, bool ascending, TipoPessoa tipoPessoa, string nome, string documento, bool? advogadoAutor = null) {
            IQueryable<Profissional> query = DatabaseContext.Profissionais;

          

            switch (sort) {
                case ProfissionalSort.EstadoOAB:
                    if (ascending) {
                        query = query.OrderBy(x => x.EstadoOABId == null ? 0 : 1).ThenBy(x => x.EstadoOABId);
                    } else {
                        query = query.OrderByDescending(x => x.EstadoOABId == null ? 0 : 1).ThenByDescending(x => x.EstadoOABId);
                    }
                    break;

                case ProfissionalSort.NumeroOAB:
                    if (ascending) {
                        query = query.OrderBy(x => x.RegistroOAB == null ? 0 : 1).ThenBy(x => x.RegistroOAB);
                    } else {
                        query = query.OrderByDescending(x => x.RegistroOAB == null ? 0 : 1).ThenByDescending(x => x.RegistroOAB);
                    }
                    break;

                case ProfissionalSort.EhAdvogadoAutor:
                    query = query.SortBy(x => x.EhAdvogado, ascending);
                    break;

                case ProfissionalSort.Estado:
                    if (ascending) {
                        query = query.OrderBy(x => x.EstadoId == null ? 0 : 1).ThenBy(x => x.EstadoId);
                    } else {
                        query = query.OrderByDescending(x => x.EstadoId == null ? 0 : 1).ThenByDescending(x => x.EstadoId);
                    }
                    break;

                case ProfissionalSort.Documento:
                    if (ascending) {
                        query = query.OrderBy(x => x.CPF == null ? 0 : 1).ThenBy(x => x.CPF).OrderBy(x => x.CNPJ == null ? 0 : 1).ThenBy(x => x.CNPJ);
                    } else {
                        query = query.OrderByDescending(x => x.CPF == null ? 0 : 1).ThenByDescending(x => x.CPF).OrderByDescending(x => x.CNPJ == null ? 0 : 1).ThenByDescending(x => x.CNPJ);
                    }
                    break;

                case ProfissionalSort.Nome:
                default:
                    if(ascending) {
                        query = query.OrderBy(x => x.Nome.Padronizar()).ThenBy(x => x.Id);
                    } else {
                        query = query.OrderByDescending(x => x.Nome.Padronizar()).ThenByDescending(x => x.Id);
                    }
                    
                    break;
            }

            if (documento != null )
            {
                query = query.WhereIfNotNull(x => x.CPF == documento.ToString() || x.CNPJ == documento.ToString(), documento);
            }
            else
            {
                query = query.WhereIfNotNull(x => x.TipoPessoaValor == tipoPessoa.Valor, tipoPessoa.Valor);
            }

            return query
                .AsNoTracking()                                
                .WhereIfNotNull(x => x.Nome.ToUpper().Contains(nome.Padronizar()), nome)                
                .WhereIfNotNull(x => x.EhAdvogado == advogadoAutor, advogadoAutor);
        }

        public CommandResult<PaginatedQueryResult<Profissional>> ObterPaginado(ProfissionalSort sort, bool ascending, int pagina, int quantidade, TipoPessoa tipoPessoa, string nome, string documento, bool? advogadoAutor = null) {
            string logName = "Profissionais";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PROFISSIONAL)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PROFISSIONAL, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Profissional>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, tipoPessoa, nome, documento, advogadoAutor);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Profissional>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Profissional>>.Valid(resultado);
        }

        public CommandResult<bool> Existe(string nome, int? id) {
            string logName = "Profissionais";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Verificar {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PROFISSIONAL)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PROFISSIONAL, UsuarioAtual.Login));
                return CommandResult<bool>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            bool resultado = DatabaseContext.Profissionais
                .WhereIfNotNull(x => x.Id != id, id)
                .Any(x => x.Nome == nome.Padronizar());

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<bool>.Valid(resultado);
        }
    }
}