using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface ICriterioSaldoGarantiaRepository : IBaseCrudRepository<CriterioSaldoGarantia, long>
    {
        Task CriarCriteriosAsync(AgendamentoSaldoGarantia agendamento, SaldoGarantiaAgendamentoDTO filtroDTO);
        Task<ICollection<CriterioSaldoGarantia>> RecuperarPorAgendamento(long codigoAgendamento);
    }
}
