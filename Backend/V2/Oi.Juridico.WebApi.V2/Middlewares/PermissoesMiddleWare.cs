using Microsoft.AspNetCore.Http;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;

namespace Oi.Juridico.WebApi.V2.Middlewares
{
    public class PermissoesMiddleWare
    {
        private RequestDelegate _requestDelegate;

        public PermissoesMiddleWare(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context, ControleDeAcessoContext db)
        {
            var userId = context.User.Identity!.Name!;

            if (userId != null)
            {
                context.Items["permissoes"] = (await db.ObtemPermissoesAsync(userId)).Select(x => x.CodMenu).ToList();
            }

            await _requestDelegate(context);
        }
    }    
}
