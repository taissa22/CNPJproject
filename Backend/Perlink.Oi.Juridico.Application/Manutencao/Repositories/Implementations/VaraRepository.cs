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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class VaraRepository : IVaraRepository
    {

        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IVaraRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public VaraRepository(IDatabaseContext databaseContext, ILogger<VaraRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Vara> ObterBase(int comarcaId)
        {
            string logName = "Comarca";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Vara> query = DatabaseContext.Varas.Where(v => v.ComarcaId == comarcaId).AsNoTracking().IgnoreQueryFilters();

            return query;
        }

        private IQueryable<Vara> ObterBase(int comarcaId, VaraSort sort, bool ascending, string pesquisa)
        {
            string logName = "Comarca";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Vara> query = DatabaseContext.Varas.Where(v => v.ComarcaId == comarcaId).AsNoTracking().IgnoreQueryFilters();

            switch (sort)
            {
                case VaraSort.Tipo:
                    query = query.SortBy(a => a.TipoVara.Nome, ascending);
                    break;
                case VaraSort.Endereco:
                    query = query.SortBy(a => a.Endereco, ascending);
                    break;
                case VaraSort.TribunalDeJustica:
                    query = query.SortBy(a => a.OrgaoBB == null || a.OrgaoBB.TribunalBB == null ? 0 : 1, ascending).ThenSortBy(a => a.OrgaoBB.TribunalBB.Nome, ascending);
                    break;
                case VaraSort.VaraBB:
                    query = query.SortBy(a => a.OrgaoBB == null ? 0 : 1, ascending).ThenSortBy(a => a.OrgaoBB.Nome, ascending);
                    break;
                case VaraSort.Id:
                default:
                    query = query.SortBy(a => a.VaraId, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.TipoVara.Nome.ToLower().Contains(pesquisa.ToLower()), pesquisa);
        }


        public CommandResult<PaginatedQueryResult<Vara>> ObterPaginado(int comarcaId, int pagina, int quantidade, VaraSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Vara>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(comarcaId, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Vara>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Vara>>.Valid(resultado);
        }

        public CommandResult<PaginatedQueryResult<Vara>> ObterPorComarca(int comarcaId)
        {
            string logName = "Vara";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(comarcaId);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            var resultado = new PaginatedQueryResult<Vara>()
            {
                Total = total,
                Data = listaBase.ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Vara>>.Valid(resultado);

        }
    }
}
