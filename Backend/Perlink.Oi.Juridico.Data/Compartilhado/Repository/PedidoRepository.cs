using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class PedidoRepository : BaseCrudRepository<Pedido, long>, IPedidoRepository
    {
        private readonly JuridicoContext dbContext;
        public PedidoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<PedidoEstornoDTO>> ConsultaPedidoEstorno(long tipoProcesso, long idProcesso)
        {
            ICollection<PedidoEstornoDTO> result;
            if (tipoProcesso == 18)
            {
                result = await ListarPedidosEstornoPorPex(idProcesso);                
            }
            else
            {
                result = await ListarPedidos(idProcesso);
            }
            return result;
        }

        public async Task<ICollection<PedidoEstornoDTO>> ConsultarPedido(long tipoProcesso, long idProcesso) {
            ICollection<PedidoEstornoDTO> result;
            if (tipoProcesso == 18) {
                result = await ListarPedidosEstornoPorPex(idProcesso);
            }
            else {
                result = await ListarPedidos(idProcesso);
            }
            return result;
        }

        private async Task<IList<PedidoEstornoDTO>> ListarPedidos(long idProcesso) {
            IQueryable<PedidoProcesso> query = dbContext.PedidoProcessos
                                            .Include(pp => pp.Pedido);
            query = query.Where(pp =>
                            pp.CodigoProcesso == idProcesso
                    );
            var result = await query.Select(pedidoProcesso => new PedidoEstornoDTO {
                CodigoPedido = pedidoProcesso.Id,
                CodigoProcesso = pedidoProcesso.CodigoProcesso,
                CodigoRiscoPerda = pedidoProcesso.Pedido.CodigoRiscoPerda,
                DescricaoPedido = pedidoProcesso.Pedido.Descricao
            }).OrderBy(e => e.DescricaoPedido).ToListAsync();

            return result;
        }

        private async Task<IList<PedidoEstornoDTO>> ListarPedidosEstorno(long idProcesso)
        {
            IQueryable<PedidoProcesso> query = dbContext.PedidoProcessos
                                            .Include(pp => pp.Pedido);
            query = query.Where(pp =>
                            pp.CodigoProcesso == idProcesso
                    );
            var result = await query.Select(pedidoProcesso => new PedidoEstornoDTO
            {
                CodigoPedido = pedidoProcesso.Id,
                CodigoProcesso = pedidoProcesso.CodigoProcesso,
                CodigoRiscoPerda = pedidoProcesso.Pedido.CodigoRiscoPerda,
                DescricaoPedido = pedidoProcesso.Pedido.Descricao
            }).OrderBy(e => e.DescricaoPedido).ToListAsync();

            return result;
        }

        private async Task<IList<PedidoEstornoDTO>> ListarPedidosEstornoPorPex(long idProcesso)
        {
            IQueryable<ContratoPedidoProcesso> query = dbContext.ContratoPedidoProcesso
                                                        .Include(cpp => cpp.ContratoProcesso)
                                                        .Include(s => s.Pedido);

            query = query.Where(cpp =>
                            cpp.ContratoProcesso.CodigoProcesso == idProcesso
                        );

            var result = await query.Select(cpp => new PedidoEstornoDTO
            {
                CodigoPedido = cpp.Id,
                CodigoProcesso = cpp.CodigoProcesso,
                NumeroContrato = cpp.ContratoProcesso.NumeroContrato,
                DescricaoPedido = cpp.Pedido.Descricao
            }).ToListAsync();

            return result;
        }
    }
}
