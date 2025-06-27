using FluentValidation;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class Bordero : EntityCrud<Bordero, long>
    {
        public Lote Lote { get; set; }

        public string NomeBeneficiario { get; set; }
        public string CpfBeneficiario { get; set; }
        public string CnpjBeneficiario { get; set; }
        public string CidadeBeneficiario { get; set; }
        public string NumeroBancoBeneficiario { get; set; }
        public string DigitoBancoBeneficiario { get; set; }
        public string NumeroAgenciaBeneficiario { get; set; }
        public string DigitoAgenciaBeneficiario { get; set; }
        public string NumeroContaCorrenteBeneficiario { get; set; }
        public string DigitoContaCorrenteBeneficiario { get; set; }
        public decimal Valor { get; set; }
        public string Comentario { get; set; }
        public long CodigoLote { get; set; }

       

        public override FluentValidation.AbstractValidator<Bordero> Validator => new BorderoValidator();

        public override void PreencherDados(Bordero data)
        {
            NomeBeneficiario = data.NomeBeneficiario;
            CpfBeneficiario = data.CpfBeneficiario;
            CnpjBeneficiario = data.CnpjBeneficiario;
            CidadeBeneficiario = data.CidadeBeneficiario;
            NumeroBancoBeneficiario = data.NumeroBancoBeneficiario;
            DigitoBancoBeneficiario = data.DigitoBancoBeneficiario;
            NumeroAgenciaBeneficiario = data.NumeroAgenciaBeneficiario;
            DigitoAgenciaBeneficiario = data.DigitoAgenciaBeneficiario;
            NumeroContaCorrenteBeneficiario = data.NumeroContaCorrenteBeneficiario;
            DigitoContaCorrenteBeneficiario = data.DigitoContaCorrenteBeneficiario;
            Valor = data.Valor;
            Comentario = data.Comentario;
            CodigoLote = data.CodigoLote;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class BorderoValidator : AbstractValidator<Bordero>
    {
        public BorderoValidator()
        {
            RuleFor(x => x.CodigoLote)
              .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
             .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
            RuleFor(x => x.Valor)
              .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
              .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
            RuleFor(x => x.NomeBeneficiario)
            .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
            .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

        }
    }
}