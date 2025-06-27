using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Entities;
using System;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Providers.Implementations
{
    internal class UsuarioAtualProvider : IUsuarioAtualProvider
    {
        private Lazy<IDatabaseContext> LazyContext { get; }
        private IDatabaseContext Context => LazyContext.Value;
        private IHttpContextAccessor HttpAccessor { get; }

        public UsuarioAtualProvider(Lazy<IDatabaseContext> lazyContext, IHttpContextAccessor httpAccessor)
        {
            LazyContext = lazyContext;
            HttpAccessor = httpAccessor;
        }

#if DEBUG
        public string Login => HttpAccessor.HttpContext.User.Identity.Name ?? "PERLINK";
#else
        public string Login => HttpAccessor.HttpContext.User.Identity.Name;
#endif

        public bool TemPermissaoPara(params string[] permissoes)
        {
            if (permissoes is null || permissoes.Length == 0)
            {
                return false;
            }

            var gruposDoUsuario = Usuario.Grupos.Select(x => x.Nome);

            return Context.Permissoes
                .Where(x => permissoes.Contains(x.Menu))
                .Where(perm => gruposDoUsuario.Any(grupo => grupo.Equals(perm.GrupoUsuario)))
                .Any();
        }

        private Usuario? usuario;
        private Usuario Usuario
        {
            get
            {
                if (usuario is null)
                {
                    usuario = Context.Usuarios.FirstOrDefault(x => x.Id == Login);
                }
                return usuario;
            }
        }
        public Usuario ObterUsuario() => Usuario;
    }
}