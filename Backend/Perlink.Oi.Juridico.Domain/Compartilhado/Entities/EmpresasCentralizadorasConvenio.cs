using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class EmpresasCentralizadorasConvenio : EntityCrud<EmpresasCentralizadorasConvenio, long>
    {
        public override AbstractValidator<EmpresasCentralizadorasConvenio> Validator => new EmpresasCentralizadorasConvenioValidator();

        public long CodigoEmpresaCentralizadora { get; set; }
        public string CodigoEstado { get; set; }
        public long NumeroAgenciaDepositaria { get; set; }
        public string DigitoAgenciaDepositaria { get; set; }
               
        public EmpresasCentralizadoras EmpresaCentralizadoras { get; set; }
        public Estado Estado { get; set; }

        public override void PreencherDados(EmpresasCentralizadorasConvenio data)
        {
            CodigoEstado = data.CodigoEstado;
            CodigoEmpresaCentralizadora = data.CodigoEmpresaCentralizadora;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class EmpresasCentralizadorasConvenioValidator : AbstractValidator<EmpresasCentralizadorasConvenio>
        {
            public EmpresasCentralizadorasConvenioValidator()
            {
            }
        }
    }
}

