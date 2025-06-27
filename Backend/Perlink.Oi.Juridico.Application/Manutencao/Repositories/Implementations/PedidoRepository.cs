using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Results.Pedido;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations {

    public class PedidoRepository : IPedidoRepository {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAssuntoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public PedidoRepository(IDatabaseContext databaseContext, ILogger<IAssuntoRepository> logger, IUsuarioAtualProvider usuarioAtual) {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        #region Trabalhista

        private IQueryable<Pedido> ObterBaseDoTrabalhista(PedidoTrabalhistaSort sort, bool ascending, string descricao) {
            IQueryable<Pedido> query = DatabaseContext.Pedidos.AsNoTracking().Where(x => x.EhTrabalhista);

            string logName = "Pedido do Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort) {
                case PedidoTrabalhistaSort.Id:
                    query = query.SortBy(a => a.Id, ascending);
                    break;

                case PedidoTrabalhistaSort.RiscoPerda:
                    if (ascending) {
                        query = query.OrderBy(a => a.RiscoPerdaId == null ? 0 : 1).ThenBy(a => a.RiscoPerdaId);
                    } else {
                        query = query.OrderByDescending(a => a.RiscoPerdaId == null ? 0 : 1).ThenByDescending(a => a.RiscoPerdaId);
                    }
                    break;

                case PedidoTrabalhistaSort.ProprioTerceiro:
                    if (ascending) {
                        query = query.OrderBy(a => a.ProprioTerceiroId == null ? 0 : 1).ThenBy(a => a.ProprioTerceiroId);
                    } else {
                        query = query.OrderByDescending(a => a.ProprioTerceiroId == null ? 0 : 1).ThenByDescending(a => a.ProprioTerceiroId);
                    }
                    break;

                case PedidoTrabalhistaSort.ProvavelZero:
                    query = query.SortBy(a => a.ProvavelZero, ascending).ThenSortBy(a => a.ProvavelZero, ascending);
                    break;

                case PedidoTrabalhistaSort.Ativo:
                    query = query.SortBy(a => a.Ativo, ascending).ThenSortBy(a => a.Ativo, ascending);
                    break;

                case PedidoTrabalhistaSort.Descricao:
                default:
                    query = query.SortBy(x => x.Descricao, ascending).ThenSortBy(x => x.Descricao, ascending);
                    break;
            }

            return query.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(descricao.ToString().ToUpper()), descricao);
        }

        public CommandResult<PaginatedQueryResult<Pedido>> ObterPaginadoDoTrabalhista(PedidoTrabalhistaSort sort, bool ascending, 
            int pagina, int quantidade, string descricao) {
            string logName = "Pedido do Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_TRABALHISTA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_TRABALHISTA, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<Pedido>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoTrabalhista(sort, ascending, descricao);

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);


            var resultado = new PaginatedQueryResult<Pedido>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<Pedido>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Pedido>> ObterDoTrabalhista(PedidoTrabalhistaSort sort,
            bool ascending, string descricao) {
            string logName = "Pedido do Trabalhista";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_TRABALHISTA)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_TRABALHISTA, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Pedido>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoTrabalhista(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Pedido>>.Valid(resultado);
        }

        #endregion Trabalhista

        #region Civel Estrategico

        private IQueryable<PedidoCommandResult> ObterBaseDoCivelEstrategico(PedidoCivelEstrategicoSort sort, bool ascending, string descricao) {

            var query = DatabaseContext.Pedidos.AsNoTracking().Where(x => x.EhCivel).Select(y => new { y.Id, y.Descricao, y.Ativo }).AsNoTracking().ToArray();
            IQueryable<PedidoCommandResult> listaPedidos = from a in DatabaseContext.Pedidos.AsNoTracking()
                                                           join ma in DatabaseContext.MigracaoPedido on a.Id equals ma.CodPedidoCivelEstrat into LeftJoinMa
                                                           from ma in LeftJoinMa.DefaultIfEmpty()
                                                           where a.EhCivelEstrategico
                                                           select new PedidoCommandResult(
                                                                a.Id,
                                                                (int?)ma.CodPedidoCivelConsumidor,
                                                                a.Descricao,
                                                                a.Ativo,
                                                                (int?)ma.CodPedidoCivelConsumidor == null ? null : query.FirstOrDefault(z => z.Id == ma.CodPedidoCivelConsumidor).Descricao,
                                                                (int?)ma.CodPedidoCivelConsumidor == null ? false : query.FirstOrDefault(z => z.Id == ma.CodPedidoCivelConsumidor) != null ? query.FirstOrDefault(z => z.Id == ma.CodPedidoCivelConsumidor).Ativo : false
                                                            );

            string logName = "Pedidos do Civel Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort)
            {
                case PedidoCivelEstrategicoSort.Ativo:
                    listaPedidos = listaPedidos.SortBy(a => a.Ativo, ascending).ThenSortBy(a => a.Ativo, ascending);
                    break;

                case PedidoCivelEstrategicoSort.Id:
                    listaPedidos = listaPedidos.SortBy(a => a.Id, ascending).ThenSortBy(a => a.Id, ascending);
                    break;

                case PedidoCivelEstrategicoSort.Descricao:
                default:
                    listaPedidos = listaPedidos.SortBy(x => x.Descricao, ascending).ThenSortBy(x => x.Descricao, ascending);
                    break;
            }

            return listaPedidos.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(descricao.ToUpper().ToString()), descricao);
        }        



        public CommandResult<PaginatedQueryResult<PedidoCommandResult>> ObterPaginadoDoCivelEstrategico(PedidoCivelEstrategicoSort sort, bool ascending, 
            int pagina, int quantidade, string descricao) {
            string logName = "Pedido do Civel Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<PedidoCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoCivelEstrategico(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<PedidoCommandResult>() {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<PedidoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<PedidoCommandResult>> ObterDoCivelEstrategico(PedidoCivelEstrategicoSort sort,
            bool ascending, string descricao) {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO)) {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<PedidoCommandResult>>.Forbidden();
            }

            string logName = "Pedido do Civel Estratégico";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
   
            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelEstrategico(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<PedidoCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Pedido>> ObterDescricaoDeParaCivelEstrategico()
        {
            IQueryable<Pedido> query = DatabaseContext.Pedidos.AsNoTracking().Where(x => x.EhCivelEstrategico);

            return CommandResult<IReadOnlyCollection<Pedido>>.Valid(query.ToArray());
        }

        #endregion Civel Estrategico

        #region Civel Consumidor

        private IQueryable<PedidoConsumidorCommandResult> ObterBaseDoCivelConsumidor(PedidoCivelConsumidorSort sort, bool ascending, string descricao)
        {
           var listaConsumidorPedidoMigracao = DatabaseContext.Pedidos.AsNoTracking().Where(x => x.EhCivelEstrategico).Select(y => new { y.Id, y.Descricao, y.Ativo, y.Audiencia }).AsNoTracking().ToArray();
            IQueryable<PedidoConsumidorCommandResult> listaPedidos = from a in DatabaseContext.Pedidos.AsNoTracking()
                                                           join ma in DatabaseContext.MigracaoPedidoConsumidor on a.Id equals ma.CodPedidoCivel into LeftJoinMa
                                                           from ma in LeftJoinMa.DefaultIfEmpty()
                                                           where a.EhCivel
                                                           select new PedidoConsumidorCommandResult(
                                                                a.Id,
                                                                (int?)ma.CodPedidoCivelEstrat,
                                                                a.Descricao,
                                                                a.Audiencia,
                                                                a.Ativo,
                                                                (int?)ma.CodPedidoCivelEstrat == null ? null : listaConsumidorPedidoMigracao.FirstOrDefault(z => z.Id == ma.CodPedidoCivelEstrat).Descricao,
                                                                (int?)ma.CodPedidoCivelEstrat == null ? false : listaConsumidorPedidoMigracao.FirstOrDefault(z => z.Id == ma.CodPedidoCivelEstrat) != null ? listaConsumidorPedidoMigracao.FirstOrDefault(z => z.Id == ma.CodPedidoCivelEstrat).Ativo : false
                                                            );

            string logName = "Pedidos do Civel Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));
            switch (sort)
            {
                case PedidoCivelConsumidorSort.Ativo:
                    listaPedidos = listaPedidos.SortBy(a => a.Ativo, ascending).ThenSortBy(a => a.Ativo, ascending);
                    break;

                case PedidoCivelConsumidorSort.Audiencia:
                    listaPedidos = listaPedidos.SortBy(a => a.Audiencia, ascending).ThenSortBy(a => a.Audiencia, ascending);
                    break;

                case PedidoCivelConsumidorSort.Id:
                    listaPedidos = listaPedidos.SortBy(a => a.Id, ascending).ThenSortBy(a => a.Id, ascending);
                    break;

                case PedidoCivelConsumidorSort.Descricao:
                default:
                    listaPedidos = listaPedidos.SortBy(x => x.Descricao, ascending).ThenSortBy(x => x.Descricao, ascending);
                    break;
            }

            return listaPedidos.WhereIfNotNull(x => x.Descricao.ToString().ToUpper().Contains(descricao.ToString().ToUpper()), descricao);
        }



        public CommandResult<PaginatedQueryResult<PedidoConsumidorCommandResult>> ObterPaginadoDoCivelConsumidor(PedidoCivelConsumidorSort sort, bool ascending,
            int pagina, int quantidade, string descricao)
        {
            string logName = "Pedido do Civel Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                return CommandResult<PaginatedQueryResult<PedidoConsumidorCommandResult>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var listaBase = ObterBaseDoCivelConsumidor(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Total de registros"));
            var total = listaBase.Count();

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Quantidade de registros para saltar"));
            var skip = Pagination.PagesToSkip(quantidade, total, pagina);

            var resultado = new PaginatedQueryResult<PedidoConsumidorCommandResult>()
            {
                Total = total,
                Data = listaBase.Skip(skip).Take(quantidade).ToArray()
            };

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<PaginatedQueryResult<PedidoConsumidorCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<PedidoConsumidorCommandResult>> ObterDoCivelConsumidor(PedidoCivelConsumidorSort sort,
            bool ascending, string descricao)
        {

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<PedidoConsumidorCommandResult>>.Forbidden();
            }

            string logName = "Pedido do Civel Consumidor";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBaseDoCivelConsumidor(sort, ascending, descricao).ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<PedidoConsumidorCommandResult>>.Valid(resultado);
        }

        public CommandResult<IReadOnlyCollection<Pedido>> ObterDescricaoDeParaCivelConsumidor()
        {
            IQueryable<Pedido> query = DatabaseContext.Pedidos.AsNoTracking().Where(x => x.EhCivel);

            return CommandResult<IReadOnlyCollection<Pedido>>.Valid(query.ToArray());
        }

        #endregion Civel Consumidor
    }
}