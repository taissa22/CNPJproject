using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class EmpresasSapFornecedorasService : BaseCrudService<EmpresasSapFornecedoras, long>, IEmpresasSapFornecedorasService
    {
        private readonly IEmpresasSapFornecedorasRepository repository;
        public EmpresasSapFornecedorasService(IEmpresasSapFornecedorasRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<bool> ExisteEmpresaSapFornecedorasComEmpresaSap(long codigoEmpresasSap)
        {
            return await repository.ExisteEmpresaSapFornecedorasComEmpresaSap(codigoEmpresasSap);
        }

        //public async Task<EmpresasSapFornecedoras> RecuperarEmpresasSapFornecedoras(long Cod)
        //{
        //    return await repository.RecuperarEmpresasSapFornecedoras(Cod);
        //}
    }
}
