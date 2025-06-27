using Microsoft.AspNetCore.Http;

namespace Oi.Juridico.WebApi.V2.Services
{
    public class ControleDeAcessoService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public ControleDeAcessoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool TemPermissao(string permissao)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            List<string> permissoesUsuario = (List<string>)httpContext!.Items["permissoes"]!;

            if (permissoesUsuario != null)
            {
                return permissoesUsuario.Contains(permissao);
            }

            return false;
        }
    }
}
