using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    public class ClassesGarantiasRepository : BaseCrudRepository<ClassesGarantias, long>, IClassesGarantiasRepository
    {
        private readonly JuridicoContext context;

        public ClassesGarantiasRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ComboboxDTO>> RecuperarClassesGarantias(long tipoLancamento)
        {
            return await context.Set<ClassesGarantias>()
                                     .AsNoTracking()
                                     .Where(cg => cg.CodigoTipoLancamento == tipoLancamento)
                                     .Select(cg => new ComboboxDTO() 
                                     {
                                         Id = cg.Id,
                                         Descricao = cg.Descricao
                                     })
                                     .OrderBy(e => e.Descricao)
                                     .ToListAsync();
        }



        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}
