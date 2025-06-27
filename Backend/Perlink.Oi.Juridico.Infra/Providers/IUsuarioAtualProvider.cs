using Perlink.Oi.Juridico.Infra.Entities;

namespace Perlink.Oi.Juridico.Infra.Providers
{
    public interface IUsuarioAtualProvider
    {
        string Login { get; }

        bool TemPermissaoPara(params string[] permissoes);

        Usuario ObterUsuario();
    }
}