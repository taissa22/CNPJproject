using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface ISaldoGarantiaService : IBaseCrudService<AgendamentoSaldoGarantia, long>
    {
        Task<ICollection<SaldoGarantiaResultadoDTO>> ExecutarAgendamentoSaldoGarantia(AgendamentoSaldoGarantia agendamento);
        Task<ICollection<KeyValuePair<string, string>>> ConsultarCriteriosPesquisa(long codigoAgendamento);
        Task SalvarAgendamento(SaldoGarantiaAgendamentoDTO filtroDTO);
        Task<ICollection<AgendamentoResultadoDTO>> ConsultarAgendamentos(OrdernacaoPaginacaoDTO filtroDTO);
        Task<int> RecuperarTotalRegistros();
        Task<AgendamentoSaldoGarantia> RecuperarAgendamento(long codigoAgendamento);
    }
}
