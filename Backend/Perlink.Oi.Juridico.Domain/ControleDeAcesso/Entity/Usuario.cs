using FluentValidation;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity
{
    public class Usuario : EntityCrud<Usuario, string> {
        public override AbstractValidator<Usuario> Validator => new UsuarioValidator();
        public string Nome { get; set; }
        public string CPF { get; set; }
        public int? CodigoOrigemUsuario { get; set; }
        public bool EhEscritorio { get { return CodigoOrigemUsuario == 5; } }
        public string Email { get; set; }
        public bool EhPerfilWeb { get; set; }       
        public virtual string TipoPerfil { get; set; }
        public virtual string GestorDefaultId { get; set; }
        public virtual Usuario GestorDefault { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual bool EhGestorAprovador { get; set; }
        public virtual bool Restrito { get; set; }
        public virtual bool EhPerfil { get; set; }
        public IList<UsuarioGrupo> UsuariosDelegacoes { get; set; }
        public IList<Lote> Lotes { get; set; }
        public IList<Processo> ProcessosUsuario { get; set; }  
        public virtual DateTime? DataUltimoAcesso { get; set; }

        public override void PreencherDados(Usuario data) {
            Nome = data.Nome;
            CPF = data.CPF;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class UsuarioValidator : AbstractValidator<Usuario> {
        public UsuarioValidator() {
            RuleFor(x => x.Id)
                .MaximumLength(30).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
                .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.Nome)
               .MaximumLength(50).WithMessage(Textos.Geral_Mensagem_Erro_Tamanho_Campo)
               .NotNull().WithMessage(Textos.Geral_Mensagem_Erro_Campo_null);

            RuleFor(x => x.Nome);
        }
    }
}