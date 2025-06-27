using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity
{
    public class Janela : EntityCrud<Janela, string>
    {
        public string CodAplicacao { get; set; }
        public string CodJanela { get; set; }
        public string CodMenu { get; set; }

        public override AbstractValidator<Janela> Validator => throw new NotImplementedException();

        public override void PreencherDados(Janela data)
        {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            throw new NotImplementedException();
        }
    }
}
