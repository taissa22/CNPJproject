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
    public class StatusCompromissoParcelaRepository : BaseCrudRepository<StatusCompromissoParcela, long>, IStatusCompromissoParcelaRepository {
        private readonly JuridicoContext dbContext;
        public StatusCompromissoParcelaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user) {
            this.dbContext = dbContext;
        }

        public async Task<bool> Existe(DateTime data) {
            var dados = await dbContext.Set<Feriados>().Where(f => f.Data == data).ToListAsync();
            return dados.Any();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }
    }
}
