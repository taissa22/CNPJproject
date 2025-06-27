using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity
{
    public class Permissao : EntityCrud<Permissao, string> {
        public string Aplicacao { get; set; }
        public string GrupoUsuario { get; set; }
        public string Janela { get; set; }
        public string Menu { get; set; }

        public override AbstractValidator<Permissao> Validator => throw new System.NotImplementedException();

        public static Permissao Criar(Permissao permissaoEntity)
        {
            var permissao = new Permissao()
            {
                Aplicacao = permissaoEntity.Aplicacao,
                GrupoUsuario = permissaoEntity.Id,
                Janela = permissaoEntity.Janela,
                Menu = permissaoEntity.Menu
            };
            return permissao;
        }

        public override void PreencherDados(Permissao data)
        {
            throw new System.NotImplementedException();
        }

        public override ResultadoValidacao Validar()
        {
            throw new System.NotImplementedException();
        }
    }
}