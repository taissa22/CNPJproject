using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Shared.Domain.Impl.Service;
using Shared.Domain.Interface;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service {
    public class UsuarioService : BaseCrudService<Usuario, string>, IUsuarioService {
        private readonly IUsuarioRepository repository;
        private readonly IAuthenticatedUser user;
        public UsuarioService(IUsuarioRepository repository, IAuthenticatedUser user) : base(repository) {
            this.repository = repository;
            this.user = user;
        }

        public bool ExisteUsuario(string login) {
            return repository.ExisteUsuario(login);
        }

        public async Task<Usuario> RecuperarPorLogin(string login) {
            return await repository.RecuperarPorLogin(login);
        }

        public string ObterLoginDoUsuarioLogado() {
            return this.user.Login;
        }

        public async Task<Usuario> ObterUsuarioLogado() {
            return await this.RecuperarPorLogin(this.ObterLoginDoUsuarioLogado());
        }


        //public void CriaUsuarioAssociadoPerfil(UsuarioGrupo usuarioGrupo)
        //{
        //    repository.Inserir(permissao);
        //}
        //public void RemoverUsuarioAssociadoPerfil(UsuarioGrupo usuarioGrupo)
        //{
        //    repository.Inserir(permissao);
        //}



    }
}