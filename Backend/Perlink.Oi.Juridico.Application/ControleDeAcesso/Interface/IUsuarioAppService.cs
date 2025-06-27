using Perlink.Oi.Juridico.Application.ControleDeAcesso.ViewModel;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Shared.Application.Interface;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface {
    public interface IUsuarioAppService : IBaseCrudAppService<UsuarioViewModel, Usuario, string> {
        bool ExisteUsuario(string login);
        Task<IResultadoApplication<object>> ObterTokenDoUsuarioComChave(LoginViewModel login);
        string ObterLoginDoUsuarioLogado();
    }
}