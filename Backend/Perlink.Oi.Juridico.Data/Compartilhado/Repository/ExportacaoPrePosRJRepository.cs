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
    public class ExportacaoPrePosRJRepository : BaseCrudRepository<ExportacaoPrePosRJ, long>, IExportacaoPrePosRJRepository
    {
        public ExportacaoPrePosRJRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
        }

        public async Task<IEnumerable<ExportacaoPrePosRJ>> ListarExportacaoPrePosRj(DateTime? dataExtracao, int pagina, int qtd)
        {
            return await context.Set<ExportacaoPrePosRJ>()
                              .AsNoTracking()
                              .Where(x => x.DataExtracao == dataExtracao || dataExtracao == null)
                              .OrderByDescending(c => c.Id)
                              .Skip((pagina - 1) * qtd)
                              .Take(qtd)
                              .ToListAsync();
        }

        public async Task<int> QuantidadeTotal(DateTime? dataExtracao)
        {
            return await context.Set<ExportacaoPrePosRJ>()
                             .AsNoTracking()
                              .Where(x => x.DataExtracao == dataExtracao || dataExtracao == null)
                             .CountAsync();
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new System.NotImplementedException();
        }
    }
}
