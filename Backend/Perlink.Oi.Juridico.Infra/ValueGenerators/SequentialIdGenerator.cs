using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.ValueGenerators
{
    internal class SequentialIdGenerator<TEntity> : ValueGenerator<int> where TEntity : IEntity
    {
        public SequentialIdGenerator(string tabela)
        {
            Tabela = tabela;
        }

        private string Tabela { get; }

        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            if (entry.Entity.GetType() == typeof(TEntity))
            {
                var sequencial = entry.Context.Set<Sequencial>().FirstOrDefault(x => x.Tabela.Equals(Tabela));

                if (sequencial is null)
                {
                    sequencial = new Sequencial(Tabela);
                    entry.Context.Set<Sequencial>().Add(sequencial);
                }

                sequencial.IncrementaValor();

                return sequencial.Valor;
            }

            return 0;
        }
    }
}