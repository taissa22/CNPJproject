using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Oi.Juridico.Contextos.V2.AtmJecContext.Data;
using Oi.Juridico.Contextos.V2.AtmPexContext.Data;
using Oi.Juridico.Contextos.V2.ControleDeAcessoContext.Data;
using Oi.Juridico.Contextos.V2.ParametrizacaoClosingContext.Data;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Oi.Juridico.Shared.V2.Seguranca;
using Oi.Juridico.Shared.V2.Services;
using Serilog.Events;
using Serilog;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Oi.Juridico.Contextos.V2.SolicitacaoAcessoContext.Data;
using Oracle.EntityFrameworkCore.Infrastructure;
using Oi.Juridico.Shared.V2.Filters;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacaoTrabalhistaContext.Data;
using Oi.Juridico.Contextos.V2.PermissaoContext.Data;
using Oi.Juridico.Contextos.V2.FechamentoTrabalhistaContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesJecContext.Data;
using Oi.Juridico.WebApi.V2.Services;
using Oi.Juridico.Contextos.V2.RelatorioMovimentacoesCivelConsumidorContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoPesquisaContext.Data;
using Oi.Juridico.Contextos.V2.PautaJuizadoComposicaoContext.Data;
using Oi.Juridico.Contextos.V2.RelatorioSolicitacaoLancamentoPexContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoLogProcessoContext.Data;
using Oi.Juridico.WebApi.V2.Middlewares;
using Oi.Juridico.Contextos.V2.CriminalContext.Data;
using Oi.Juridico.Contextos.V2.PerfilContext.Data;
using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Data;
using Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoSolicitanteContext.Data;
using HashidsNet;
using Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoContext.Data;
using Oi.Juridico.Contextos.V2.ESocialContext.Data;
using Oi.Juridico.Contextos.V2.AgendaAudienciaContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoCalculoVEPContext.Data;
using Oi.Juridico.Contextos.V2.SequenceContext.Data;
using Oi.Juridico.Contextos.V2.ESocialLogContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Services;
using Oi.Juridico.Contextos.V2.RelatorioPagamentoEscritorioContext.Data;
using Oi.Juridico.Contextos.V2.ManutencaoContratoEscritorioContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.v1.Repositories;
using Oi.Juridico.Contextos.V2.EmpresaContratadaContext.Data;
using Oi.Juridico.Contextos.V2.ResultadoNegociacaoContext.Data;
using Oi.Juridico.Contextos.V2.StatusContatoContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Relatorios;
using Oi.Juridico.Contextos.V2.AtmCCContext.Data;
using Oi.Juridico.Contextos.V2.LogProcessoTrabalhistaContext.Data;
using Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.Services;
using Oi.Juridico.Contextos.V2.AgendCargaCompromissoContext.Data;
using Oi.Juridico.Contextos.V2.CargaDeCompromissoContext.Data;

Log.Logger = new LoggerConfiguration()
          .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
          .Enrich.FromLogContext()
          .WriteTo.Console()
          .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

builder.Services.AddTransient<EmailSender>(i =>
    new EmailSender(
        Oi.Juridico.AddOn.Configuracoes.Smtp.Host,
        Oi.Juridico.AddOn.Configuracoes.Smtp.Port,
        Oi.Juridico.AddOn.Configuracoes.Smtp.EnableSSL,
        Oi.Juridico.AddOn.Configuracoes.Smtp.Email,
        Oi.Juridico.AddOn.Configuracoes.Smtp.Name,
        Oi.Juridico.AddOn.Configuracoes.Smtp.Password,
        Oi.Juridico.AddOn.Configuracoes.Smtp.UseCredentials
    )
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/csv", "text/plain" });
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddSingleton<IHashids>(_ => new Hashids("nZxTYJlhAJ2b7cbOA3dbtV7UFqmVjc6RBb7b1oCd47vqlZPabb0QL9ii0Cd7l7914UzG97VviXLQso7yFDMILT1Meisf@6Og@74suU6j#KjXvbpQso6mSpsxKy$vCBoI", 15));
builder.Services.AddSingleton<JwtSettings>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddCors();

builder.Services.AddMvc(options =>
{
    options.Filters.Add<OperationCancelledExceptionFilter>();

    // configura a autentica��o em todos os endpoints
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));

    options.EnableEndpointRouting = false;
    //options.OutputFormatters.Add(new CsvOutputFormatter(new CsvFormatterOptions(";")));
    options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local;
});

builder.Services
    .AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
    .Configure<JwtSettings>((opts, jwtSettings) =>
    {
        opts.RequireHttpsMetadata = false;
        opts.SaveToken = true;
        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = jwtSettings.SigningCredentials.Key,
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.Configure<IISOptions>(o =>
{
    o.ForwardClientCertificate = false;
});

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer();

// Ativa o uso do token como forma de autorizar o acesso
// a recursos deste projeto
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser().Build());
});

// configura��o do Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api do projeto Oi.Juridico.WebApi.V2", Version = "v1" });
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

// ORACLE / CONTEXTOS
string connectionString = Oi.Juridico.AddOn.Configuracoes.ConnectionString.JurUserComPool;

builder.Services.AddOracle<AgendamentoCalculoVEPDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<RelatorioPagamentoEscritorioDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<AtmJecContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<RelatorioMovimentacoesJecContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<AtmPexContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ParametrizacaoClosingContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ParametroJuridicoContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ControleDeAcessoContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<PermissaoContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<SolicitacaoAcessoContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<FechamentoContingenciaContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<PautaJuizadoPesquisaContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<PautaJuizadoComposicaoContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<RelatorioMovimentacoesCivelConsumidorContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<RelatorioSolicitacaoLancamentoPexContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<AgendamentoLogProcessoContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ManutencaoDbContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<FechamentoTrabalhistaContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<RelatorioMovimentacaoTrabalhistaContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<PerfilContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<DistribuicaoProcessoEscritorioContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<CriminalContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<AgendaAudienciaContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<ESocialDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ESocialLogDbContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<ManutencaoSolicitanteContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ManutencaoEscritorioContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ManutencaoContratoEscritorioDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<EmpresaContratadaDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<ResultadoNegociacaoDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<StatusContatoDbContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<SequenceDbContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<AgendamentoRelatorioNegociacaoDbContext>(connectionString, OracleOptions(), ContextOptions());
builder.Services.AddOracle<AgendCargaCompromissoDbContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<AtmCCContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<LogProcessoTrabalhistaContext>(connectionString, OracleOptions(), ContextOptions());

builder.Services.AddOracle<CargaDeCompromissoDbContext>(connectionString, OracleOptions(), ContextOptions());

// Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ControleDeAcessoService>();

// Services - eSocial
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services.ESocialF2500RemuneracaoService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services.ESocialF2500MudancaCategoriaService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services.ESocialF2500UnicContrService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services.ESocialF2500InfoContratoService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services.ESocialF2500IdePeriodoService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.Services.ESocialF2500Service>();

builder.Services.AddTransient<ESocialF2500RemuneracaoService>();
builder.Services.AddTransient<ESocialF2500MudancaCategoriaService>();
builder.Services.AddTransient<ESocialF2500UnicContrService>();
builder.Services.AddTransient<ESocialF2500InfoContratoService>();
builder.Services.AddTransient<ESocialF2500IdePeriodoService>();
builder.Services.AddTransient<ESocialF2500Service>();
builder.Services.AddTransient<ESocialF2500AbonoService>();
builder.Services.AddTransient<ESocialF2500InfoIntermService>();

builder.Services.AddTransient<ESocialF2501BenefPenService>();
builder.Services.AddTransient<ESocialF2501DedSuspService>();
builder.Services.AddTransient<ESocialF2501IdeAdvService>();
builder.Services.AddTransient<ESocialF2501PenAlimService>();
builder.Services.AddTransient<ESocialF2501DedDepenService>();
builder.Services.AddTransient<ESocialF2501InfoProcRetService>();
builder.Services.AddTransient<ESocialF2501InfoValoresService>();
builder.Services.AddTransient<ESocialF2501InfoCrIrrfService>();
builder.Services.AddTransient<ESocialF2501Service>();
builder.Services.AddTransient<ESocialF2501InfoDepService>();

builder.Services.AddTransient<ESocialF2501Service>();

builder.Services.AddTransient<DBContextService>();

builder.Services.AddTransient<ESocialPartesProcessoService>();
builder.Services.AddTransient<ESocialDownloadRetornoService>();
builder.Services.AddTransient<ESocialDashboardService>();
builder.Services.AddTransient<ESocialDashboardRepository>();

builder.Services.AddTransient<ESocialF2501DedDepenService>();
builder.Services.AddTransient<ESocialF2501InfoCrIrrfService>();
builder.Services.AddTransient<ESocialF2501Service>();

builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.Services.AgendamentoRelatorioNegociacaoService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.Services.AgendamentoRelatorioATMService>();

// Services - audiencia
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.AgendaAudienciaTrabalhista.Services.AgendaAudienciaService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.LogProcesso.Trabalhista.Services.LogProcessoTrabalhistaService>();
builder.Services.AddTransient<Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.Services.AgendamentoCargaCompromissoService>();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() == false)
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
#if DEBUG
    var swaggerEndpoint = "/swagger/v1/swagger.json";
#else
    var swaggerEndpoint = "/apiV2/swagger/v1/swagger.json";
#endif
    app.UseSwaggerUI(c =>
    {
        c.EnableTryItOutByDefault();
        c.DisplayRequestDuration();
        c.DefaultModelsExpandDepth(-1);
        c.SwaggerEndpoint(swaggerEndpoint, "API SISJUR v2");
    });
}

app.UseAuthentication();

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

app.UseCors(builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader().WithExposedHeaders("Content-Disposition"));

app.UseMiddleware(typeof(PermissoesMiddleWare));
app.UseMvc();

app.UseAuthorization();
app.MapControllers();

app.Run();

static Action<OracleDbContextOptionsBuilder> OracleOptions()
{
    return options =>
    {
        options.UseOracleSQLCompatibility("11");
    };
}

static Action<DbContextOptionsBuilder> ContextOptions()
{
    return options =>
    {
        options.EnableDetailedErrors();
#if DEBUG
        options.EnableSensitiveDataLogging();
#endif
    };
}