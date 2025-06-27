using FluentValidation;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB
{
    public class BBTribunais : EntityCrud<BBTribunais, long>
    {
        public override AbstractValidator<BBTribunais> Validator => new BBTribunaisValidator();
        public long CodigoBB { get; set; }
        public string Descricao { get; set; }
        public string IndicadorInstancia { get; set; }
        public IList<BBOrgaos> BBOrgaos { get; set; }

        public override void PreencherDados(BBTribunais data)
        {
            CodigoBB = data.CodigoBB;
            Descricao = data.Descricao;
            IndicadorInstancia = data.IndicadorInstancia;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class BBTribunaisValidator : AbstractValidator<BBTribunais>
    {
        public BBTribunaisValidator()
        {
            RuleFor(x => x.CodigoBB)
             .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
            RuleFor(x => x.Descricao)
             .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
             .MaximumLength(50).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo);
            RuleFor(x => x.IndicadorInstancia)
             .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
        }
    }
}