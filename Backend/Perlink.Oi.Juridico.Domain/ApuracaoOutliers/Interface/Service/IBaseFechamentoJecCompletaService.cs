using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Service {
    public interface IBaseFechamentoJecCompletaService : IBaseCrudService<BaseFechamentoJecCompleta, long> {
        Task<IEnumerable<FechamentoJecDisponivelDTO>> CarregarFechamentosDisponiveisParaAgendamento();
        Task<ICollection<ApuracaoOutliersDownloadBaseFechamentoDTO>> ListarBaseFechamento(long codEmpCentralizadora, DateTime mesAnoFechamento, DateTime dataFechamento);
        Task<ICollection<ListaProcessosBaseFechamentoJecDTO>> ListarProcessosResultado(long codEmpCentralizadora, DateTime mesAnoFechamento, DateTime dataFechamento);
    }
}
