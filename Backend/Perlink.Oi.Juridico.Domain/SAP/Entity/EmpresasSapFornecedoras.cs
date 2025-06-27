namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    using FluentValidation;
    using global::Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
    using Shared.Domain.Impl.Entity;
    using Shared.Domain.Impl.Validator;

    namespace Perlink.Oi.Juridico.Domain.SAP.Entity
    {
        public class EmpresasSapFornecedoras : EntityCrud<EmpresasSapFornecedoras, long>
        {
            public override AbstractValidator<EmpresasSapFornecedoras> Validator => new EmpresasSapFornecedorasValidator();
            public long CodigoEmpresaSap { get; set; }
            public string CodigoBloqueioSAP { get; set; }

            public Fornecedor fornecedor { get; set; }
            
            public Empresas_Sap empresaSap { get; set; }
            public override void PreencherDados(EmpresasSapFornecedoras data)
            {
                CodigoEmpresaSap = data.CodigoEmpresaSap;
                CodigoBloqueioSAP = data.CodigoBloqueioSAP;
            }

            public override ResultadoValidacao Validar()
            {
                return ExecutarValidacaoPadrao(this);
            }
        }

        internal class EmpresasSapFornecedorasValidator : AbstractValidator<EmpresasSapFornecedoras>
        {
            public EmpresasSapFornecedorasValidator()
            {
            }
        }
    }
}