using FluentValidation;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB {
    public class BBComarca : EntityCrud<BBComarca, long>
    {
        public string CodigoEstado { get; set; }
        public string Descricao { get; set; }
        public long CodigoBB { get; set; }

        public IList<Comarca> Comarcas { get; set; }
        public IList<BBOrgaos> BBOrgaos { get; set; }
        public override AbstractValidator<BBComarca> Validator => new BBComarcaValidator();

        public override void PreencherDados(BBComarca data)
        {
            CodigoEstado = data.CodigoEstado;
            Descricao = data.Descricao;
            CodigoBB = data.CodigoBB;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class BBComarcaValidator : AbstractValidator<BBComarca>
    {
        public BBComarcaValidator()
        {
            RuleFor(x => x.Descricao)
                 .NotEmpty()
                 .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                 .MaximumLength(100).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                 .WithName("Descrição");

            RuleFor(x => x.CodigoEstado)
                 .NotEmpty()
                 .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                 .MaximumLength(2).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                 .WithName("UF");

            RuleFor(x => x.CodigoBB)
                 .NotEmpty()
                 .WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                 .WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                 .WithName("Comarca BB");
        }
    }
}
