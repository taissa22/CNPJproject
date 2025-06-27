using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class LoteLancamento : EntityCrud<LoteLancamento, long> {
        public long CodigoProcesso { get; set; }
        public long CodigoLancamento { get; set; }

        public Lote Lote { get; set; }

        public override FluentValidation.AbstractValidator<LoteLancamento> Validator => new LoteLancamentoValidator();

        public LancamentoProcesso LancamentoProcesso { get; set; }

        public override void PreencherDados(LoteLancamento data) {
            CodigoProcesso = data.CodigoProcesso;
            CodigoLancamento = data.CodigoLancamento;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class LoteLancamentoValidator : AbstractValidator<LoteLancamento> {
        public LoteLancamentoValidator() { }
    }
}
