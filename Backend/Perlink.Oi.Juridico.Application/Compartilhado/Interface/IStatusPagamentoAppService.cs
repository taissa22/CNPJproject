using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IStatusPagamentoAppService : IBaseCrudAppService<StatusPagamentoViewModel, StatusPagamento, long>
    {
    }
}