using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity
{
    public class Menu : EntityCrud<Menu, string>
    {
        public string CodigoMenu { get; set; }
        public string DescricaoMenu { get; set; }
        public string CaminhoMenu { get; set; }
        public string TipoMenu { get; set; }

        public override AbstractValidator<Menu> Validator => throw new NotImplementedException();

        public override void PreencherDados(Menu data)
        {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            throw new NotImplementedException();
        }
    }
}
