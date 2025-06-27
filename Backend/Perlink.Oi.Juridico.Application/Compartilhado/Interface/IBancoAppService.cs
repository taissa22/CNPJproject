using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IBancoAppService : IBaseCrudAppService<BancoViewModel, Banco, long>
    {
        Task<IResultadoApplication<ICollection<BancoViewModel>>> RecuperarNomeBanco();
    }
}
