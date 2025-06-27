using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository.InterfaceBB
{
    public class BBStatusParcelasRepository : BaseCrudRepository<BBStatusParcelas, long>, IBBStatusParcelasRepository
    {
        private readonly JuridicoContext dbContext;

        public BBStatusParcelasRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
        /// <summary>
        /// Retorna o objeto de BBStatusParcelas com maior sequence a partir do codigo informado
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public async Task<BBStatusParcelas> RecuperarPorCodigoBBStatusParcela(long codigo)
        {            
            var ret = await dbContext.BBStatusParcelas
                 .AsNoTracking()
                 .Where(bb => bb.CodigoBB == codigo)
                 .LastOrDefaultAsync();
            return ret;
        }

        public async Task<bool> VerificarDuplicidadeCodigoBBStatusParcela(BBStatusParcelas obj)
        {
            var ret = await dbContext.BBStatusParcelas
             .AsNoTracking()
             .AnyAsync(cc => cc.CodigoBB == obj.CodigoBB &&
                                     cc.Id != obj.Id);
            return ret;
        }
    }
}

