using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class FornecedorasContratos : EntityCrud<FornecedorasContratos, long>
    {
        public override AbstractValidator<FornecedorasContratos> Validator => new FornecedorasContratosValidator();

        public long CodigoEmpreSap { get; set; }
        public long NumeroContrato { get; set; }
        public DateTime? DataValidade { get; set; }
        public decimal ValorContrato { get; set; }
        public decimal ValorSaldoContrato { get; set; }
        public bool IndRetencaodeValor { get; set; }
        public bool IndContratoAtivo { get; set; }
        public Fornecedor fornecedor { get; set; }
        public Empresas_Sap empresaSap { get; set; }
        public override void PreencherDados(FornecedorasContratos data)
        {
            CodigoEmpreSap = data.CodigoEmpreSap;
            NumeroContrato = data.NumeroContrato;
            DataValidade = data.DataValidade;
            ValorContrato = data.ValorContrato;
            ValorSaldoContrato = data.ValorSaldoContrato;
            IndRetencaodeValor = data.IndRetencaodeValor;
            IndContratoAtivo = data.IndContratoAtivo;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }

        internal class FornecedorasContratosValidator : AbstractValidator<FornecedorasContratos>
        {
            public FornecedorasContratosValidator()
            {
            }
        }

    }
}
