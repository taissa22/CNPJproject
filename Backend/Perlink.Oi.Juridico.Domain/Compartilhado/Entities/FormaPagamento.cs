using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class FormaPagamento : EntityCrud<FormaPagamento, long>
    {
        public string DescricaoFormaPagamento { get; set; }
        public bool IndicaBordero { get; set; }
        public bool IndicaDadosBancarios { get; set; }
        public bool IndicaRestrita { get; set; }

        public override AbstractValidator<FormaPagamento> Validator => new FormaPagamentoValidator();

        public IList<Lote> Lotes { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }
        public IList<CompromissoProcesso> CompromissoProcessos { get; set; }

        public override void PreencherDados(FormaPagamento data)
        {
            DescricaoFormaPagamento = data.DescricaoFormaPagamento;
            IndicaBordero = data.IndicaBordero;
            IndicaDadosBancarios = data.IndicaDadosBancarios;
            IndicaRestrita = data.IndicaRestrita;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class FormaPagamentoValidator : AbstractValidator<FormaPagamento>
        {
            public FormaPagamentoValidator()
            {
            }
        }
    }
}