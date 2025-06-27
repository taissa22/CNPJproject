using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.Shared.V2.Seguranca
{
    // marca os endpoints que necessitam de autenticação no Swagger
    public class SecureEndpointAuthRequirementFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // endpoint que não necessitam de autenticação
            if (context.ApiDescription.ActionDescriptor.DisplayName!.StartsWith("Oi.Juridico.WebApi.V2.Areas.ControleDeAcesso.Controllers.AccountsController.LoginAsync"))
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
