using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Repository {
    public interface IAgendarApuracaoOutlierRepository : IBaseCrudRepository<AgendarApuracaoOutliers, long> {
        Task<AgendarApuracaoOutliers> AgendarApuracaoOutliers(AgendarApuracaoOutliers agendarApuracaoOutliers);        
        void RemoverAgendamento(AgendarApuracaoOutliers agendamento);
        Task<IEnumerable<ListarAgendamentosApuracaoOutliersDTO>> CarregarAgendamento(int pagina, int qtd);
        Task<int> ObterQuantidadeTotal();
        Task<ICollection<AgendarApuracaoOutliers>> ObterAgendados();  
        Task<ApuracaoOutliersDownloadResultadoDTO> ObterResultadoDoAgendamento(long id);
        Task<AgendarApuracaoOutliers> RealizarCalculos(AgendarApuracaoOutliers agendamento);
    }
}
