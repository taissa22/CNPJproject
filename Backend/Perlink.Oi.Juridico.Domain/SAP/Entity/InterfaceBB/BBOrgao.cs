using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB
{
    public class BBOrgaos : EntityCrud<BBOrgaos, long> {

        public long Codigo { get; set; }
        public string Nome { get; set; }
        public long CodigoBBTribunal { get; set; }
        public long CodigoBBComarca { get; set; }
        public BBTribunais BBTribunais { get; set; }
        public BBComarca BBComarca { get; set; }
        public IList<Vara> Varas { get; set; }

        public override AbstractValidator<BBOrgaos> Validator => new BBOrgaosValidator();

        public override void PreencherDados(BBOrgaos data) {
            Codigo = data.Codigo;
            CodigoBBComarca = data.CodigoBBComarca;
            CodigoBBTribunal = data.CodigoBBTribunal;
            Nome = data.Nome;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }

        internal class BBOrgaosValidator : AbstractValidator<BBOrgaos> {
            public BBOrgaosValidator() {
                RuleFor(x => x.Nome)
                    .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                    .MaximumLength(100).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                    .WithName("Nome do órgão BB");
                RuleFor(x => x.Codigo)
                    .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                    .WithName("Órgão BB");
                RuleFor(x => x.CodigoBBComarca)
                    .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                    .WithName("Comarca BB");
                RuleFor(x => x.CodigoBBTribunal)
                    .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                    .WithName("Tribunal BB");
            }
        }
    }
}
