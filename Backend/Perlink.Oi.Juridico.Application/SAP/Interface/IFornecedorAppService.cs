using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface IFornecedorAppService : IBaseCrudAppService<FornecedorViewModel, Fornecedor, long>
    {
        //Fornecedor RecuperarFornecedor(long Cod);

        Task<IPagingResultadoApplication<ICollection<FornecedorResultadoViewModel>>> RecuperarFornecedorPorFiltro(FornecedorFiltroDTO fornecedorFiltroDTO);
        Task<IResultadoApplication<byte[]>> ExportarFornecedores(FornecedorFiltroDTO fornecedorFiltroDTO);
        Task<IResultadoApplication> CadastrarFornecedor(FornecedorCriacaoViewModel fornecedorCriacaoViewModel);
        Task<IResultadoApplication> ExcluirFornecedor(long codigoFornecedor);
        Task<int> ObterQuantidadeTotalPorFiltro(FornecedorFiltroDTO loteFiltroDTO);
        Task<IResultadoApplication> AtualizarFornecedor(FornecedorAtualizarViewModel fornecedorAtualizarViewModel);
    }
}
