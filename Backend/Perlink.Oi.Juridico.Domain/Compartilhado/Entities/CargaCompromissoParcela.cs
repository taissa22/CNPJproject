using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class CargaCompromissoParcela : EntityCrud<CargaCompromissoParcela, string>
    {

        public decimal Id { get; set; }

        public decimal IdCompromisso { get; set; }

        public decimal? SeqLancamento { get; set; }

        public decimal? NroParcela { get; set; }

        public decimal? Valor { get; set; }

        public DateTime? Vencimento { get; set; }

        public decimal? Status { get; set; }

        public string MotivoExclusao { get; set; }

        public string Deletado { get; set; }

        public DateTime? DeletadoDatahora { get; set; }

        public string DeletadoLogin { get; set; }

        public byte? CodLancamento { get; set; }

        public int CodProcesso { get; set; }

        public string MotivoCancelamento { get; set; }

        public string ComentarioCancelamento { get; set; }
        public string ComentarioEstorno { get; set; }

        public string UsrSolicCancelamento { get; set; }
        public string DataSolicCancelamento { get; set; }

        public override AbstractValidator<CargaCompromissoParcela> Validator => new CargaCompromissoParcelaValidator();

        public override void PreencherDados(CargaCompromissoParcela data)
        {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class CargaCompromissoParcelaValidator : AbstractValidator<CargaCompromissoParcela>
        {
            public CargaCompromissoParcelaValidator()
            {



            }
        }
    }
}
