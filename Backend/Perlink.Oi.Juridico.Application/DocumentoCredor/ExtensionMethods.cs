using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories.Implementations;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Services;
using Perlink.Oi.Juridico.Application.DocumentoCredor.Services.Implementations;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Handlers;
using Perlink.Oi.Juridico.Infra.Handlers.Implementations;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Providers.Implementations;
using System;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor
{
    public static class ExtensionMethods
    {
        public static IServiceCollection AddDocumentosCredores(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDatabaseContext(optionsAction);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IPdfHandler, PdfHandler>();
            services.AddScoped<IFileHandler, FileHandler>();
            services.AddScoped<IXlsxHandler, XlsxHandler>();
            services.AddScoped<ICsvHandler, CsvHandler>();
            services.AddScoped<IComprovanteService, ComprovanteService>();
            services.AddScoped<IDocumentoService, DocumentoService>();
            services.AddScoped<IAgendamentoCargaComprovanteRepository, AgendamentoCargaComprovanteRepository>();
            services.AddScoped<IAgendamentoCargaComprovanteService, AgendamentoCargaComprovanteService>();
            services.AddScoped<IParametroJuridicoProvider, ParametroJuridicoProvider>();
            services.AddScoped<IAgendamentoCargaDocumentoRepository, AgendamentoCargaDocumentoRepository>();
            services.AddScoped<IAgendamentoCargaDocumentoService, AgendamentoCargaDocumentoService>();
            services.AddScoped<IParametroJuridicoRepository, ParametroJuridicoRepository>();

            return services;
        }
    }
}