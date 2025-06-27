using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados;

namespace Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Service
{
    public interface IGrupoEstadosService : IBaseCrudService<Entity.GrupoEstados, long>
    {

    }
}
