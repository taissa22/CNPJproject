using FluentValidation;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Estado : EntityCrud<Estado, string> {

        public string NomeEstado { get; set; }
        public long? CodigoIndice { get; set; }
        public decimal? ValorJuros { get; set; }
        public long? SequencialMunicipio { get; set; }
        public IEnumerable<GrupoEstados> GrupoEstados { get; set; }

        public override AbstractValidator<Estado> Validator => throw new NotImplementedException();

        public IList<EmpresasCentralizadorasConvenio> EmpresasCentralizadorasConvenio { get; set; }

        public override void PreencherDados(Estado data) {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar() {
            throw new NotImplementedException();
        }
    }
}
