using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Feriados : EntityCrud<Feriados, long> {

        public long CodigoClassificacaoFeriado { get; set; }
        public DateTime Data { get; set; }

        public override AbstractValidator<Feriados> Validator => throw new NotImplementedException();

        public override void PreencherDados(Feriados data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            throw new NotImplementedException();
        }
    }
}
