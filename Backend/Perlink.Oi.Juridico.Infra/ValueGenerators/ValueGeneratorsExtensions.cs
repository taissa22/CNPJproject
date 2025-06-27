using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Linq.Expressions;

namespace Perlink.Oi.Juridico.Infra.ValueGenerators
{
    internal static class ValueGeneratorsExtensions
    {
        public static PropertyBuilder HasSequentialIdGenerator<TEntity>(this PropertyBuilder builder, string tabela) where TEntity : class, IEntity {
            builder
                .ValueGeneratedOnAdd()
                .HasValueGenerator((a, b) => new SequentialIdGenerator<TEntity>(tabela));

            return builder;
        }

        public static PropertyBuilder HasNextSequenceValueGenerator(this PropertyBuilder builder, string schema, string sequenceName)
        {
            builder
                .ValueGeneratedOnAdd()
                .HasValueGenerator((a, b) => new NextSequenceValueGenerator(schema, sequenceName));

            return builder;
        }

        public static PropertyBuilder HasAutoIncrementValueGenerator<TEntity>(this PropertyBuilder builder, Expression<Func<TEntity, int>> expression, int incrementFactor = 1) where TEntity : class, IEntity {
            builder
                .HasValueGenerator((a, b) => new AutoIncrementValueGenerator<TEntity>(expression, incrementFactor));

            return builder;
        }
    }
}