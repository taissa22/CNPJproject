using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class FornecedorasContratosService : BaseCrudService<FornecedorasContratos, long>, IFornecedorasContratosService
    {
        private readonly IFornecedorasContratosRepository FornecedorasContratosRepository;


        public FornecedorasContratosService(IFornecedorasContratosRepository FornecedorasContratosRepo) : base(FornecedorasContratosRepo)
        {
            this.FornecedorasContratosRepository = FornecedorasContratosRepo;
        }

        public async Task<bool> ExisteFornecedorasContratosComEmpresaSap(long codigoFornecedor)
        {
            return await FornecedorasContratosRepository.ExisteFornecedorasContratosComEmpresaSap(codigoFornecedor);
        }
    }
}
