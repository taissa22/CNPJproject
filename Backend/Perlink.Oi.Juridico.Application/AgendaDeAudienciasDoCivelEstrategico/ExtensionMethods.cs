using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories.Implementations;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Services;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Services.Implementatios;
using Perlink.Oi.Juridico.Infra;
using System;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico {

    public static class ExtensionMethods {

        public static IServiceCollection AddAgendaDeAudienciasDoCivelEstrategico(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) {
            services.AddDatabaseContext(optionsAction);

            services.AddScoped<IAdvogadoDoEscritorioRepository, AdvogadoDoEscritorioRepository>();
            services.AddScoped<IComarcaRepository, ComarcaRepository>();
            services.AddScoped<IEmpresaDoGrupoRepository, EmpresaDoGrupoRepository>();
            services.AddScoped<IEscritorioRepository, EscritorioRepository>();
            services.AddScoped<IPedidoDoProcessoRepository, PedidoDoProcessoRepository>();
            services.AddScoped<IPrepostoRepository, PrepostoRepository>();
            services.AddScoped<IParteDoProcessoRepository, ParteDoProcessoRepository>();
            services.AddScoped<IFeriadoRepository, FeriadoRepository>();
            services.AddScoped<IAudienciaRepository, AudienciaRepository>();
            services.AddScoped<IAudienciaService, AudienciaService>();
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();

            return services;
        }
    }
}