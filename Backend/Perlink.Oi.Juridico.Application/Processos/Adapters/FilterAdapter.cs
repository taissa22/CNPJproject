using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;

namespace Perlink.Oi.Juridico.Application.Processos.Adapters
{
    public class FilterAdapter
    {
        public static Filter ToDTO(FilterVM viewModel)
        {
            return new Filter
            {
                FieldName = viewModel.FieldName,
                Value = viewModel.Value,
                Value2 = viewModel.Value2,
                FilterOperator = (FilterOperatorEnum)((short)viewModel.FilterOperator)
            };
        }
    }
}
