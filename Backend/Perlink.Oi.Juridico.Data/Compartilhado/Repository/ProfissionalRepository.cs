using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class ProfissionalRepository : BaseCrudRepository<Profissional, long>, IProfissionalRepository
    {
        private readonly JuridicoContext dbContext;
        public ProfissionalRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public override async Task<IDictionary<string, string>> RecuperarDropDown()
        {
            // TODO: Implementar dropdown
            return await dbContext.Set<Profissional>()
                    .AsNoTracking()
                    .OrderBy(x => x.NomeProfissional)
                    .ToDictionaryAsync(x => x.Id.ToString(),
                                       x => x.NomeProfissional);
        }

        public async Task<IEnumerable<Profissional>> RecuperarTodosEscritorios()
        {
            return await dbContext.Set<Profissional>()
                .Where(p => p.IndicadorEscritorio.Value)
                .AsNoTracking()
                .OrderBy(x => x.NomeProfissional)
                .ToListAsync();
        }

        public async Task<IEnumerable<Profissional>> RecuperarTodosProfissionais()
        {
            return await dbContext.Set<Profissional>()
                .Where(p => !p.IndicadorEscritorio.Value && !p.IndicadorAdvogado.Value)
                .AsNoTracking()
                .OrderBy(x => x.NomeProfissional)
                .ToListAsync();
        }

        public async Task<bool> ExisteGrupoLoteJuizadoComEscritorio(long codigoGrupoLoteJuizado)
        {
            return await dbContext.Profissional.CountAsync(e => e.CodigoGrupoLoteJuizado == codigoGrupoLoteJuizado) > 0;
        }
    }
}
