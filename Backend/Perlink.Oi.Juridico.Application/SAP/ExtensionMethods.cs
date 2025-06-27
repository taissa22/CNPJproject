using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Perlink.Oi.Juridico.Application.SAP.Repositories;
using Perlink.Oi.Juridico.Application.SAP.Repositories.Implementations;
using Perlink.Oi.Juridico.Application.SAP.Services;
using Perlink.Oi.Juridico.Application.SAP.Services.Implementations;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Handlers;
using Perlink.Oi.Juridico.Infra.Handlers.Implementations;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Providers.Implementations;
using System;

namespace Perlink.Oi.Juridico.Application.SAP
{
    public static class ExtensionMethods
    {
        public static IServiceCollection AddAgendamentoMigracaoPedidosSAP(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDatabaseContext(optionsAction);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ICsvHandler, CsvHandler>();
            services.AddScoped<IFileHandler, FileHandler>();
            services.AddScoped<IParametroJuridicoProvider, ParametroJuridicoProvider>();
            services.AddScoped<IAgendamentosMigracaoPedidosSapService, AgendamentosMigracaoPedidosSapService>();
            services.AddScoped<IAgendamentosMigracaoPedidosSapRepository, AgendamentosMigracaoPedidosSapRepository>();

            return services;
        }
    }
}