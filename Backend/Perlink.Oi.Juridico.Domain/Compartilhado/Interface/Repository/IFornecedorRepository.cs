using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IFornecedorRepository : IBaseCrudRepository<Fornecedor, long>
    {
        Task<IEnumerable<FornecedorResultadoDTO>> FiltroConsultaFornecedor(FornecedorFiltroDTO filtros);

        Task<ICollection<FornecedorExportarDTO>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO);
        Task<Fornecedor> FornecedorComCodigoSAPJaCadastrado(string codigoSAP);
        Task<Fornecedor> CadastrarFornecedor(Fornecedor fornecedor);
        Task<int> ObterQuantidadeTotalPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO);
        Task<Fornecedor> AtualizarFornecedor(Fornecedor modelo);
        Task<Fornecedor> VerificarCodigoSap(string codigoSAP, long CodigoFornecedor);
        Task<IEnumerable<FornecedorDTOFiltro>> RecuperarFornecedorParaFiltroLote();
    }
}