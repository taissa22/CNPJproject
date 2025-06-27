using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.ViewModel;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Interface
{
    public interface IGrupoEmpresaContailSapAppService : IBaseCrudAppService<GrupoXEmpresaViewModel, GrupoEmpresaContabilSap, long>
    {
        CommandResult Atualizar(IList<GrupoEmpresaContabilSapDTO> grupos);

        Task<IResultadoApplication<AgregadoGrupoContabilSapViewModel>> ListarGrupoEmpresaContabilSap();

        Task<IList<GrupoEmpresaContabilSap>> Exportar();
    }
}