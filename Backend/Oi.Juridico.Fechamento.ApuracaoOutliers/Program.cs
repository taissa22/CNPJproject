using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.Interface;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.CrossCutting.IoC;
using Shared.Domain.Interface;
using System;
using System.IO;

namespace Oi.Juridico.Fechamento.ApuracaoOutliers {
    class Program {
        private static ServiceProvider _serviceProvider;
        static void Main(string[] args) {

            RegisterServices();

            ILogger<Program> logger = _serviceProvider.GetService<ILoggerFactory>()
                                        .CreateLogger<Program>();

            var appService = _serviceProvider.GetService<IApuracaoOutliersAppService>();
            //Expurga arquivos e agendamentos antigos
            logger.LogInformation("\n################### Início do executor ApuracaoOutliers ###################\n");
            //appService.Expurgar(logger);            

            appService.ExecutarAgendamentos(logger);
            logger.LogInformation("################### Fim do executor ApuracaoOutliers ###################");


            DisposeServices();
        }

        private static void RegisterServices() {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            var services = new ServiceCollection();

            Configuracao.InjecaoDependencia(services, configuration.GetConnectionString("JuridicoConnection"));
            services.AddScoped<IAuthenticatedUser,AuthenticatedUser>();


            services.AddLogging(builder => {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog(new NLogProviderOptions {
                    CaptureMessageTemplates = true,
                    CaptureMessageProperties = true
                });
            });

            _serviceProvider = services.BuildServiceProvider();
        }
        private static void DisposeServices() {
            if (_serviceProvider == null) {
                return;
            }
            if (_serviceProvider is IDisposable) {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
