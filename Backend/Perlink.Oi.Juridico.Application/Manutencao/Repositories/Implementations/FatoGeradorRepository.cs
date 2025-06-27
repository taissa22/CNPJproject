using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class FatoGeradorRepository : IFatoGeradorRepository
    {
        private IDatabaseContext DatabaseContext { get; }

        private ILogger<IFatoGeradorRepository> Logger { get; }

        private IUsuarioAtualProvider UsuarioAtual { get; }

        public FatoGeradorRepository(IDatabaseContext databaseContext, ILogger<IFatoGeradorRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<FatoGerador> ObterBase(FatoGeradorSort sort, bool ascending, string? pesquisa) {
            IQueryable<FatoGerador> query = DatabaseContext.FatosGeradores.AsNoTracking();

            switch (sort) {
                case FatoGeradorSort.Id:
                    query = query.SortBy(x => x.Id, ascending).ThenSortBy(x => x.Id, ascending);
                    break;

                case FatoGeradorSort.Ativo                :
                    if (ascending)
                    {
                        query = query.OrderBy(x => x.Ativo == null ? 0 : 1).ThenBy(x => x.Ativo);
                    }
                    else
                    {
                        query = query.OrderByDescending(x => x.Ativo == null ? 0 : 1).ThenByDescending(x => x.Ativo);
                    }
                    break;

                case FatoGeradorSort.Nome:
                default:
                    query = query.SortBy(x => x.Nome, ascending).ThenSortBy(x => x.Nome, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Nome.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);
        }

        public CommandResult<IReadOnlyCollection<FatoGerador>> Obter(FatoGeradorSort sort, bool ascending, string? pesquisa) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FATO_GERADOR)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FATO_GERADOR, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<FatoGerador>>.Forbidden();
            }

            string logName = "Fato Gerador";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<FatoGerador>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<FatoGerador>> ObterPaginado(FatoGeradorSort sort, bool ascending, int pagina, int quantidade, string? pesquisa) {
            string logName = "Fato Gerador";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FATO_GERADOR))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FATO_GERADOR, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<FatoGerador>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);


            var resultado = new PaginatedQueryResult<FatoGerador>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<FatoGerador>>.Valid(resultado);
        }       
    }
}