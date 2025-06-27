using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class TipoLancamento : EntityCrud<TipoLancamento, long> {
        public override AbstractValidator<TipoLancamento> Validator => new TipoLancamentoValidator();

        public string Descricao { get; set; }
        public IList<LancamentoProcesso> LancamentosProcesso { get; set; }

        public override void PreencherDados(TipoLancamento data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            throw new NotImplementedException();
        }
    }
    internal class TipoLancamentoValidator : AbstractValidator<TipoLancamento>
    {
        public TipoLancamentoValidator() { }
    }
}
