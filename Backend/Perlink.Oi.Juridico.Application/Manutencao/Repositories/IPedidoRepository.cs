using Perlink.Oi.Juridico.Application.Manutencao.Results.Pedido;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using Perlink.Oi.Juridico.Manutencao.Sorts;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IPedidoRepository {

        #region Civel Estratégico

        CommandResult<PaginatedQueryResult<PedidoCommandResult>> ObterPaginadoDoCivelEstrategico(PedidoCivelEstrategicoSort sort, bool ascending, 
            int pagina, int quantidade, string descricao);        

        CommandResult<IReadOnlyCollection<PedidoCommandResult>> ObterDoCivelEstrategico(PedidoCivelEstrategicoSort sort,
           bool ascending, string descricao);

        #endregion

        #region Trabalhista

        CommandResult<PaginatedQueryResult<Pedido>> ObterPaginadoDoTrabalhista(PedidoTrabalhistaSort sort, bool ascending,
            int pagina, int quantidade, string descricao);

        CommandResult<IReadOnlyCollection<Pedido>> ObterDoTrabalhista(PedidoTrabalhistaSort sort, bool ascending, string descricao);
        CommandResult<IReadOnlyCollection<Pedido>> ObterDescricaoDeParaCivelEstrategico();
        #endregion

        #region Consumidor
        CommandResult<PaginatedQueryResult<PedidoConsumidorCommandResult>> ObterPaginadoDoCivelConsumidor(PedidoCivelConsumidorSort sort, bool ascending,
            int pagina, int quantidade, string descricao);

        CommandResult<IReadOnlyCollection<PedidoConsumidorCommandResult>> ObterDoCivelConsumidor(PedidoCivelConsumidorSort sort,
             bool ascending, string descricao);

        CommandResult<IReadOnlyCollection<Pedido>> ObterDescricaoDeParaCivelConsumidor();
        #endregion
    }
}
