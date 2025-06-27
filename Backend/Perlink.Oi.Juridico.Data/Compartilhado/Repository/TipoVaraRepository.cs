using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class TipoVaraRepository : BaseCrudRepository<TipoVara, long>, ITipoVaraRepository
    {
        private readonly JuridicoContext dbContext;
        public TipoVaraRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TipoVara>> RecuperarPorVaraEComarca(long codigoComarca, long codigoVara)
        {
            var resultQuery = dbContext.TipoVaraProcessos.Join(
                    dbContext.Varas,
                    tv => tv.Id,
                    v => v.CodigoTipoVara,
                    (tv, v) => new { TipoVara = tv, Vara = v }
                )
                .Where(x => x.Vara.Id == codigoComarca)
                .Where(x => x.Vara.CodigoVara == codigoVara)
                .Where(x => x.TipoVara.IndicadorTrabalhista == true)
                .Select(x => x.TipoVara);

            return await resultQuery.ToListAsync();
        }
    }
}
