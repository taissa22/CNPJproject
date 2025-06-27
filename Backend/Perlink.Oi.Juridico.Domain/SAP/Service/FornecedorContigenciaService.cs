using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class FornecedorContigenciaService : BaseCrudService<Fornecedor, long>, IFornecedorContigenciaService
    {
        private readonly IFornecedorContigenciaRepository repository;

        public FornecedorContigenciaService(IFornecedorContigenciaRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<ICollection<FornecedorContigenciaResultadoDTO>> ConsultarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            return await repository.ConsultarFornecedorContigencia(fornecedorContigenciaConsultaDTO);
        }

        public Task<ICollection<FornecedorContigenciaExportarDTO>> ExportarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            return repository.ExportarFornecedorContigencia(fornecedorContigenciaConsultaDTO);
        }

        public async Task<int> RecuperarTotalRegistros(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO)
        {
            return await repository.RecuperarTotalRegistros(fornecedorContigenciaConsultaDTO);
        }
    }
}