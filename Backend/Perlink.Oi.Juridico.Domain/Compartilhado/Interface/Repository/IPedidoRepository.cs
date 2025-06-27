using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Interface.Repository;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IPedidoRepository : IBaseCrudRepository<Pedido, long>
    {
        Task<ICollection<PedidoEstornoDTO>> ConsultaPedidoEstorno(long tipoProcesso, long idProcesso);
        Task<ICollection<PedidoEstornoDTO>> ConsultarPedido(long tipoProcesso, long idProcesso);
    }
}
