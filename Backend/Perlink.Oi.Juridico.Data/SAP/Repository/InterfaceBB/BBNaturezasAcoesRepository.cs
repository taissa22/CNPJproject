using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.InterfaceBB
{
    public class BBNaturezasAcoesRepository : BaseCrudRepository<BBNaturezasAcoes, long>, IBBNaturezasAcoesRepository
    {
        private readonly JuridicoContext dbContext;

        public BBNaturezasAcoesRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
        public async Task<bool> CodigoBBJaExiste(BBNaturezasAcoes bbNaturezas)
        {
            return await dbContext.BBNaturezasAcoes.AsNoTracking()
                        .AnyAsync(bb => bb.CodigoBB == bbNaturezas.CodigoBB && bb.Id != bbNaturezas.Id);
        }

        public async Task<BBNaturezasAcoes> RecuperarPorCodigoBB(long codigo)
        {
            return await dbContext.BBNaturezasAcoes.AsNoTracking()
                        .FirstOrDefaultAsync(bb => bb.CodigoBB == codigo);
        }
    }
}

