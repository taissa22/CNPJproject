using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IEscritorioAppService : IBaseCrudAppService<EscritorioViewModel, Profissional, long>
    {
        Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperarAreaCivelConsumidor();
        Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperarAreaProcon();
        Task<IResultadoApplication<IEnumerable<EscritorioListaViewModel>>> RecuperarAreaCivelEstrategico();
    }
}
