using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class FornecedorasContratosRepository : BaseCrudRepository<FornecedorasContratos, long>, IFornecedorasContratosRepository
    {
        private readonly JuridicoContext dbcontext;
        public FornecedorasContratosRepository(JuridicoContext dbcontext, IAuthenticatedUser user) : base(dbcontext, user)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<bool> ExisteFornecedorasContratosComEmpresaSap(long codigoEmpresaSap)
        {
            var resultado = await context.Set<FornecedorasContratos>()
                .AnyAsync(l => l.CodigoEmpreSap == codigoEmpresaSap);
            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }


    }
}
