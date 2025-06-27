using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.Filtros;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IProfissionalAppService : IBaseCrudAppService<ProfissionalDropDownViewModel, Profissional, long>
    {
        Task<IResultadoApplication<ICollection<ProfissionalDropDownViewModel>>> RecuperarTodosProfissionais();

        Task<IResultadoApplication<ICollection<ProfissionalDropDownViewModel>>> RecuperarTodosEscritorios();
    }
}
