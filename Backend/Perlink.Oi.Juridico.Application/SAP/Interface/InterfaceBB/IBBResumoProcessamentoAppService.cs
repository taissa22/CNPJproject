using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Application.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB
{
    public interface IBBResumoProcessamentoAppService : IBaseCrudAppService<BBResumoProcessamentoViewModel, BBResumoProcessamento, long>
    {
        Task<IResultadoApplication<ICollection<BBResumoProcessamentoGuiaViewModel>>> BuscarGuiasOK(long numeroLoteBB);
        Task<IResultadoApplication<ICollection<BBResumoProcessamentoResultadoViewModel>>> ConsultarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<IResultadoApplication<byte[]>> ExportarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<IResultadoApplication<byte[]>> ExportarGuias(long numeroLoteBB);
        Task<IResultadoApplication<BBResumoProcessamentoGuiaExibidaViewModel>> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento);
        Task<IResultadoApplication<List<BBResumoProcessamentoImportacaoViewModel>>> Upload(List<IFormFile> files);
        Task<IResultadoApplication> SalvarImportacao(List<BBResumoProcessamentoImportacaoDTO> importacaoDTO);
        Task<IResultadoApplication<ImportacaoParametroJuridicoUploadViewModel>> ConsultarParametrosUpload();
    }
}