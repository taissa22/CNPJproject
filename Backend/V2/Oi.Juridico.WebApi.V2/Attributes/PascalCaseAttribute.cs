using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Oi.Juridico.WebApi.V2.Attributes
{
    public class PascalCaseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext ctx)
        {
            if (!(ctx.Result is ObjectResult objectResult)) return;

            var serializer = new JsonSerializerOptions { PropertyNamingPolicy = null };
            serializer.Converters.Add(new JsonStringEnumConverter());

            var formatter = new SystemTextJsonOutputFormatter(serializer);

            objectResult.Formatters.Add(formatter);
        }
    }
}
