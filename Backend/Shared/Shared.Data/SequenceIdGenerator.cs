using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using System.Linq;

namespace Shared.Data
{
    public class SequenceIdGenerator : ValueGenerator<long>
    {
        private string _tabela;

        public SequenceIdGenerator(string tabela)
        {
            _tabela = tabela.ToUpper();
        }

        public override bool GeneratesTemporaryValues => false;

        public override long Next(EntityEntry entry)
        {
            var sequencial = entry.Context.Set<Sequencial>().FirstOrDefault(x => x.Id.Equals(_tabela));

            if (sequencial is null)
            {
                sequencial = new Sequencial(_tabela);
                entry.Context.Set<Sequencial>().Add(sequencial);
            }

            sequencial.IncrementaValor();

            return sequencial.ValorDaSequence;
        }
    }
}
