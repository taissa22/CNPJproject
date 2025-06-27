using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity.Processos
{
    public class AudienciaProcesso : EntityCrud<AudienciaProcesso, long>
    {
        public long? SequenciaAudiencia { get; set; }

        public long? CodigoProfissional { get; set; }

        public long? CodigoAdvogado { get; set; }        

        public long? CodigoProfissionalAcompanhante { get; set; }

        public long? CodigoAdvogadoAcompanhante { get; set; }

        public long? CodigoPreposto { get; set; }

        public long? CodigoPrepostoAcompanhante { get; set; }

        public Processo Processo { get; set; }

        public Preposto Preposto { get; set; }

        public long? TipoAudienciaId { get; set; }

        public TipoAudiencia TipoAudiencia { get; set; }

        public override AbstractValidator<AudienciaProcesso> Validator => new AudienciaProcessoValidator();

        public override void PreencherDados(AudienciaProcesso data)
        {
            Id = data.Id;
            SequenciaAudiencia = data.SequenciaAudiencia;
            CodigoProfissional = data.CodigoProfissional;
            CodigoAdvogado = data.CodigoAdvogado;
            CodigoProfissionalAcompanhante = data.CodigoProfissionalAcompanhante;
            CodigoAdvogadoAcompanhante = data.CodigoAdvogadoAcompanhante;
            CodigoPreposto = data.CodigoPreposto;
            CodigoPrepostoAcompanhante = data.CodigoPrepostoAcompanhante;
        }

        public override ResultadoValidacao Validar() => ExecutarValidacaoPadrao(this);

        internal class AudienciaProcessoValidator : AbstractValidator<AudienciaProcesso>
        {
            public AudienciaProcessoValidator()
            {
                RuleFor(x => x.Id).NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                                  .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);
                RuleFor(x => x.SequenciaAudiencia).NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null)
                                                  .NotEmpty().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);             
            }
        }
    }
}
