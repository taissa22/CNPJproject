using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Manutencao.Sorts;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    internal class FielDepositarioRepository : IFielDepositarioRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IFielDepositarioRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public FielDepositarioRepository(IDatabaseContext context, ILogger<IFielDepositarioRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = context;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        /// <summary>
        /// Gera a query base para busca
        /// </summary>

        public IQueryable<FielDepositario> ObterBase(FielDepositarioSort sort, bool ascending) {
            IQueryable<FielDepositario> query = DatabaseContext.FieisDepositarios.AsNoTracking();

            switch (sort) {
                case FielDepositarioSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case FielDepositarioSort.Cpf:
                    if (ascending) {
                        query = query.OrderBy(a => a.Cpf == null ? 0 : 1).ThenBy(a => a.Cpf);
                    } else {
                        query = query.OrderByDescending(a => a.Cpf == null ? 0 : 1).ThenByDescending(a => a.Cpf);
                    }
                    break;

                case FielDepositarioSort.Nome:
                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }
            return query;
        }

        /// <summary>
        /// Recupera os registros paginados
        /// </summary>
        public CommandResult<PaginatedQueryResult<FielDepositario>> ObterPaginado(int pagina, int quantidade,
            FielDepositarioSort sort, bool ascending) {
            string logName = "Fiel Depositário";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FIEL_DEPOSITARIO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FIEL_DEPOSITARIO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<FielDepositario>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<FielDepositario>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<FielDepositario>>.Valid(resultado);
        }

        /// <summary>
        /// Recupera os retistros com o filtros para exportação.
        /// </summary>
        public CommandResult<IReadOnlyCollection<FielDepositario>> Obter(FielDepositarioSort sort, bool ascending) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_FIEL_DEPOSITARIO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_FIEL_DEPOSITARIO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<FielDepositario>>.Forbidden();
            }

            string logName = "Fiel Depositário";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending).ToArray();

            return CommandResult<IReadOnlyCollection<FielDepositario>>.Valid(resultado);
        }
    }
}