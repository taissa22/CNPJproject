using FluentValidation;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class MigracaoCategoriaPagamento : EntityCrud<MigracaoCategoriaPagamento, long>
    {
        public override AbstractValidator<MigracaoCategoriaPagamento> Validator => throw new NotImplementedException();

        public long?  CodCategoriaPagamentoCivel { get; set; }

        public long? CodCategoriaPagamentoEstra { get; set; }

        public override void PreencherDados(MigracaoCategoriaPagamento data)
        {
            CodCategoriaPagamentoCivel = data.CodCategoriaPagamentoCivel;
            CodCategoriaPagamentoEstra = data.CodCategoriaPagamentoEstra;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }


        internal class CategoriaPagamentoValidator : AbstractValidator<MigracaoCategoriaPagamento>
        {
          
        }
    }
}
