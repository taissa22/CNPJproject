using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
 
    public interface IEmpresas_SapService : IBaseCrudService<Empresas_Sap, long>
    {
        Task<IEnumerable<EmpresaSapDTO>> RecuperarEmpresasPorFiltro(Empresas_SapFiltroDTO FiltroDTO);
        Task<ICollection<EmpresaSapDTO>> ExportarEmpresasSap(Empresas_SapFiltroDTO filtroDTO);
        Task<bool> EmpresaComSiglaJaCadastrada(Empresas_Sap empresa);
        Task CadastrarEmpresa(Empresas_Sap empresa);
        Task<Empresas_Sap> AtualizarEmpresa(Empresas_Sap empresa);
        Task ExcluirEmpresasSap(long codigoFornecedor);
        Task<int> ObterQuantidadeTotalPorFiltro(Empresas_SapFiltroDTO filtroDTO);
        Task<bool> EmpresaAssociadaNaEmpresaDoGrupo(Empresas_Sap entidade);
    }
}
