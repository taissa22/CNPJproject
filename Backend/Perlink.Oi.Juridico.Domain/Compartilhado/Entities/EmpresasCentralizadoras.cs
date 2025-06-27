using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class EmpresasCentralizadoras : EntityCrud<EmpresasCentralizadoras, long>
    {
        public override AbstractValidator<EmpresasCentralizadoras> Validator => new EmpresasCentralizadorasValidator();

        public string Nome { get; set; }
        public long NumeroOrdemClassificacaoProcesso { get; set; }
        public IList<EmpresasCentralizadorasConvenio> EmpresasCentralizadorasConvenio { get; set; }
        public IList<Parte> Partes { get; set; }

        public override void PreencherDados(EmpresasCentralizadoras data)
        {
            Nome = data.Nome;
            NumeroOrdemClassificacaoProcesso = data.NumeroOrdemClassificacaoProcesso;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class EmpresasCentralizadorasValidator : AbstractValidator<EmpresasCentralizadoras>
        {
            public EmpresasCentralizadorasValidator()
            {
            }
        }
    }

}