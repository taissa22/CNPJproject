using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CredorCompromisso : EntityCrud<CredorCompromisso, long> {

        public long CodigoCredor { get; set; }
        public long CodigoProcesso { get; set; }
        public long CodigoPlanoCompromisso { get; set; }
        public decimal ValorPagamentoCredor { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public long CodigoFornecedor { get; set; }
        public long CodigoCategoriaPagamento { get; set; }
        public string NomeBeneficiario { get; set; }
        public string NumeroCPFBeneficiario { get; set; }
        public string NumeroCNPJBeneficiario { get; set; }
        public string NumeroBancoBeneficiario { get; set; }
        public string DVBancoBeneficiario { get; set; }
        public string NumeroAgenciaBeneficiario { get; set; }
        public string DVAgenciaBeneficiario { get; set; }
        public string NumeroContaCorrenteBeneficiario { get; set; }
        public string DVContaCorrenteBeneficiario { get; set; }
        public string NomeCidadeBeneficiario { get; set; }
        public bool IndicaAtivo { get; set; }

        public override AbstractValidator<CredorCompromisso> Validator => new CredorCompromissoValidator();        

        public override void PreencherDados(CredorCompromisso data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }

        internal class CredorCompromissoValidator : AbstractValidator<CredorCompromisso> {
            public CredorCompromissoValidator() {



            }
        }
    }
}
