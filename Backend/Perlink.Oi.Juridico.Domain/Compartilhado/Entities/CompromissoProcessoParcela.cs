using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CompromissoProcessoParcela : EntityCrud<CompromissoProcessoParcela, long>
    {
        public override AbstractValidator<CompromissoProcessoParcela> Validator => new CompromissoProcessoParcelaValidator();

        public long CodigoProcesso { get; set; }
        //public long CodigoCompromisso { get; set; }
        public long CodigoParcela { get; set; }
        public long? NumeroParcela { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorParcela { get; set; }
        public long CodigoStatusCompromissoParcela { get; set; }
        public long? CodMotivoSuspCancelParcela { get; set; }
        public long? CodigoLancamento { get; set; }
        public string Comentario { get; set; }
        public DateTime? DataSuspensao { get; set; }
        public string UsuarioSuspensao { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string UsuarioCancelamento { get; set; }

        public CompromissoProcesso CompromissoProcesso { get; set; }
        public LancamentoProcesso LancamentoProcesso { get; set; }
        public MotivoSuspensaoCancelamentoParcela MotivoSuspensaoCancelamentoParcela { get; set; }
        public StatusCompromissoParcela StatusCompromissoParcela { get; set; }
        //public Parcela Parcela { get; set; }//ToDo - Falta mapear

        public override void PreencherDados(CompromissoProcessoParcela data)
        {
            
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class CompromissoProcessoParcelaValidator : AbstractValidator<CompromissoProcessoParcela>
        {
            public CompromissoProcessoParcelaValidator()
            {
            }
        }
    }
}