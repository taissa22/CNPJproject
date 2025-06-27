using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Perlink.Oi.Juridico.Infra.ValueGenerators {
    internal class AutoIncrementValueGenerator<TEntity> : ValueGenerator<int> where TEntity : class, IEntity {
        public AutoIncrementValueGenerator(Expression<Func<TEntity, int>> expression, int incrementFactor) {
            this.expression = expression;
            this.incrementFactor = incrementFactor;
        }

        public override bool GeneratesTemporaryValues => false;

        private readonly Expression<Func<TEntity, int>> expression;
        private readonly int incrementFactor;

        public override int Next(EntityEntry entry) {
            return entry.Context.Set<TEntity>().Max(expression) + incrementFactor;
        }
    }
}
