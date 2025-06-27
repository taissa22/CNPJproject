using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Suporte.Entity
{
    public class RotinaScheduleLog : EntityCrud<RotinaScheduleLog, long>
    {
        public DateTime DataDaOcorrencia { get; set; }
        public string Mensagem { get; set; }
        public long IdRegistro { get; set; }
        public long? Status { get; set; }
        public long IdRotina { get; set; }
        public bool VisualizacaoEstaDisponivel { get; set; }

        public override AbstractValidator<RotinaScheduleLog> Validator => new RotinaScheduleLogValidator();

        public override void PreencherDados(RotinaScheduleLog data)
        {
            IdRotina = data.IdRotina;
            IdRegistro = data.IdRegistro;
            DataDaOcorrencia = data.DataDaOcorrencia;
            Mensagem = data.Mensagem;
            Status = data.Status;
            IdRotina = data.IdRotina;
            VisualizacaoEstaDisponivel = data.VisualizacaoEstaDisponivel;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class RotinaScheduleLogValidator : AbstractValidator<RotinaScheduleLog>
    {
        public RotinaScheduleLogValidator()
        {

        }
    }


}
