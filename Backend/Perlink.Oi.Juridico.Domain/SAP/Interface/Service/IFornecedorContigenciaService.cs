using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface IFornecedorContigenciaService : IBaseCrudService<Fornecedor, long>
    {
        Task<ICollection<FornecedorContigenciaResultadoDTO>> ConsultarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
        Task<ICollection<FornecedorContigenciaExportarDTO>> ExportarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
        Task<int> RecuperarTotalRegistros(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
    }
}
