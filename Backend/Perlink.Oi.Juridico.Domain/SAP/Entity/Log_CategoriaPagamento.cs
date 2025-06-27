using FluentValidation;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class Log_CategoriaPagamento : EntityCrud<Log_CategoriaPagamento, long>
    {
        public override AbstractValidator<Log_CategoriaPagamento> Validator => new Log_CategoriaPagamentoValidator();
        public long CodigoTipoLancamento { get; set; }
        public string CodigoUsuarioOperacao { get; set; }
        public DateTime DataOperacao { get; set; }
        public string TipoOperacao { get; set; }
        public bool IndicadorCivel { get; set; }
        public bool IndicadorCivelEstrategico { get; set; }
        public bool IndicadorCivelAdministrativo { get; set; }
        public bool IndicadorJuizado { get; set; }
        public bool IndicadorTrabalhista { get; set; }
        public bool IndicadorTributarioAdministrativo { get; set; }
        public bool IndicadorTributarioJudicial { get; set; }
        public bool IndicadorProcon { get; set; }
        public bool IndicadorPex { get; set; }
        public bool IndicadorAdministrativo { get; set; }
        public bool IndicadorCriminalAdministrativo { get; set; }
        public bool IndicadorCriminalJudicial { get; set; }

        //public CategoriaPagamento CategoriaPagamento { get; set; }

        public void PreencherDados(CategoriaPagamento categoria)
        {
            DataOperacao = DateTime.Now;
            CodigoTipoLancamento = categoria.CodigoTipoLancamento;
            IndicadorCivel = categoria.IndicadorCivel;
            IndicadorCivelEstrategico = categoria.IndicadorCivelEstrategico;
            IndicadorJuizado = categoria.IndicadorJuizado;
            IndicadorTrabalhista = categoria.IndicadorTrabalhista;
            IndicadorTributarioAdministrativo = categoria.IndicadorTributarioAdministrativo;
            IndicadorTributarioJudicial = categoria.IndicadorTributarioJudicial;
            IndicadorProcon = categoria.IndicadorProcon;
            IndicadorPex = categoria.IndicadorPex;
            IndicadorAdministrativo = categoria.IndicadorAdministrativo;
            IndicadorCriminalAdministrativo = categoria.IndicadorCriminalAdministrativo;
            IndicadorCriminalJudicial = categoria.IndicadorCriminalJudicial;
        }

        public override void PreencherDados(Log_CategoriaPagamento data)
        {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class Log_CategoriaPagamentoValidator : AbstractValidator<Log_CategoriaPagamento>
    {
        public Log_CategoriaPagamentoValidator()
        {
        }
    }
}