using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB
{
    public class BBNaturezasAcoes : EntityCrud<BBNaturezasAcoes, long>
    {
        public override AbstractValidator<BBNaturezasAcoes> Validator => new BBNaturezasAcoesValidator();
        public long CodigoBB { get; set; }
        public string Descricao { get; set; }

        public IList<Acao> Acoes { get; set; }
        public override void PreencherDados(BBNaturezasAcoes data)
        {
            CodigoBB = data.CodigoBB;
            Descricao = data.Descricao;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class BBNaturezasAcoesValidator : AbstractValidator<BBNaturezasAcoes>
    {
        public BBNaturezasAcoesValidator()
        {
            RuleFor(x => x.Descricao)
                 .NotEmpty()
                 .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                 .MaximumLength(50).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                 .WithName("Descrição");

            RuleFor(x => x.CodigoBB)
                 .NotEmpty()
                 .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                 .WithName("Natureza Ação BB")
                 .Custom((codBB, context) => {
                     if (codBB.ToString().Length > 4)
                     {
                         context.AddFailure("Tamanho do campo Natureza Ação BB está fora do permitido.\r\n");
                     }
                 }) ;
        }
    }
}