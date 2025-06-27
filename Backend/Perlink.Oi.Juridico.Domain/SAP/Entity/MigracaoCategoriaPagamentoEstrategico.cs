using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.Entity
{
    public class MigracaoCategoriaPagamentoEstrategico : EntityCrud<MigracaoCategoriaPagamentoEstrategico, long>
    {
        public override AbstractValidator<MigracaoCategoriaPagamentoEstrategico> Validator => throw new NotImplementedException();
        public long? CodCategoriaPagamentoEstra { get; set; }

        public long? CodCategoriaPagamentoCivel { get; set; }


        public override void PreencherDados(MigracaoCategoriaPagamentoEstrategico data)
        {
            CodCategoriaPagamentoEstra = data.CodCategoriaPagamentoEstra;
            CodCategoriaPagamentoCivel = data.CodCategoriaPagamentoCivel;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }


        internal class MigracaoCategoriaPagamentoeEstrategicotoValidator : AbstractValidator<MigracaoCategoriaPagamentoEstrategico>
        {
          
        }
    }
}
