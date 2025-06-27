using Perlink.Oi.Juridico.Application.SAP.ViewModel;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.ManutençãoFornecedorContigencia;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFornecedores;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface
{
    public interface IFornecedorContigenciaAppService : IBaseCrudAppService<FornecedorViewModel, Fornecedor, long>
    {
        Task<IPagingResultadoApplication<ICollection<FornecedorContigenciaResultadoViewModel>>> ConsultarFornecedorContigencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
        Task<IResultadoApplication<byte[]>> ExportarFornecedorContingencia(FornecedorContigenciaConsultaDTO fornecedorContigenciaConsultaDTO);
        Task<IResultadoApplication>AtualizarFornecedorContigencia(FornecedorContigenciaAtualizaViewModel fornecedorAtualizar);
    }
}