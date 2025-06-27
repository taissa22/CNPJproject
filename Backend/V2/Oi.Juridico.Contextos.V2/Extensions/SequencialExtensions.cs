using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oi.Juridico.Contextos.V2.ManutencaoContext.Data;
using Oi.Juridico.Contextos.V2.SequenceContext.Data;

namespace Oi.Juridico.Contextos.V2.Extensions
{
    public static class SequencialExtensions
    { 
        public static decimal GetNextSequencial(this ManutencaoDbContext context, string tabela)
        {
            var sequencial = context.Sequencial.FirstOrDefault(s => s.CodTabela == tabela);

            if (sequencial != null)
            {
                sequencial.ValSeq += 1;

                context.SaveChanges();

                return sequencial.ValSeq.Value;
            }
            else
            {
                throw new System.Exception("Sequencial inexistente");
            }

        }

        public static async Task<decimal> GetNextSequence(this SequenceDbContext context, string sequence, CancellationToken ct)
        {
            var sql = $"select jur.{sequence}.nextval as SequenceValue from dual";
            
            var sequencial = await context.OracleSequence.FromSqlRaw(sql).ToListAsync(ct);

            if (sequencial != null)
            {
                return sequencial[0].SequenceValue;
            }
            else
            {
                throw new System.Exception("Sequencial inexistente");
            }

        }
    }
}
