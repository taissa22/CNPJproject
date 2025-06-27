using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository {
    public class PedidoSAPRepository : BaseCrudRepository<PedidoSAP, long>, IPedidoSAPRepository {
        private readonly JuridicoContext dbContext;

        public PedidoSAPRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user) {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExistePedidoSAPComFornecedor(long codigoFornecedor) {
            var resultado = await context.Set<PedidoSAP>()
                .AnyAsync(l => l.CodigoFornecedor == codigoFornecedor.ToString());

            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }
    }
}
