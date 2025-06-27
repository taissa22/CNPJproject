using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service {
    public interface IPedidoService : IBaseCrudService<Pedido, long>
    {
        Task<ICollection<PedidoEstornoDTO>> ConsultarPedido(long tipoProcesso, long idProcesso);

        Task<ICollection<PedidoEstornoDTO>> ConsultaPedidoEstorno(long tipoProcesso, long idProcesso);
    }
}

