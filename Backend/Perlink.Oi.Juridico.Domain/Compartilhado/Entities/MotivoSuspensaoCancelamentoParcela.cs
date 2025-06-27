using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class MotivoSuspensaoCancelamentoParcela : EntityCrud<MotivoSuspensaoCancelamentoParcela, long> {

        public string DescricaoMotivoSuspencaoCancelamentoParcela { get; set; }
        public bool IndicaSuspensao { get; set; }
        public bool IndicaCancelamento { get; set; }
        public bool IndicaAtivo { get; set; }
        public CompromissoProcessoParcela CompromissoProcessoParcela { get; set; }
        public override AbstractValidator<MotivoSuspensaoCancelamentoParcela> Validator => new MotivoSuspensaoCancelamentoParcelaValidator();

        public override void PreencherDados(MotivoSuspensaoCancelamentoParcela data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }

        internal class MotivoSuspensaoCancelamentoParcelaValidator : AbstractValidator<MotivoSuspensaoCancelamentoParcela> {
            public MotivoSuspensaoCancelamentoParcelaValidator() {



            }
        }
    }
}
