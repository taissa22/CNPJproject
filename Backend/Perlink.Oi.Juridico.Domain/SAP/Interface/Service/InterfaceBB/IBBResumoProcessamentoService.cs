using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB
{
    public interface IBBResumoProcessamentoService : IBaseCrudService<BBResumoProcessamento, long>
    {
        Task<ICollection<BBResumoProcessamentoResultadoDTO>> ConsultarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<ICollection<BBResumoProcessamentoResultadoDTO>> ExportarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<ICollection<BBResumoProcessamentoGuiaDTO>> BuscarGuiasOK(long numeroLoteBB);
        Task<int> TotaisArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<BBResumoProcessamentoGuiaExibidaDTO> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento);
        Task<BBResumoProcessamentoImportacaoDTO> RecuperarDadosImportacao(IFormFile file);
        Task SalvarImportacao(BBResumoProcessamentoImportacaoDTO dto);
        Task<int> ConsultarParametroMaxArquivosUpload();
        Task<long> ConsultarParametroTamanhoArquivosUpload();
    }
}