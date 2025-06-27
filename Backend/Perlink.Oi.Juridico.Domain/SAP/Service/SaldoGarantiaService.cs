using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class SaldoGarantiaService : BaseCrudService<AgendamentoSaldoGarantia, long>, ISaldoGarantiaService
    {
        private readonly ISaldoGarantiaRepository repository;
        private readonly ICriterioSaldoGarantiaRepository criteriosRepository;
        private readonly IAuthenticatedUser user;

        public SaldoGarantiaService(ICriterioSaldoGarantiaRepository criteriosRepository, ISaldoGarantiaRepository repository, IAuthenticatedUser user) : base(repository)
        {
            this.criteriosRepository = criteriosRepository;
            this.repository = repository;
            this.user = user;
        }

        public async Task<ICollection<AgendamentoResultadoDTO>> ConsultarAgendamentos(OrdernacaoPaginacaoDTO filtroDTO)
        {
            return await repository.ConsultarAgendamentos(filtroDTO);
        }

        public async Task<ICollection<KeyValuePair<string, string>>> ConsultarCriteriosPesquisa(long codigoAgendamento)
        {
            return await repository.ConsultarCriteriosPesquisa(codigoAgendamento);
        }

        public async Task<ICollection<SaldoGarantiaResultadoDTO>>ExecutarAgendamentoSaldoGarantia(AgendamentoSaldoGarantia agendamento)
        {
            return await repository.ExecutarAgendamentoSaldoGarantia(agendamento);            
        }

        public async Task<AgendamentoSaldoGarantia> RecuperarAgendamento(long codigoAgendamento)
        {
            return await repository.RecuperarAgendamento(codigoAgendamento);
        }

        public async Task<int> RecuperarTotalRegistros()
        {
            return await repository.RecuperarTotalRegistros();
        }

        public async Task SalvarAgendamento(SaldoGarantiaAgendamentoDTO filtroDTO)
        {
             await repository.SalvarAgendamento(filtroDTO);
        }
    }
}
