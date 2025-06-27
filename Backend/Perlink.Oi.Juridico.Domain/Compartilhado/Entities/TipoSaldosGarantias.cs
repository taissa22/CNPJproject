using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class TiposSaldosGarantias : EntityCrud<TiposSaldosGarantias, long>
    {
        public override AbstractValidator<TiposSaldosGarantias> Validator => new TiposSaldosGarantiasValidator();

        public bool IndicaBaixaPagamento { get; set; }
        public string Descricao { get; set; }
        
        public override void PreencherDados(TiposSaldosGarantias data)
        {
            Descricao = data.Descricao;
            IndicaBaixaPagamento = data.IndicaBaixaPagamento;
       
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class TiposSaldosGarantiasValidator : AbstractValidator<TiposSaldosGarantias>
    {
        public TiposSaldosGarantiasValidator()
        {

        }
    }
}
