using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class FornecedorasFaturasRepository : BaseCrudRepository<FornecedorasFaturas, long>, IFornecedorasFaturasRepository
    {
        private readonly JuridicoContext dbcontext;
        public FornecedorasFaturasRepository(JuridicoContext dbcontext, IAuthenticatedUser user) : base(dbcontext, user)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<bool> ExisteFornecedorasFaturasComEmpresaSap(long codigoEmpresaSap)
        {
            var resultado = await context.Set<FornecedorasFaturas>()
                .AnyAsync(l => l.CodigoEmpresaSap == codigoEmpresaSap);
            return resultado;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

    }
}
