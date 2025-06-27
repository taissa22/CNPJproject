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
    public class CredorCompromissoRepository : BaseCrudRepository<CredorCompromisso, long>, ICredorCompromissoRepository {
        private readonly DbContext dbContext;
        public CredorCompromissoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user) {
            this.dbContext = context;
        }

        public async Task<CredorCompromisso> ObterCredorCompromisso(long codigoProcesso, long codigoCredorCompromisso) {
            return await dbContext.Set<CredorCompromisso>()
                .Where(cc => cc.Id == codigoCredorCompromisso
                             && cc.CodigoProcesso == codigoProcesso)
                .FirstOrDefaultAsync();
        }
        public async Task<CredorCompromisso> ObterCredorCompromisso(long codigoProcesso)
        {
            return await dbContext.Set<CredorCompromisso>()
                .Where(cc => cc.CodigoProcesso == codigoProcesso)
                .FirstOrDefaultAsync();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }
    }
}
