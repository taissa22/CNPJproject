using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;

namespace Perlink.Oi.Juridico.Application.Processos.Adapters
{
    public class SortOrderAdapter
    {
        public static SortOrder ToDTO(SortOrderVM viewModel)
        {
            return new SortOrder
            {
                Direction = viewModel.Direction,
                Property = viewModel.Property
            };
        }
    }
}
