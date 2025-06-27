
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class Filter
    {
        public string FieldName { get; set; }

        public string Value { get; set; }

        public string Value2 { get; set; }

        public FilterOperatorEnum FilterOperator { get; set; }
    }
}
