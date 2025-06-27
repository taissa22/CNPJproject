using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;


namespace Perlink.Oi.Juridico.WebApi.Helpers
{
    // marca os endpoints que necessitam de autenticação no Swagger
    public class SecureEndpointAuthRequirementFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // endpoint que não necessitam de autenticação
            if (context.ApiDescription.ActionDescriptor.DisplayName != null && context.ApiDescription.ActionDescriptor.DisplayName.StartsWith("Perlink.Oi.Juridico.CivelEstrategico.WebApi.Areas.CivelEstrategico.Controllers.AccountsController.Login"))
            {
                return;
            }

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "token" }
                    }] = new List<string>()
                }
            };
        }
    }
}
