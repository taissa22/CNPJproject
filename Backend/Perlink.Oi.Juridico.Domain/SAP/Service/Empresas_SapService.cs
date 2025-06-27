using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class Empresas_SapService : BaseCrudService<Empresas_Sap, long>, IEmpresas_SapService
    {
        private readonly IEmpresas_SapRepository repository;

        public Empresas_SapService(IEmpresas_SapRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task ExcluirEmpresasSap(long CodigoEmpresaSap)
        {
            var Empresa = await repository.RecuperarPorId(CodigoEmpresaSap);

            await repository.Remover(Empresa);
        }

        public Task<Empresas_Sap> AtualizarEmpresa(Empresas_Sap empresa)
        {
            return repository.AtualizarEmpresa(empresa);
        }

        public Task CadastrarEmpresa(Empresas_Sap empresa)
        {
            return repository.CadastrarEmpresa(empresa);
        }

        public Task<bool> EmpresaComSiglaJaCadastrada(Empresas_Sap empresa)
        {
            return repository.EmpresaComSiglaJaCadastrada(empresa);
        }

        public async Task<ICollection<EmpresaSapDTO>> ExportarEmpresasSap(Empresas_SapFiltroDTO filtroDTO)
        {
            return await repository.ExportarEmpresasSap(filtroDTO);
        }

        public async Task<int> ObterQuantidadeTotalPorFiltro(Empresas_SapFiltroDTO filtroDTO)
        {
            return await repository.ObterQuantidadeTotalPorFiltro(filtroDTO);
        }

        public Task<IEnumerable<EmpresaSapDTO>> RecuperarEmpresasPorFiltro(Empresas_SapFiltroDTO filtroDTO)
        {
            return repository.RecuperarEmpresasPorFiltro(filtroDTO);
        }

        public Task<bool> EmpresaAssociadaNaEmpresaDoGrupo(Empresas_Sap entidade)
        {
            return repository.EmpresaAssociadaNaEmpresaDoGrupo(entidade);
        }
    }
}