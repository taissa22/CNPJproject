using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Shared.Domain.Interface;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.Compartilhado.Repository
{
    [Obsolete]
    public class SequencialRepository : ISequencialRepository
    {
        private readonly JuridicoContext dbContext;

        public SequencialRepository(JuridicoContext dbContext, IAuthenticatedUser user)
        {
            this.dbContext = dbContext;
        }

        public async Task<long> RecuperarSequencialAsync(string codTabela)
        {
            var seq = await dbContext.Sequencial
                            .Where(filtro => filtro.Id.ToUpper().Equals(codTabela))
                            .AsNoTracking()
                            .Select(x => x.ValorDaSequence)
                            .FirstOrDefaultAsync();

            seq++;

            return seq;
        }

        public async Task AtualizarSequence(string codTabela)
        {
            var sequencial = new Sequencial(codTabela.ToUpper(), await RecuperarSequencialAsync(codTabela));

            dbContext.Entry(sequencial).State = EntityState.Modified;

            await Task.CompletedTask;
        }
    }
}
