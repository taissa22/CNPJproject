using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface IFornecedorContigenciaRepository : IBaseCrudRepository<Fornecedor, long>
    {
        Task<ICollection<FornecedorContigenciaResultadoDTO>> ConsultarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
        Task<ICollection<FornecedorContigenciaExportarDTO>> ExportarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
        Task<int> RecuperarTotalRegistros(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
    }
}
