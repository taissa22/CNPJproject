using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CompromissoProcesso : EntityCrud<CompromissoProcesso, long>
    {
        public override AbstractValidator<CompromissoProcesso> Validator => new CompromissoProcessoValidator();


        public long CodigoProcesso { get; set; }
        //public long CodigoStatusCompromisso { get; set; }
        //public long CodigoPlanoCompromisso { get; set; }
        public long CodigoFormaPagamento { get; set; }
        public long CodigoFornecedor { get; set; }
        public long CodigoCategoriaPagamento { get; set; }
        public bool? IndicadorEspolio { get; set; }
        public bool? IndicadorSindicato { get; set; }
        public string NomeBeneficiario { get; set; }
        public string NumeroCPFBeneficiario { get; set; }
        public string NumeroCNPJBeneficiario { get; set; }
        public string NumeroBancoBeneficiario { get; set; }
        public string DigitoBancoBeneficiario { get; set; }
        public string NumeroAgenciaBeneficiario { get; set; }
        public string DigitoAgenciaBeneficiario { get; set; }
        public string NumeroContaCorrenteBeneficiario { get; set; }
        public string DigitoContaCorrenteBeneficiario { get; set; }
        public string NomeCidadeBeneficiario { get; set; }
        public string Comentario { get; set; }
        public decimal TotalCompromisso { get; set; }
        public int? StatusCompromisso { get; set; }
        public IList<CompromissoProcessoCredor> CompromissoProcessoCredores { get; set; }
        public IList<CompromissoProcessoParcela> CompromissoProcessoParcelas { get; set; }

        public Processo Processo { get; set; }
        public Fornecedor Fornecedor { get; set; }
        public FormaPagamento FormaPagamento { get; set; }
        public CategoriaPagamento CategoriaPagamento { get; set; }
        //public PlanoCompromisso PlanoCompromisso { get; set; }
        //public StatusCompromisso StatusCompromisso { get; set; }

        public override void PreencherDados(CompromissoProcesso data)
        {

        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class CompromissoProcessoValidator : AbstractValidator<CompromissoProcesso>
        {
            public CompromissoProcessoValidator()
            {
           


            }
        }
    }
}