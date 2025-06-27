using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service {
    public class PedidoSAPService : BaseCrudService<PedidoSAP, long>, IPedidoSAPService {
        private readonly IPedidoSAPRepository pedidoSAPRepository;


        public PedidoSAPService(IPedidoSAPRepository pedidoSAPRepository) : base(pedidoSAPRepository) {
            this.pedidoSAPRepository = pedidoSAPRepository;
        }

        public async Task<bool> ExistePedidoSAPComFornecedor(long codigoFornecedor) {
            return await pedidoSAPRepository.ExistePedidoSAPComFornecedor(codigoFornecedor);
        }
    }
}
