using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity
{
    public class JuroCorrecaoProcesso : EntityCrud<JuroCorrecaoProcesso, long>
    {
        public DateTime? DataVigencia { get; set; }

        public double? ValorJuros { get; set; }

        public TipoProcesso TipoProcesso { get; set; }

        public override AbstractValidator<JuroCorrecaoProcesso> Validator => new JuroCorrecaoProcessoValidator();

        public override void PreencherDados(JuroCorrecaoProcesso data)
        {
            Id = data.Id;
            DataVigencia = data.DataVigencia;
            ValorJuros = data.ValorJuros;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class JuroCorrecaoProcessoValidator : AbstractValidator<JuroCorrecaoProcesso>
        {
            public JuroCorrecaoProcessoValidator()
            {
                RuleFor(x => x.DataVigencia)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

                RuleFor(x => x.ValorJuros)
                    .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                    .ScalePrecision(2, 6).WithMessage(Textos.DuasCasasDecimaisValidas);
            }
        }
    }
}
