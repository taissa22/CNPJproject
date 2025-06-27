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
    class ComarcaRepository : IComarcaRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IComarcaRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public ComarcaRepository(IDatabaseContext databaseContext, ILogger<IComarcaRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<Comarca> ObterBase(ComarcaSort sort, bool ascending, string codigoEstado, string pesquisa = null)
        {
            string logName = "Comarca";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<Comarca> query = DatabaseContext.Comarcas.AsNoTracking();
            
            switch (sort)
            {
                case ComarcaSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case ComarcaSort.EstadoId:
                    query = query.SortBy(a => a.Estado.Id, ascending);
                    break;

                case ComarcaSort.ComarcaBB:
                    query = query.SortBy(a => a.ComarcaBBId, ascending);
                    break;

                case ComarcaSort.Nome:
                default:
                    query = query.SortBy(a => a.Nome, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Estado.Id == codigoEstado  , codigoEstado).WhereIfNotNull(x => x.Nome.ToLower().Contains(pesquisa.ToLower()) , pesquisa);
        }

        public CommandResult<PaginatedQueryResult<Comarca>> ObterPaginado(int pagina, int quantidade, string codigoEstado, ComarcaSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Comarca";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Comarca>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(sort, ascending, codigoEstado, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<Comarca>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Comarca>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Comarca>> Obter(string codigoEstado, ComarcaSort sort, bool ascending, string pesquisa = null)
        {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_COMARCA))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_COMARCA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Comarca>>.Forbidden();
            }

            string logName = "Comarca";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(sort, ascending, codigoEstado, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Comarca>>.Valid(resultado);
        }

    }
}
