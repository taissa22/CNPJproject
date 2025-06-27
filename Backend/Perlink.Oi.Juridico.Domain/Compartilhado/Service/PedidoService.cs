using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class PedidoService : BaseCrudService<Pedido, long>, IPedidoService
    {
        private readonly IPedidoRepository repository;

        public PedidoService(IPedidoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<ICollection<PedidoEstornoDTO>> ConsultaPedidoEstorno(long tipoProcesso, long idProcesso)
        {
            return await repository.ConsultarPedido(tipoProcesso, idProcesso);
        }

        public async Task<ICollection<PedidoEstornoDTO>> ConsultarPedido(long tipoProcesso, long idProcesso) {
            return await repository.ConsultarPedido(tipoProcesso, idProcesso);
        }
    }
}
