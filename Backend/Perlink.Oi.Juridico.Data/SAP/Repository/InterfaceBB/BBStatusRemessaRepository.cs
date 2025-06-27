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
    public class BBStatusRemessaRepository : BaseCrudRepository<BBStatusRemessa, long>, IBBStatusRemessaRepository
    {
        private readonly JuridicoContext dbContext;

        public BBStatusRemessaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }

        public long RecuperarIdBBStatusRemessa(long codigoBBEstado)
        {
            return dbContext.BBStatusRemessa
                .Where(r => r.Codigo == codigoBBEstado)
                .Select(r => r.Id).FirstOrDefault();

        }
    }
}

