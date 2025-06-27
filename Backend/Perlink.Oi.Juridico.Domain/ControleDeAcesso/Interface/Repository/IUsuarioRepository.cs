using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Repository;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository
{
    public interface IUsuarioRepository : IBaseCrudRepository<Usuario, string>
    {
        bool ExisteUsuario(string login);
        bool ExistePerfil(string NomePerfil);
        Task<Usuario> RecuperarPorLogin(string login);
        void AtualizarUsuarioPerfil(Usuario usuario);
        void SalvarUsuarioPerfil(Usuario usuario);

    }
}
