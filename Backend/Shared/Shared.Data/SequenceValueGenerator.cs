using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace Shared.Data
{
    public class SequenceValueGenerator : ValueGenerator<Int64>
    {
        private string _schema;
        private string _sequenceName;

        public SequenceValueGenerator(string schema, string sequenceName)
        {
            _schema = schema;
            _sequenceName = sequenceName;
        }

        public override bool GeneratesTemporaryValues => false;

        public override Int64 Next(EntityEntry entry)
        {
            using (var command = entry.Context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = $"SELECT {_schema}.{_sequenceName}.NEXTVAL FROM DUAL";
                entry.Context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    reader.Read();
                    var result = reader.GetInt64(0);
                    return result;
                }
            }
        }
    }
}
