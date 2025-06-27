using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class GrupoPedidoRepository : IGrupoPedidoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IGrupoPedidoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public GrupoPedidoRepository(IDatabaseContext databaseContext, ILogger<IGrupoPedidoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<GrupoPedido> ObterBase(TipoProcesso tipoProcesso, GrupoPedidoSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Grupo Pedido";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            IQueryable<GrupoPedido> query = DatabaseContext.GruposPedidos.Where(x => x.TipoProcessoId == tipoProcesso.Id).AsNoTracking();

            switch (sort)
            {
                case GrupoPedidoSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case GrupoPedidoSort.Descricao:   
                        query = query.OrderBy(a => a.Descricao).ThenBy(a => a.Id);                    
                    break;

                case GrupoPedidoSort.TipoProcessoId:
                default:
                    query = query.SortBy(a => a.TipoProcessoId, ascending);
                    break;
            }

            query = query.WhereIfNotNull(x => x.Descricao.ToUpper().Contains(pesquisa.ToString().ToUpper()), pesquisa);            
            return query;
        }

        public CommandResult<PaginatedQueryResult<GrupoPedido>> ObterPaginado(TipoProcesso tipoProcesso, GrupoPedidoSort sort, bool ascending, int pagina, int quantidade, string pesquisa)
        {
            string logName = "Grupos Pedidos";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));            

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, sort, ascending, pesquisa);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<GrupoPedido>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<GrupoPedido>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<GrupoPedido>> ObterTodos(TipoProcesso tipoProcesso)
        {
            string logName = nameof(GrupoPedido);
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBase(tipoProcesso, GrupoPedidoSort.Descricao, true, null);

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<GrupoPedido>>.Valid(listaBase.ToArray());
        }

        public CommandResult<IReadOnlyCollection<GrupoPedido>> Obter(TipoProcesso tipoProcesso, GrupoPedidoSort sort, bool ascending, string pesquisa = null)
        {
            string logName = "Grupo Pedido";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(tipoProcesso, sort, ascending, pesquisa).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<GrupoPedido>>.Valid(resultado);
        }   
    }
}