using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Processos.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Processos.EFRepositories
{
    public class AudienciaProcessoRepository : BaseCrudRepository<AudienciaProcesso, long>, IAudienciaProcessoRepository
    {
        private readonly JuridicoContext dbContext;

        public AudienciaProcessoRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user)
        {
            dbContext = context;
        }

        public async Task<AudienciaProcesso> ObterPorChavesCompostas(long codProcesso, long seqAudiencia)
        {
            //var t = dbContext.AudienciaProcesso
            //                      .Where(x => x.Id == codProcesso && x.SequenciaAudiencia == seqAudiencia)
            //                      .ToSql();
            var teste = await dbContext.AudienciaProcesso
                                  .Where(x => x.Id == codProcesso && x.SequenciaAudiencia == seqAudiencia)
                                  .FirstOrDefaultAsync();

            return teste;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }
    }
}
