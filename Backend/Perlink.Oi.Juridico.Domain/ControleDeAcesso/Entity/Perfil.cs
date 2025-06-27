using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity
{
    public class Perfil : EntityCrud<Perfil, string>
    {  
        public string Nome { get; set; }
        public string CPF { get; set; }
        public int CodigoOrigemUsuario { get; set; }
        public bool EhEscritorio { get { return CodigoOrigemUsuario == 5; } }
        public string Email { get; set; }
        public bool EhPerfilWeb { get; set; }
        public virtual string TipoPerfil { get; set; }
        public virtual string GestorDefaultId { get; set; }
        public virtual Usuario GestorDefault { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual bool GestorAprovador { get; set; }
        public virtual bool Restrito { get; set; }
        //public virtual bool Perfil { get; set; }
        public IList<UsuarioGrupo> UsuariosDelegacoes { get; set; }

        public override AbstractValidator<Perfil> Validator => throw new NotImplementedException();

        public override void PreencherDados(Perfil data)
        {
            throw new NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            throw new NotImplementedException();
        }
    }
}
