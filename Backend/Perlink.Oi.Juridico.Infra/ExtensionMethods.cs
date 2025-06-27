using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Perlink.Oi.Juridico.Infra.Implementations;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Providers.Implementations;
using System;

namespace Perlink.Oi.Juridico.Infra
{
    public static class ExtensionMethods
    {
        public static IServiceCollection AddDatabaseContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            return services.AddDatabaseContext<UsuarioAtualProvider>(optionsAction);
        }

        public static IServiceCollection AddDatabaseContext<TImpl>(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction) where TImpl : class, IUsuarioAtualProvider
        {
            services.AddDbContextPool<DatabaseContext>(optionsAction);
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddScoped(sp => new Lazy<IDatabaseContext>(() => sp.GetRequiredService<IDatabaseContext>()));
            services.AddScoped<IUsuarioAtualProvider, TImpl>();
            services.AddScoped(sp => new Lazy<IUsuarioAtualProvider>(() => sp.GetRequiredService<IUsuarioAtualProvider>()));

            return services;
        }
    }
}