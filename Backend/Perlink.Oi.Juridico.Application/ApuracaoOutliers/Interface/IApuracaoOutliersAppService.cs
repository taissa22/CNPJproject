using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Application.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ApuracaoOutliers.Interface {
    public interface IApuracaoOutliersAppService : IBaseCrudAppService<AgendarApuracaoOutliersViewModel, AgendarApuracaoOutliers, long> {

        Task<IResultadoApplication<ICollection<FechamentoJecDisponivelViewModel>>> CarregarFechamentosDisponiveisParaAgendamento();
        Task<IResultadoApplication>  AgendarApuracaoOutliers(AgendarApuracaoOutliersDTO dto);
        Task<IResultadoApplication> RemoverAgendamento(long id);
        Task<IPagingResultadoApplication<ICollection<AgendarApuracaoOutliersViewModel>>> CarregarAgendamento(int pagina, int qtd);
        Task<Stream> Download(string fileName);

        #region Executor
        Task ExecutarAgendamentos(ILogger logger);
        #endregion Executor
    }
}
