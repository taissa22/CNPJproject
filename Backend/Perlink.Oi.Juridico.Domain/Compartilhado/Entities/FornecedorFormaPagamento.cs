using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class FornecedorFormaPagamento : EntityCrud<FornecedorFormaPagamento, long> {
        public override AbstractValidator<FornecedorFormaPagamento> Validator => new FornecedorFormaPagamentoValidator();

        public long CodigoFormaPagamento { get; set; }
        public string CodigoFormaPagamentoSAP { get; set; }


        public override void PreencherDados(FornecedorFormaPagamento data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }

        internal class FornecedorFormaPagamentoValidator : AbstractValidator<FornecedorFormaPagamento> {
            public FornecedorFormaPagamentoValidator() {
                
            }
        }
    }
}
