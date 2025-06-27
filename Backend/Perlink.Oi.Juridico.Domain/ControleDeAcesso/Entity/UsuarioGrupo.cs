using Shared.Domain.Impl.Entity;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity
{
    public class UsuarioGrupo : Entity<UsuarioGrupo, string> {
        public string Aplicacao { get; set; }
        public string UsuarioId { get;  set; }
        public Usuario Usuario { get;  set; }
        public string GrupoAplicacao { get; set; }
        public string GrupoUsuario { get; set; }

        //public override AbstractValidator<UsuarioGrupo> Validator => throw new System.NotImplementedException();

        //public override void PreencherDados(UsuarioGrupo data)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public override ResultadoValidacao Validar()
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}