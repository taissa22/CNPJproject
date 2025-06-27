using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IEmpresas_SapRepository : IBaseCrudRepository<Empresas_Sap, long>
    {
        Task<IEnumerable<EmpresaSapDTO>> RecuperarEmpresasPorFiltro(Empresas_SapFiltroDTO FiltroDTO);
        Task<ICollection<EmpresaSapDTO>> ExportarEmpresasSap(Empresas_SapFiltroDTO filtroDTO);
        Task<bool> EmpresaComSiglaJaCadastrada(Empresas_Sap empresa);
        Task CadastrarEmpresa(Empresas_Sap model);
        Task<Empresas_Sap> AtualizarEmpresa(Empresas_Sap model);
        Task<int> ObterQuantidadeTotalPorFiltro(Empresas_SapFiltroDTO filtroDTO);
        Task<bool> EmpresaAssociadaNaEmpresaDoGrupo(Empresas_Sap entidade);
    }
}
