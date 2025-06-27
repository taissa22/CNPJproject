using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class CompromissoProcessoCredorRepository : BaseCrudRepository<CompromissoProcessoCredor, long>, ICompromissoProcessoCredorRepository
    {
        private readonly JuridicoContext dbContext;
        public CompromissoProcessoCredorRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<CompromissoProcessoCredor> ObterCompromissoProcessoCredor(long codigoProcesso, long codigoCompromisso) {
            return await dbContext.Set<CompromissoProcessoCredor>()                
                .Where(cpc => cpc.CodigoProcesso == codigoProcesso
                              && cpc.Id == codigoCompromisso)
                .FirstOrDefaultAsync();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

    }
}
