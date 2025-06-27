using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Banco : EntityCrud<Banco, long>
    {
        public override AbstractValidator<Banco> Validator => throw new NotImplementedException();

       
        public string NomeBanco { get; set; }
        public string NumeroBanco { get; set; }
        public string DigitoVerificadorBanco { get; set; }
        public string NumeroAgencia { get; set; }
        public string DigitoVerificadorAgencia { get; set; }

        public IList<Fornecedor> Fornecedores { get; set; }
        public IList<LancamentoProcesso> LancamentoProcessos { get; set; }


        public override void PreencherDados(Banco data)
        {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            throw new NotImplementedException();
        }
    }
}