using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.Filtros;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IFornecedorService : IBaseCrudService<Fornecedor, long>
    {
        Task<IEnumerable<FornecedorResultadoDTO>> RecuperarFornecedorPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO);
        Task<ICollection<FornecedorExportarDTO>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO);

        Task<Fornecedor> CadastrarFornecedor(Fornecedor fornecedor);
        Task<Fornecedor> FornecedorComCodigoSAPJaCadastrado(string codigoSAP);
        
        Task ExcluirFornecedor(long codigoFornecedor);
        Task<int> ObterQuantidadeTotalPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO);
        Task<Fornecedor> AtualizarFornecedor(Fornecedor fornecedor);
        Task<Fornecedor> VerificarCodigoSap(string codigoSAP, long CodigoFornecedor);
        Task<IEnumerable<FornecedorDTOFiltro>> RecuperarFornecedorParaFiltroLote();
    }
}
