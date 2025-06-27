using FluentValidation;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity {
    public class AgendamentoSaldoGarantia : EntityCrud<AgendamentoSaldoGarantia, long>  {
        public override AbstractValidator<AgendamentoSaldoGarantia> Validator => new AgendamentoSaldoGarantiaValidator();

        //public long CodigoAgendamento { get; set; }
        public long CodigoTipoProcesso { get; set; }
        public DateTime DataAgendamento { get; set; }
        public string NomeAgendamento { get; set; }
        public DateTime? DataExecucao { get; set; }
        public DateTime? DataFinalizacao { get; set; }
        public string CodigoUsuario { get; set; }
        public long CodigoStatusAgendamento { get; set; }
        public string NomeArquivoGerado { get; set; }
        public string MensagemErro { get; set; }
        public string MensagemErroTrace { get; set; }

        public IList<CriterioSaldoGarantia> CriteriosSaldoGarantias { get; set; }

        public override void PreencherDados(AgendamentoSaldoGarantia data) {            
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class AgendamentoSaldoGarantiaValidator : AbstractValidator<AgendamentoSaldoGarantia> {
            public AgendamentoSaldoGarantiaValidator() {
            }
        }
    }
}
