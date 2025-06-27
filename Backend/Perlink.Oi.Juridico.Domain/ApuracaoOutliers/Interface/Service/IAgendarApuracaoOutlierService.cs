using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Service {
    public interface IAgendarApuracaoOutlierService : IBaseCrudService<AgendarApuracaoOutliers, long> {
        Task<AgendarApuracaoOutliers> AgendarApuracaoOutliers(AgendarApuracaoOutliers obj);
        void RemoverAgendamento(AgendarApuracaoOutliers agendamento);
        Task<IEnumerable<ListarAgendamentosApuracaoOutliersDTO>> CarregarAgendamento(int pagina, int qtd);
        Task<int> ObterQuantidadeTotal();
        Task<ApuracaoOutliersDownloadArquivoDTO> BaixarArquivosResultado(AgendarApuracaoOutliers model, Parametro parametroPastaDestinoArquivo);
        Task<ApuracaoOutliersDownloadArquivoDTO> DownloadBaseFechamento(int id);
        
        #region Executor
        Task TratandoAgendamentosInterrompidos();
        Task<ICollection<AgendarApuracaoOutliers>> ObterAgendados();
        Task<ApuracaoOutliersDownloadResultadoDTO> ObterResultadoDoAgendamento(long id);
        Task<AgendarApuracaoOutliers> RealizarCalculos(AgendarApuracaoOutliers agendamento);

        #endregion Executor
    }
}
