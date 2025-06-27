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
    public class TipoProcessoRepository : BaseCrudRepository<TipoProcesso, long>, ITipoProcessoRepository
    {
        private readonly JuridicoContext dbContext;
        public TipoProcessoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            // TODO: Implementar dropdown
            return await dbContext.TipoProcesso
                    .AsNoTracking()
                    .OrderBy(x => x.Id)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.Descricao);
        }

        public async Task<IEnumerable<TipoProcesso>> RecuperarTodosSAP(long[] codigoTipoProcessoSAP) {
            return await dbContext.TipoProcesso
                .Where(t => codigoTipoProcessoSAP.Contains(t.Id))
                .AsNoTracking()
                .OrderBy(t => t.Descricao)
                .ToListAsync();
        }
    }
}
