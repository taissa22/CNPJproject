using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.ViewModel;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.DTO;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.GrupoDeEstados.Interface
{
    public interface IGrupoDeEstadosAppService : IBaseCrudAppService<GrupoEstadosViewModel, Domain.GrupoDeEstados.Entity.GrupoDeEstados, long>
    {
        CommandResult Atualizar(IList<GrupoDeEstadosDTO> grupos);

        Task<IResultadoApplication<AgregadoGrupoDeEstadosViewModel>> ListarGrupoDeEstados();

        Task<IList<Domain.GrupoDeEstados.Entity.GrupoDeEstados>> Exportar();
    }
}
