using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
    public class ComarcaRepository : BaseCrudRepository<Comarca, long>, IComarcaRepository
    {
        private readonly JuridicoContext dbContext;

        public ComarcaRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<Comarca> RecuperarComarca(long CodigoComarca)
        {
            var Comarca = await dbContext.Comarca
              .Where(filtro => filtro.Id.Equals(CodigoComarca))
              .AsNoTracking()
              .FirstOrDefaultAsync();

            return Comarca;

        }

        public async Task<ICollection<ComarcaDTO>> RecuperarTodasComarca()
        {
            var Comarca = await dbContext.Comarca
                .Select(s => new ComarcaDTO
                {
                    Id = s.Id,
                    Descricao = s.CodigoEstado + " - " + s.Nome
                })
                .OrderBy(o => o.Descricao)
                .AsNoTracking()
                .ToListAsync();

            return Comarca;

        }

        public async Task<ICollection<ComarcaDTO>> RecuperarComarcaPorEstado(string estado)
        {
            var Comarca = await dbContext.Comarca
                .Where(filtro => filtro.CodigoEstado.Equals(estado))
                .Select(s => new ComarcaDTO
                {
                    Id = s.Id,
                    Descricao = s.Nome
                })
                .OrderBy(o => o.Descricao)
                .AsNoTracking()
                .ToListAsync();

            return Comarca;

        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
        public async Task<bool> ExisteBBComarcaAssociadoComarca(long codigoBBComarca)
        {
            return await dbContext.Comarca
              .AsNoTracking()
              .AnyAsync(c => c.CodigoBBComarca == codigoBBComarca);
        }

    }
}

