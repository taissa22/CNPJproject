using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Compartilhado.Interface
{
    public interface IEstadoAppService
    {
        Task<IResultadoApplication<IEnumerable<EstadoDTO>>> RecuperarEstados();
    }
}
