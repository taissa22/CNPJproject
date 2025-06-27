using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Domain.Interface.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service
{
    public interface IUsuarioService : IBaseCrudService<Usuario, string>
    {
        bool ExisteUsuario(string login);
        Task<Usuario> RecuperarPorLogin(string login);
        string ObterLoginDoUsuarioLogado();
        Task<Usuario> ObterUsuarioLogado();
    }
}
