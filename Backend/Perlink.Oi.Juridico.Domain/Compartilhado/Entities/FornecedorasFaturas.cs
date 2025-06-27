using FluentValidation;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class FornecedorasFaturas : EntityCrud<FornecedorasFaturas, long>
    {
        public override AbstractValidator<FornecedorasFaturas> Validator => new FornecedorasFaturasValidator();

        public long CodigoEmpresaSap { get; set; }
        public string CodigoBloqueioSap { get; set; }
        public decimal ValorFatura { get; set; }
        public Fornecedor fornecedor { get; set; }
        public Empresas_Sap empresaSap { get; set; }
        public override void PreencherDados(FornecedorasFaturas data)
        {
            CodigoEmpresaSap = data.CodigoEmpresaSap;
            CodigoBloqueioSap = data.CodigoBloqueioSap;
            ValorFatura = data.ValorFatura;
        }

        public override ResultadoValidacao Validar()
        {
            throw new NotImplementedException();
        }
    }

    internal class FornecedorasFaturasValidator : AbstractValidator<FornecedorasFaturas>
    {
        public FornecedorasFaturasValidator()
        {

        }
    }
}
