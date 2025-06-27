using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class StatusCompromissoParcela : EntityCrud<StatusCompromissoParcela, long> {
        
        public string DescricaoStatusParcela { get; set; }
        public bool IndicaAtivo { get; set; }
        public CompromissoProcessoParcela CompromissoProcessoParcela { get; set; }

        public override AbstractValidator<StatusCompromissoParcela> Validator => new StatusCompromissoParcelaValidator();

        public override void PreencherDados(StatusCompromissoParcela data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }

        internal class StatusCompromissoParcelaValidator : AbstractValidator<StatusCompromissoParcela> {
            public StatusCompromissoParcelaValidator() {



            }
        }
    }
}
