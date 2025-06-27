using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB {
    public class BBModalidade : EntityCrud<BBModalidade, long>
    {
        public string Descricao { get; set; }
        public long CodigoBB { get; set; }
       
        public override AbstractValidator<BBModalidade> Validator => new BBModalidadeValidator();

        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }

        public override void PreencherDados(BBModalidade data)
        {
            Descricao = data.Descricao;
            CodigoBB = data.CodigoBB;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class BBModalidadeValidator : AbstractValidator<BBModalidade>
    {
        public BBModalidadeValidator()
        {
            RuleFor(x => x.Descricao)
                  .NotEmpty()
                  .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                  .MaximumLength(50).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                  .WithName("Descrição");

            RuleFor(x => x.CodigoBB)
                 .NotEmpty()
                 .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                 .WithName("Modalidade BB")
                 .Custom((codBB, context) => {
                     if (codBB.ToString().Length > 4)
                     {
                         context.AddFailure("Tamanho do campo Natureza Ação BB está fora do permitido.\r\n");
                     }
                 });
        }
    }
}
