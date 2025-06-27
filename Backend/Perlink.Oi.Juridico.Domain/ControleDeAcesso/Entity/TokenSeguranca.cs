using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity {
    public class TokenSeguranca : EntityCrud<TokenSeguranca, decimal> {
        public string CodigoUsuario { get; set; }
        public string Token { get; set; }
        public string Ip { get; set; }
        public DateTime DataDeCriacao { get; set; }

        public override AbstractValidator<TokenSeguranca> Validator => new TokenSegurancaValidator();

        public override void PreencherDados(TokenSeguranca data) {
            Id = data.Id;
            CodigoUsuario = data.CodigoUsuario;
            Token = data.Token;
            Ip = data.Ip;
            DataDeCriacao = data.DataDeCriacao;
        }

        public override ResultadoValidacao Validar() {
            return ExecutarValidacaoPadrao(this);
        }
    }
    internal class TokenSegurancaValidator : AbstractValidator<TokenSeguranca> {
        public TokenSegurancaValidator() {
            RuleFor(x => x.Id);
            RuleFor(x => x.CodigoUsuario);
            RuleFor(x => x.Token);
            RuleFor(x => x.Ip);
            RuleFor(x => x.DataDeCriacao);
        }
    }
}