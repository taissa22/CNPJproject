using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Oi.Juridico.WebApi.V2.Services;

namespace Oi.Juridico.WebApi.V2.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TemPermissaoAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permissao;

        public TemPermissaoAttribute(string permissao)
        {
            _permissao = permissao;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;

            if (!usuario.Identity!.IsAuthenticated)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized);
                return;
            }

            ControleDeAcessoService? controleDeAcessoService = context.HttpContext.RequestServices.GetService(typeof(ControleDeAcessoService)) as ControleDeAcessoService;

            var temAutorizacao = controleDeAcessoService!.TemPermissao(_permissao);
            if (!temAutorizacao)
            {
                context.Result = new StatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
