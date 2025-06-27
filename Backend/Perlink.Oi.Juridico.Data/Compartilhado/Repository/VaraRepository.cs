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
    public class VaraRepository : BaseCrudRepository<Vara, long>, IVaraRepository {
        private readonly JuridicoContext dbContext;
        public VaraRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user) {
            this.dbContext = dbContext;
        }

        public async Task<bool> ExisteBBOrgaoVinculado(long idBBOrgao) {
            return await dbContext.Varas.AnyAsync(v => v.IdBancoDoBrasilOrgao.Value == idBBOrgao);                
        }

        public async Task<bool> ExisteCodigoBBOrgaoVinculado(long codigoBBOrgao) {
            return await dbContext.Varas.Where(v => v.IdBancoDoBrasilOrgao.HasValue)
                .AnyAsync(v => v.BBOrgaos.Codigo == codigoBBOrgao);
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }
    }
}
