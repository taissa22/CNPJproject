using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.DTO;
using Perlink.Oi.Juridico.Domain.Relatorios.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Service
{
    public interface IGrupoEmpresaContabilSapService : IBaseCrudService<GrupoEmpresaContabilSap, long>
    {
        void Atualizar(IList<GrupoEmpresaContabilSapDTO> grp);
    }
}