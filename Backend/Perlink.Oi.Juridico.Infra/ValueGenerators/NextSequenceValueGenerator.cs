using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Diagnostics.CodeAnalysis;

namespace Perlink.Oi.Juridico.Infra.ValueGenerators
{
    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "Value generated from code")]
    internal class NextSequenceValueGenerator : ValueGenerator<int>
    {
        private readonly string schema;
        private readonly string sequenceName;

        public NextSequenceValueGenerator(string schema, string sequenceName)
        {
            this.schema = schema;
            this.sequenceName = sequenceName;
        }

        public override bool GeneratesTemporaryValues => false;

        public override int Next(EntityEntry entry)
        {
            using var command = entry.Context.Database.GetDbConnection().CreateCommand();
            command.CommandText = $"SELECT {schema}.{sequenceName}.NEXTVAL FROM DUAL";
            entry.Context.Database.OpenConnection();
            using var reader = command.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0);
        }
    }
}