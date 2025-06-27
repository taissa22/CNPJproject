using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class EmpresasSapFornecedorasRepository : BaseCrudRepository<EmpresasSapFornecedoras, long>, IEmpresasSapFornecedorasRepository
    {
        private readonly JuridicoContext dbContext;

        public EmpresasSapFornecedorasRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExisteEmpresaSapFornecedorasComEmpresaSap(long codigoEmpresasSap)
        {
            var resultado = await context.Set<EmpresasSapFornecedoras>()
                .AnyAsync(l => l.CodigoEmpresaSap == codigoEmpresasSap);
            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
    }
}

