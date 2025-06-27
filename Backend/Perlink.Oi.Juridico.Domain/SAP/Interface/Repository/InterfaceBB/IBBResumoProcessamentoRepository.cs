using Perlink.Oi.Juridico.Domain.SAP.DTO.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB
{
    public interface IBBResumoProcessamentoRepository : IBaseCrudRepository<BBResumoProcessamento, long>
    {
        Task<ICollection<BBResumoProcessamentoResultadoDTO>> ConsultarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<ICollection<BBResumoProcessamentoGuiaDTO>> BuscarGuiasOK(long numeroLoteBB);
        Task<ICollection<BBResumoProcessamentoResultadoDTO>> ExportarArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<int> TotaisArquivoRetorno(BBResumoProcessamentoFiltroDTO filtroDTO);
        Task<BBResumoProcessamentoGuiaExibidaDTO> BuscarGuiaExibicao(long codigoProcesso, long codigoLancamento);
        Task<BBResumoProcessamentoGuiaDTO> RecuperarLancamentoProcessoDoArquivo(long codProcesso, long codLancamento, long codLote);
    }
}
