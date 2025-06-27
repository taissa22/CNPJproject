using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class FornecedorFormaPagamentoRepository : BaseCrudRepository<FornecedorFormaPagamento, long>, IFornecedorFormaPagamentoRepository {
        private readonly JuridicoContext dbContext;

        public FornecedorFormaPagamentoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user) {
            this.dbContext = context;
        }

        public async Task<bool> ExisteFormaPagamentoComFornecedor(long codigoFornecedor) {
            var resultado = await context.Set<FornecedorFormaPagamento>()
                .AnyAsync(l => l.Id == codigoFornecedor);

            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }
    }
}
