using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Perlink.Oi.Juridico.Application.Security;
using Perlink.Oi.Juridico.CrossCutting.IoC;
using Perlink.Oi.Juridico.Domain.Compartilhado;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico;
using Perlink.Oi.Juridico.WebApi.Export.CSV;
using Microsoft.Net.Http.Headers;
using System.Linq;
using Perlink.Oi.Juridico.Application.DocumentoCredor;
using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.SAP;
using Oi.Juridico.Contextos.AtmJecContext.Data;
using System;
using Oi.Juridico.Contextos.AtmPexContext.Data;
using Oi.Juridico.Contextos.RelatorioMovimentacoesPexContext.Data;
using Oi.Juridico.Contextos.RelatoriosContingenciaTrabalhistaContext.Data;
using Oi.Juridico.Contextos.SegurancaContext.Data;
using Perlink.Oi.Juridico.WebApi.Helpers;

namespace Perlink.Oi.Juridico.WebApi
{
#pragma warning disable CS1591
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = global::Oi.Juridico.AddOn.Configuracoes.ConnectionString.JurUserSemPool;

            Configuracao.InjecaoDependencia(services, connectionString);

            services.AddDbContextPool<AtmJecContext>(ConfiguracaoOracle(connectionString));

            services.AddDbContextPool<AtmPexContext>(ConfiguracaoOracle(connectionString));

            services.AddDbContextPool<AtmPexContext>(options =>
            {
                options.UseOracle(connectionString, o => o.UseOracleSQLCompatibility("11"));
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            services.AddDbContextPool<RelatorioMovimentacoesPexContext>(options =>
            {
                options.UseOracle(connectionString, o => o.UseOracleSQLCompatibility("11"));
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            });

            services.AddDbContextPool<SegurancaContext>(ConfiguracaoOracle(connectionString));

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/csv", "text/plain" });
            });

            services.AddAgendaDeAudienciasDoCivelEstrategico(x => x.UseOracle(connectionString, options => options.UseOracleSQLCompatibility("11")).EnableSensitiveDataLogging(true));
            services.AddManutencao(x => x.UseOracle(connectionString, options => options.UseOracleSQLCompatibility("11")).EnableSensitiveDataLogging(true));
            services.AddDocumentosCredores(x => x.UseOracle(configuration.GetConnectionString("JuridicoConnection"), options => options.UseOracleSQLCompatibility("11")).EnableSensitiveDataLogging(true));
            services.AddAgendamentoMigracaoPedidosSAP(x => x.UseOracle(configuration.GetConnectionString("JuridicoConnection"), options => options.UseOracleSQLCompatibility("11")).EnableSensitiveDataLogging(true));
            
            services.AddDbContextPool<RelatoriosContingenciaTrabalhistaContext>(ConfiguracaoOracle(connectionString));

            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

            services.AddCors();

            services.AddMvc(
            options =>
            {
                options.OutputFormatters.Add(new CsvOutputFormatter(new CsvFormatterOptions(";")));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            });

            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>
            (options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            var jwtSettings = services.BuildServiceProvider().GetRequiredService<JwtSettings>();

            services.Configure<IISOptions>(o =>
            {
                o.ForwardClientCertificate = false;
            });

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.RequireHttpsMetadata = false;
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = jwtSettings.SigningCredentials.Key,
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });        

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api do projeto Perlink.Oi.Juridico.WebApi", Version = "v1" });
                c.CustomSchemaIds(type => type.ToString());
                c.AddSecurityDefinition("token", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = HeaderNames.Authorization,
                    Scheme = "Bearer"
                });
                c.OperationFilter<SecureEndpointAuthRequirementFilter>();
            });
        }

        private static Action<DbContextOptionsBuilder> ConfiguracaoOracle(string connectionString)
        {
            return options =>
            {
                options.UseOracle(connectionString, o => o.UseOracleSQLCompatibility("11"));
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
            };
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (env.IsProduction() == false)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                #if DEBUG
                    var swaggerEndpoint = "/swagger/v1/swagger.json";
                #else
                    var swaggerEndpoint = "/api/swagger/v1/swagger.json";
                #endif

                app.UseSwaggerUI(c =>
                {                    
                    c.EnableTryItOutByDefault();
                    c.DisplayRequestDuration();
                    c.DefaultModelsExpandDepth(-1);
                    c.SwaggerEndpoint(swaggerEndpoint, "WebApi Com Asp.net Core");
                });
            }

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("pt-BR"),
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    new CultureInfo("pt-BR"),
                }
            });

            app.UseCors(builder => builder.AllowAnyMethod()
                                         .AllowAnyOrigin()
                                         .AllowAnyHeader()
                                         .WithExposedHeaders("Content-Disposition"));

            app.UseAuthentication();
            app.UseMvc();


            Runtime.ConnectionString = configuration.GetConnectionString("JuridicoConnection");
        }
    }
#pragma warning disable CS1591
}
