using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class TipoVara : EntityCrud<TipoVara, long>
    {
        public override AbstractValidator<TipoVara> Validator => new TipoVaraValidator();
        public string NomeTipoVara { get; set; }
        public bool IndicadorCivel { get; set; }
        public bool IndicadorTrabalhista { get; set; }
        public bool IndicadorTributaria { get; set; }
        public bool IndicadorJuizado { get; set; }
        public bool IndicadorCivelEstrategico { get; set; }
        public bool IndicadorCriminalJudicial { get; set; }
        public bool IndicadorProcon { get; set; }
        public IList<Processo> ProcessosTipoVara { get; set; }

        public override void PreencherDados(TipoVara data)
        {
            NomeTipoVara = data.NomeTipoVara;
            IndicadorCivel = data.IndicadorCivel;
            IndicadorTrabalhista = data.IndicadorTrabalhista;
            IndicadorTributaria = data.IndicadorTributaria;
            IndicadorJuizado = data.IndicadorJuizado;
            IndicadorCivelEstrategico = data.IndicadorCivelEstrategico;
            IndicadorCriminalJudicial = data.IndicadorCriminalJudicial;
            IndicadorProcon = data.IndicadorProcon;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class TipoVaraValidator : AbstractValidator<TipoVara>
    {
        public TipoVaraValidator()
        {
            //RuleFor(x => x.NomeTipoVara)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorCivel).NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorTrabalhista)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorTributaria)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorJuizado)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorCivelEstrategico)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorCriminalJudicial)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            //RuleFor(x => x.IndicadorProcon)
            //    .NotNull()
            //    .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
        }
    }
}