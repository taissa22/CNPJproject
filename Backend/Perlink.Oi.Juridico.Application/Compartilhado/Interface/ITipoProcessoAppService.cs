using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface ITipoProcessoAppService : IBaseCrudAppService<TipoProcessoViewModel, TipoProcesso, long> {
        Task<IResultadoApplication<ICollection<TipoProcessoViewModel>>> RecuperarTodosSAP(string tela);
    }
}