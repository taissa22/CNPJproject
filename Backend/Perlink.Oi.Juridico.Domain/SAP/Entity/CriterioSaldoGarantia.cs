using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class CriterioSaldoGarantia : EntityCrud<CriterioSaldoGarantia, long>
    {
        public long CodigoAgendamento { get; set; }
        public string Criterio { get; set; }
        public string Parametros { get; set; }
        public string ValoresParametros { get; set; }
        public string NomeCriterio { get; set; }
        public string NomeCriterioFormatado { get; set; }

        public AgendamentoSaldoGarantia Agendamento { get; set; }

        public override AbstractValidator<CriterioSaldoGarantia> Validator => new CriterioSaldoGarantiaValidator();

        public override void PreencherDados(CriterioSaldoGarantia data)
        {
            CodigoAgendamento = data.CodigoAgendamento;
            Criterio = data.Criterio;
            Parametros = data.Parametros;
            ValoresParametros = data.ValoresParametros;
            NomeCriterio = data.NomeCriterio;
            NomeCriterioFormatado = data.NomeCriterioFormatado;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class CriterioSaldoGarantiaValidator : AbstractValidator<CriterioSaldoGarantia>
    {
        public CriterioSaldoGarantiaValidator()
        {

        }
    }
}
