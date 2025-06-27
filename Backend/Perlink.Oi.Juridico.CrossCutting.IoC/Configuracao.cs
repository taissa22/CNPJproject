using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Perlink.Oi.Juridico.Application;
using Perlink.Oi.Juridico.Application.AlteracaoBloco;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.Impl;
using Perlink.Oi.Juridico.Application.AlteracaoBloco.Interface;
using Perlink.Oi.Juridico.Application.Compartilhado;
using Perlink.Oi.Juridico.Application.Compartilhado.Impl;
using Perlink.Oi.Juridico.Application.Compartilhado.Interface;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Impl;
using Perlink.Oi.Juridico.Application.ControleDeAcesso.Interface;
using Perlink.Oi.Juridico.Application.Logs;
using Perlink.Oi.Juridico.Application.Logs.Impl;
using Perlink.Oi.Juridico.Application.Logs.Interface;
using Perlink.Oi.Juridico.Application.Manutencao.Handlers;
using Perlink.Oi.Juridico.Application.Manutencao.Impl;
using Perlink.Oi.Juridico.Application.Manutencao.Interface;
using Perlink.Oi.Juridico.Application.Processos.Impl;
using Perlink.Oi.Juridico.Application.Processos.Interface;
using Perlink.Oi.Juridico.Application.Relatorios;
using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Impl;
using Perlink.Oi.Juridico.Application.Relatorios.Contingencia.Interface;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations;
using Perlink.Oi.Juridico.Application.SAP;
using Perlink.Oi.Juridico.Application.SAP.Impl;
using Perlink.Oi.Juridico.Application.SAP.Impl.InterfaceBB;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.Interface.InterfaceBB;
using Perlink.Oi.Juridico.Application.Security;
using Perlink.Oi.Juridico.Data;
using Perlink.Oi.Juridico.Data.AlteracaoBloco.Repository;
using Perlink.Oi.Juridico.Data.Compartilhado.Repository;
using Perlink.Oi.Juridico.Data.ControleDeAcesso.Repository;
using Perlink.Oi.Juridico.Data.Logs.Repository;
using Perlink.Oi.Juridico.Data.Manutencao.AdoRepositories;
using Perlink.Oi.Juridico.Data.Manutencao.EFRepositories;
using Perlink.Oi.Juridico.Data.Manutencao.EFREpositories;
using Perlink.Oi.Juridico.Data.Processos.AdoRepositories;
using Perlink.Oi.Juridico.Data.Processos.EFRepositories;
using Perlink.Oi.Juridico.Data.Relatorios.Repository;
using Perlink.Oi.Juridico.Data.SAP.Repository;
using Perlink.Oi.Juridico.Data.SAP.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Data.SAP.Repository.Processos;
using Perlink.Oi.Juridico.Data.Transactions;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Repository;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Service;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Service;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Interface.Service;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Service;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Perlink.Oi.Juridico.Domain.External.Service;
using Perlink.Oi.Juridico.Domain.Logs.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Logs.Interface.Service;
using Perlink.Oi.Juridico.Domain.Logs.Service;
using Perlink.Oi.Juridico.Domain.Manutencao.Composer;
using Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.AdoRepositories;
using Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.EFRepository;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Service;
using Perlink.Oi.Juridico.Domain.Processos.Interface.AdoRepository;
using Perlink.Oi.Juridico.Domain.Processos.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Processos.Interface.Service;
using Perlink.Oi.Juridico.Domain.Processos.Service;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Relatorios.Contingencia.Interface.Service;
using Perlink.Oi.Juridico.Domain.Relatorios.Service;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service.Processos;
using Perlink.Oi.Juridico.Domain.SAP.Service;
using Perlink.Oi.Juridico.Domain.SAP.Service.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Service.Processos;
using Serilog;
using Shared.Domain.Interface;
using System.Reflection;
using IAcaoService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IAcaoService;
using IParteService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IParteService;
using IPedidoService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IPedidoService;
using IPercentualATMRepository = Perlink.Oi.Juridico.Application.Manutencao.Repositories.IPercentualATMRepository;
using IProfissionalService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IProfissionalService;
using PercentualATMRepository = Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations.PercentualATMRepository;
using IEmpresaDoGrupoService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IEmpresaDoGrupoService;
using IComarcaService = Perlink.Oi.Juridico.Domain.SAP.Interface.Service.IComarcaService;
using ITipoVaraService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.ITipoVaraService;
using IVaraService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IVaraService;
using IEstadoService = Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service.IEstadoService;
using EstadoService = Perlink.Oi.Juridico.Domain.Compartilhado.Service.EstadoService;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.Impl;
using Perlink.Oi.Juridico.Application.GrupoDeEstados.Interface;
using Perlink.Oi.Juridico.Application.GrupoDeEstados;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Service;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Service;
using Perlink.Oi.Juridico.Data.GrupoDeEstados.Repository;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Interface.Repository;
using Perlink.Oi.Juridico.Application.ContingenciaPex.Interface;
using Perlink.Oi.Juridico.Application.ContingenciaPex.Impl;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Service;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Service;
using Perlink.Oi.Juridico.Data.ContingenciaPex.Repository;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Interface.Repository;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.Interface;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.Impl;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Service;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Service;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Repository;
using Perlink.Oi.Juridico.Data.ApuracaoOutliers.Repository;

namespace Perlink.Oi.Juridico.CrossCutting.IoC
{
    public class Configuracao
    {
        public static void InjecaoDependencia(IServiceCollection services, string configuration)
        {
            services.AddHttpContextAccessor();
            services.AddEntityFrameworkOracle()
                .AddDbContextPool<JuridicoContext>((serviceProvider, c) =>
                {
                    c.UseOracle(configuration, options => options.UseOracleSQLCompatibility("11")).EnableSensitiveDataLogging(true);
                    c.UseInternalServiceProvider(serviceProvider);
                });

            InjectDependence(services);
        }

        public static void InjecaoDependenciaTest(IServiceCollection services)
        {
            InjectDependence(services);
        }

        private static void InjectDependence(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ControleDeAcessoConfigurationProfile).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(CompartilhadoConfigurationProfile).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(SAPConfigurationProfile).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(AlteracaoEmBlocoConfigurationProfile).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(LogsConfigurationProfile).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(RelatoriosConfigurationProfile).GetTypeInfo().Assembly);
            services.AddAutoMapper(typeof(GrupoDeEstadosConfigurationProfile).GetTypeInfo().Assembly);

            Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                        .CreateLogger();

            services.AddSingleton<ILogger>(Log.Logger);
            // services.AddSingleton<IHostingEnvironment>(new HostingEnvironment());
            //Colocar em ordem alfabetica para facilitar a busca

            #region area envolvida

            services.AddScoped<IAreaEnvolvidaRepository, AreaEnvolvidaRepository>();

            #endregion area envolvida

            #region AlteracaoEmBloco

            services.AddScoped<IAlteracaoEmBlocoAppService, AlteracaoEmBlocoAppService>();
            services.AddScoped<IAlteracaoEmBlocoService, AlteracaoEmBlocoService>();
            services.AddScoped<IAlteracaoEmBlocoRepository, AlteracaoEmBlocoRepository>();

            #endregion AlteracaoEmBloco

            services.AddScoped<IUow, Uow>();

            #region Configuracao

            services.AddScoped<IConfiguracaoAppService, ConfiguracaoAppService>();

            #endregion Configuracao

            #region GrupoDeEstados

            services.AddScoped<IGrupoDeEstadosAppService, GrupoDeEstadosAppService>();
            services.AddScoped<IGrupoDeEstadosService, GrupoDeEstadosService>();
            services.AddScoped<IGrupoDeEstadosRepository, GrupoDeEstadosRepository>(); 
            services.AddScoped<IGrupoEstadosService, GrupoEstadosService>();
            services.AddScoped<IGrupoEstadosRepository, GrupoEstadosRepository>();

            #endregion Configuracao

            #region Parametro

            services.AddScoped<IParametroAppService, ParametroAppService>();
            services.AddScoped<IParametroService, ParametroService>();
            services.AddScoped<IParametroRepository, ParametroRepository>();

            #endregion Parametro

            #region Usuario

            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IAutenticacaoAppService, AutenticacaoAppService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();

            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ITokenSegurancaRepository, TokenSegurancaRepository>();

            #endregion Usuario

            #region Permissao

            services.AddScoped<IPermissaoAppService, PermissaoAppService>();
            services.AddScoped<IPermissaoService, PermissaoService>();
            services.AddScoped<IPermissaoRepository, PermissaoRepository>();

            #endregion Permissao

            #region Perfil

            services.AddScoped<IPerfilAppService, PerfilAppService>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IPerfilRepository, PerfilRepository>();

            #endregion Perfil

            #region UsuarioGrupo

            services.AddScoped<IUsuarioGrupoRepository, UsuarioGrupoRepository>();

            #endregion UsuarioGrupo

            #region EmpresadoDoGrupo

            services.AddScoped<IEmpresaDoGrupoAppService, EmpresaDoGrupoAppService>();
            services.AddScoped<IEmpresaDoGrupoService, EmpresaDoGrupoService>();
            services.AddScoped<IEmpresaDoGrupoRepository, EmpresaDoGrupoRepository>();

            #endregion EmpresadoDoGrupo

            #region Escritorio

            services.AddScoped<IEscritorioAppService, EscritorioAppService>();
            services.AddScoped<Domain.Compartilhado.Interface.Service.IEscritorioService, EscritorioService>();
            services.AddScoped<IEscritorioRepository, EscritorioRepository>();

            #endregion Escritorio

            #region Banco

            services.AddScoped<IBancoAppService, BancoAppService>();
            services.AddScoped<IBancoService, BancoService>();
            services.AddScoped<IBancoRepository, BancoRepository>();

            #endregion Banco

            #region Grupo Empresa Contabil SAP

            services.AddScoped<IGrupoEmpresaContailSapAppService, GrupoEmpresaContailSapAppService>();
            services.AddScoped<IGrupoEmpresaContabilSapService, GrupoEmpresaContabilSapService>();
            services.AddScoped<IGrupoEmpresaContabilSapRepository, GrupoEmpresaContabilSapRepository>();
            services.AddScoped<IGrupoEmpresaContabilSapParteRepository, GrupoEmpresaContabilSapParteRepository>();

            #endregion Grupo Empresa Contabil SAP

            #region FornecedorasContratos

            services.AddScoped<IFornecedorasContratosService, FornecedorasContratosService>();
            services.AddScoped<IFornecedorasContratosRepository, FornecedorasContratosRepository>();

            #endregion FornecedorasContratos

            #region FornecedorContigencia

            services.AddScoped<IFornecedorContigenciaAppService, FornecedorContigenciaAppService>();
            services.AddScoped<IFornecedorContigenciaService, FornecedorContigenciaService>();
            services.AddScoped<IFornecedorContigenciaRepository, FornecedorContigenciaRepository>();

            #endregion FornecedorContigencia

            #region FornecedorasFaturas

            services.AddScoped<IFornecedorasFaturasService, FornecedorasFaturasService>();
            services.AddScoped<IFornecedorasFaturasRepository, FornecedorasFaturasRepository>();

            #endregion FornecedorasFaturas

            #region StatusPagamento

            services.AddScoped<IStatusPagamentoService, StatusPagamentoService>();
            services.AddScoped<IStatusPagamentoRepository, StatusPagamentoRepository>();

            #endregion StatusPagamento

            #region Lote

            services.AddScoped<ILoteAppService, LoteAppService>();
            services.AddScoped<ILoteService, LoteService>();
            services.AddScoped<ILoteRepository, LoteRepository>();

            #endregion Lote

            #region TipoProcesso

            services.AddScoped<ITipoProcessoAppService, TipoProcessoAppService>();
            services.AddScoped<ITipoProcessoService, TipoProcessoService>();
            services.AddScoped<ITipoProcessoRepository, TipoProcessoRepository>();

            #endregion TipoProcesso

            #region LogLote

            services.AddScoped<ILog_LoteAppService, Log_LoteAppService>();
            services.AddScoped<ILog_LoteService, Log_LoteService>();
            services.AddScoped<ILog_LoteRepository, Log_LoteRepository>();

            #endregion LogLote

            #region Bordero

            services.AddScoped<IBorderoAppService, BorderoAppService>();
            services.AddScoped<IBorderoService, BorderoService>();
            services.AddScoped<IBorderoRepository, borderoRepository>();

            #endregion Bordero

            #region EmpresasCentralizadoras

            services.AddScoped<IEmpresasCentralizadorasService, EmpresasCentralizadorasService>();
            services.AddScoped<IEmpresasCentralizadorasRepository, EmpresasCentralizadorasRepository>();

            #endregion EmpresasCentralizadoras

            #region Comarca

            services.AddScoped<IComarcaAppService, ComarcaAppService>();
            services.AddScoped<IComarcaService, ComarcaService>();
            services.AddScoped<IComarcaRepository, ComarcaRepository>();

            #endregion Comarca

            #region LancamentoProcesso

            services.AddScoped<ILancamentoProcessoAppService, LancamentoProcessoAppService>();
            services.AddScoped<ILancamentoProcessoService, LancamentoProcessoService>();
            services.AddScoped<ILancamentoProcessoRepository, LancamentoProcessoRepository>();

            #endregion LancamentoProcesso

            #region Processo

            services.AddScoped<IProcessoAppService, ProcessoAppService>();
            services.AddScoped<IProcessoService, ProcessoService>();
            services.AddScoped<IProcessoRepository, ProcessoRepository>();

            #endregion Processo

            #region Log Processo

            services.AddScoped<ILogProcessoAppService, LogProcessoAppService>();
            services.AddScoped<ILogProcessoService, LogProcessoService>();
            services.AddScoped<ILogProcessoRepository, LogProcessoRepository>();

            #endregion Log Processo



            #region Log Pedido Processo

            services.AddScoped<ILogPedidoProcessoAppService, LogPedidoProcessoAppService>();
            services.AddScoped<ILogPedidoProcessoService, LogPedidoProcessoService>();
            services.AddScoped<ILogPedidoProcessoRepository, LogPedidoProcessoRepository>();

            #endregion Log Pedido Processo



            #region Log Parte Processo

            services.AddScoped<ILogParteProcessoAppService, LogParteProcessoAppService>();
            services.AddScoped<ILogParteProcessoService, LogParteProcessoService>();
            services.AddScoped<ILogParteProcessoRepository, LogParteProcessoRepository>();

            #endregion Log Parte Processo



            #region Log Valor Processo

            services.AddScoped<ILogValorProcessoAppService, LogValorProcessoAppService>();
            services.AddScoped<ILogValorProcessoService, LogValorProcessoService>();
            services.AddScoped<ILogValorProcessoRepository, LogValorProcessoRepository>();

            #endregion Log Valor Processo



            #region Log Contratos e Pedidos dos Lançamentos

            services.AddScoped<ILogLancamentoContratoPedidoProcessoAppService, LogLancamentoContratoPedidoProcessoAppService>();
            services.AddScoped<ILogLancamentoContratoPedidoProcessoService, LogLancamentoContratoPedidoProcessoService>();
            services.AddScoped<ILogLancamentoContratoPedidoProcessoRepository, LogLancamentoContratoPedidoProcessoRepository>();

            #endregion Log Contratos e Pedidos dos Lançamentos



            #region Log Decisões de Andamento

            services.AddScoped<ILogDecisaoContratoPedidoProcessoAppService, LogDecisaoContratoPedidoProcessoAppService>();
            services.AddScoped<ILogDecisaoContratoPedidoProcessoService, LogDecisaoContratoPedidoProcessoService>();
            services.AddScoped<ILogDecisaoContratoPedidoProcessoRepository, LogDecisaoContratoPedidoProcessoRepository>();

            #endregion Log Decisões de Andamento

            #region Log Documento Processo

            services.AddScoped<ILogDocumentoProcessoAppService, LogDocumentoProcessoAppService>();
            services.AddScoped<ILogDocumentoProcessoService, LogDocumentoProcessoService>();
            services.AddScoped<ILogDocumentoProcessoRepository, LogDocumentoProcessoRepository>();

            #endregion Log Documento Processo

            #region Log Documento Andamentos

            services.AddScoped<ILogDocumentoAndamentoProcessoAppService, LogDocumentoAndamentoProcessoAppService>();
            services.AddScoped<ILogDocumentoAndamentoProcessoService, LogDocumentoAndamentoProcessoService>();
            services.AddScoped<ILogDocumentoAndamentoProcessoRepository, LogDocumentoAndamentoProcessoRepository>();

            #endregion Log Documento Andamentos

            #region Log Documento Lancamentos

            services.AddScoped<ILogDocumentoLancamentoProcessoAppService, LogDocumentoLancamentoProcessoAppService>();
            services.AddScoped<ILogDocumentoLancamentoProcessoService, LogDocumentoLancamentoProcessoService>();
            services.AddScoped<ILogDocumentoLancamentoProcessoRepository, LogDocumentoLancamentoProcessoRepository>();

            #endregion Log Documento Lancamentos

            #region Log Participantes e Papéis dos Andamentos

            services.AddScoped<ILogParticipantesPapeisAndamentosAppService, LogParticipantesPapeisAndamentosAppService>();
            services.AddScoped<ILogParticipantesPapeisAndamentosService, LogParticipantesPapeisAndamentosService>();
            services.AddScoped<ILogParticipantesPapeisAndamentosRepository, LogParticipantesPapeisAndamentosRepository>();

            #endregion Log Participantes e Papéis dos Andamentos

            #region Log Andamento Processo

            services.AddScoped<ILogAndamentoProcessoAppService, LogAndamentoProcessoAppService>();
            services.AddScoped<ILogAndamentoProcessoService, LogAndamentoProcessoService>();
            services.AddScoped<ILogAndamentoProcessoRepository, LogAndamentoProcessoRepository>();

            #endregion Log Andamento Processo



            #region Log Andamento Serviço Processo

            services.AddScoped<ILogAndamentoServicoProcessoAppService, LogAndamentoServicoProcessoAppService>();
            services.AddScoped<ILogAndamentoServicoProcessoService, LogAndamentoServicoProcessoService>();
            services.AddScoped<ILogAndamentoServicoProcessoRepository, LogAndamentoServicoProcessoRepository>();

            #endregion Log Andamento Serviço Processo



            #region Log Andamento Pedido Processo

            services.AddScoped<ILogAndamentoPedidoProcessoAppService, LogAndamentoPedidoProcessoAppService>();
            services.AddScoped<ILogAndamentoPedidoProcessoService, LogAndamentoPedidoProcessoService>();
            services.AddScoped<ILogAndamentoPedidoProcessoRepository, LogAndamentoPedidoProcessoRepository>();

            #endregion Log Andamento Pedido Processo



            #region Log Observacao Processo

            services.AddScoped<ILogObservacaoProcessoAppService, LogObservacaoProcessoAppService>();
            services.AddScoped<ILogObservacaoProcessoService, LogObservacaoProcessoService>();
            services.AddScoped<ILogObservacaoProcessoRepository, LogObservacaoProcessoRepository>();

            #endregion Log Observacao Processo



            #region Log Audiencia Processo

            services.AddScoped<ILogAudienciaProcessoAppService, LogAudienciaProcessoAppService>();
            services.AddScoped<ILogAudienciaProcessoService, LogAudienciaProcessoService>();
            services.AddScoped<ILogAudienciaProcessoRepository, LogAudienciaProcessoRepository>();

            #endregion Log Audiencia Processo



            #region Log Lancamento Processo

            services.AddScoped<ILogLancamentoProcessoAppService, LogLancamentoProcessoAppService>();
            services.AddScoped<ILogLancamentoProcessoService, LogLancamentoProcessoService>();
            services.AddScoped<ILogLancamentoProcessoRepository, LogLancamentoProcessoRepository>();

            #endregion Log Lancamento Processo



            #region Log Lote Lancamento Processo

            services.AddScoped<ILogLoteProcessoAppService, LogLoteProcessoAppService>();
            services.AddScoped<ILogLoteLancamentoProcessoService, LogLoteLancamentoProcessoService>();
            services.AddScoped<ILogLoteLancamentoProcessoRepository, LogLoteLancamentoProcessoRepository>();

            #endregion Log Lote Lancamento Processo



            #region Log Lote

            services.AddScoped<ILogLoteProcessoService, LogLoteProcessoService>();
            services.AddScoped<ILogLoteProcessoRepository, LogLoteProcessoRepository>();

            #endregion Log Lote

            #region LogPrazoProcesso

            services.AddScoped<ILogPrazoProcessoAppService, LogPrazoProcessoAppService>();
            services.AddScoped<ILogPrazoProcessoService, LogPrazoProcessoService>();
            services.AddScoped<ILogPrazoProcessoRepository, LogPrazoProcessoRepository>();

            #endregion LogPrazoProcesso

            #region Log Advogado Autor Processo

            services.AddScoped<ILogAdvogadoAutorProcessoAppService, LogAdvogadoAutorProcessoAppService>();
            services.AddScoped<ILogAdvogadoAutorProcessoService, LogAdvogadoAutorProcessoService>();
            services.AddScoped<ILogAdvogadoAutorProcessoRepository, LogAdvogadoAutorProcessoRepository>();

            #endregion Log Advogado Autor Processo

            #region Log Linha Processo

            services.AddScoped<ILogLinhaProcessoAppService, LogLinhaProcessoAppService>();
            services.AddScoped<ILogLinhaProcessoService, LogLinhaProcessoService>();
            services.AddScoped<ILogLinhaProcessoRepository, LogLinhaProcessoRepository>();

            #endregion Log Linha Processo

            #region Log Apuracao Dossie

            services.AddScoped<ILogApuracaoDossieAppService, LogApuracaoDossieAppService>();
            services.AddScoped<ILogApuracaoDossieService, LogApuracaoDossieService>();
            services.AddScoped<ILogApuracaoDossieRepository, LogApuracaoDossieRepository>();

            #endregion Log Apuracao Dossie

            #region Log Apuracao Bloqueio

            services.AddScoped<ILogApuracaoBloqueioAppService, LogApuracaoBloqueioAppService>();
            services.AddScoped<ILogApuracaoBloqueioService, LogApuracaoBloqueioService>();
            services.AddScoped<ILogApuracaoBloqueioRepository, LogApuracaoBloqueioRepository>();

            #endregion Log Apuracao Bloqueio

            #region Log Apuracao Reparo

            services.AddScoped<ILogApuracaoReparoAppService, LogApuracaoReparoAppService>();
            services.AddScoped<ILogApuracaoReparoService, LogApuracaoReparoService>();
            services.AddScoped<ILogApuracaoReparoRepository, LogApuracaoReparoRepository>();

            #endregion Log Apuracao Reparo

            #region Log Apuracao Contestacao

            services.AddScoped<ILogApuracaoContestacaoAppService, LogApuracaoContestacaoAppService>();
            services.AddScoped<ILogApuracaoContestacaoService, LogApuracaoContestacaoService>();
            services.AddScoped<ILogApuracaoContestacaoRepository, LogApuracaoContestacaoRepository>();

            #endregion Log Apuracao Contestacao

            #region Log Apuracao Conta em Debito

            services.AddScoped<ILogApuracaoContaEmDebitoAppService, LogApuracaoContaEmDebitoAppService>();
            services.AddScoped<ILogApuracaoContaEmDebitoService, LogApuracaoContaEmDebitoService>();
            services.AddScoped<ILogApuracaoContaEmDebitoRepository, LogApuracaoContaEmDebitoRepository>();

            #endregion Log Apuracao Conta em Debito

            #region Log Apuracao Carta Cobranca

            services.AddScoped<ILogApuracaoCartaCobrancaAppService, LogApuracaoCartaCobrancaAppService>();
            services.AddScoped<ILogApuracaoCartaCobrancaService, LogApuracaoCartaCobrancaService>();
            services.AddScoped<ILogApuracaoCartaCobrancaRepository, LogApuracaoCartaCobrancaRepository>();

            #endregion Log Apuracao Carta Cobranca

            #region Log Apuracao Protecao Credito

            services.AddScoped<ILogApuracaoProtecaoCreditoAppService, LogApuracaoProtecaoCreditoAppService>();
            services.AddScoped<ILogApuracaoProtecaoCreditoService, LogApuracaoProtecaoCreditoService>();
            services.AddScoped<ILogApuracaoProtecaoCreditoRepository, LogApuracaoProtecaoCreditoRepository>();

            #endregion Log Apuracao Protecao Credito

            #region Log Apuracao Medicao Pulso

            services.AddScoped<ILogApuracaoMedicaoPulsoAppService, LogApuracaoMedicaoPulsoAppService>();
            services.AddScoped<ILogApuracaoMedicaoPulsoService, LogApuracaoMedicaoPulsoService>();
            services.AddScoped<ILogApuracaoMedicaoPulsoRepository, LogApuracaoMedicaoPulsoRepository>();

            #endregion Log Apuracao Medicao Pulso

            #region Log Decisao Pedido Processo

            services.AddScoped<ILogDecisaoPedidoProcessoAppService, LogDecisaoPedidoProcessoAppService>();
            services.AddScoped<ILogDecisaoPedidoProcessoService, LogDecisaoPedidoProcessoService>();
            services.AddScoped<ILogDecisaoPedidoProcessoRepository, LogDecisaoPedidoProcessoRepository>();

            #endregion Log Decisao Pedido Processo

            #region Log Motivacao Processo

            services.AddScoped<ILogMotivacaoProcessoAppService, LogMotivacaoProcessoAppService>();
            services.AddScoped<ILogMotivacaoProcessoService, LogMotivacaoProcessoService>();
            services.AddScoped<ILogMotivacaoProcessoRepository, LogMotivacaoProcessoRepository>();

            #endregion Log Motivacao Processo

            #region Log Pendencia Processo

            services.AddScoped<ILogPendenciaProcessoAppService, LogPendenciaProcessoAppService>();
            services.AddScoped<ILogPendenciaProcessoService, LogPendenciaProcessoService>();
            services.AddScoped<ILogPendenciaProcessoRepository, LogPendenciaProcessoRepository>();

            #endregion Log Pendencia Processo

            #region Log Processo Incidente

            services.AddScoped<ILogProcessoIncidenteAppService, LogProcessoIncidenteAppService>();
            services.AddScoped<ILogProcessoIncidenteService, LogProcessoIncidenteService>();
            services.AddScoped<ILogProcessoIncidenteRepository, LogProcessoIncidenciaRepository>();

            #endregion Log Processo Incidente

            #region Log Proposta Contato Processo

            services.AddScoped<ILogPropostaContatoProcessoAppService, LogPropostaContatoProcessoAppService>();
            services.AddScoped<ILogPropostaContatoProcessoService, LogPropostaContatoProcessoService>();
            services.AddScoped<ILogPropostaContatoProcessoRepository, LogPropostaContatoProcessoRepository>();

            #endregion Log Proposta Contato Processo



            #region Log Contato Processo

            services.AddScoped<ILogContatoProcessoAppService, LogContatoProcessoAppService>();
            services.AddScoped<ILogContatoProcessoService, LogContatoProcessoService>();
            services.AddScoped<ILogContatoProcessoRepository, LogContatoProcessoRepository>();

            #endregion Log Contato Processo

            #region Log Servico Processo

            services.AddScoped<ILogServicoProcessoAppService, LogServicoProcessoAppService>();
            services.AddScoped<ILogServicoProcessoService, LogServicoProcessoService>();
            services.AddScoped<ILogServicoProcessoRepository, LogServicoProcessoRepository>();

            #endregion Log Servico Processo

            #region Log Servico Proposta

            services.AddScoped<ILogServicoPropostaAppService, LogServicoPropostaAppService>();
            services.AddScoped<ILogServicoPropostaService, LogServicoPropostaService>();
            services.AddScoped<ILogServicoPropostaRepository, LogServicoPropostaRepository>();

            #endregion Log Servico Proposta

            #region Log Parte Pedido Processo

            services.AddScoped<ILogPartePedidoProcessoAppService, LogPartePedidoProcessoAppService>();
            services.AddScoped<ILogPartePedidoProcessoService, LogPartePedidoProcessoService>();
            services.AddScoped<ILogPartePedidoProcessoRepository, LogPartePedidoProcessoRepository>();

            #endregion Log Parte Pedido Processo

            #region Log Objeto

            services.AddScoped<ILogObjetoAppService, LogObjetoAppService>();
            services.AddScoped<ILogObjetoService, LogObjetoService>();
            services.AddScoped<ILogObjetoRepository, LogObjetoRepository>();

            #endregion Log Objeto

            #region Log DIRF Processo

            services.AddScoped<ILogDirfProcessoAppService, LogDirfProcessoAppService>();
            services.AddScoped<ILogDirfProcessoService, LogDirfProcessoService>();
            services.AddScoped<ILogDirfProcessoRepository, LogDirfProcessoRepository>();

            #endregion Log DIRF Processo

            #region Log Gfip Processo

            services.AddScoped<ILogGfipProcessoAppService, LogGfipProcessoAppService>();
            services.AddScoped<ILogGfipProcessoService, LogGfipProcessoService>();
            services.AddScoped<ILogGfipProcessoRepository, LogGfipProcessoRepository>();

            #endregion Log Gfip Processo

            #region Log Processos Conexos

            services.AddScoped<ILogProcessosConexosAppService, LogProcessosConexosAppService>();
            services.AddScoped<ILogProcessosConexosService, LogProcessosConexosService>();
            services.AddScoped<ILogProcessosConexosRepository, LogProcessosConexosRepository>();

            #endregion Log Processos Conexos

            #region Log Ecaminhamentos Processos

            services.AddScoped<ILogEncaminhamentosProcessosAppService, LogEncaminhamentosProcessosAppService>();
            services.AddScoped<ILogEncaminhamentosProcessosService, LogEncaminhamentosProcessosService>();
            services.AddScoped<ILogEncaminhamentosProcessosRepository, LogEncaminhamentosProcessosRepository>();

            #endregion Log Ecaminhamentos Processos

            #region Log Decisao Andamento Processo

            services.AddScoped<ILogDecisaoAndamentoProcessoAppService, LogDecisaoAndamentoProcessoAppService>();
            services.AddScoped<ILogDecisaoAndamentoProcessoService, LogDecisaoAndamentoProcessoService>();
            services.AddScoped<ILogDecisaoAndamentoProcessoRepository, LogDecisaoAndamentoProcessoRepository>();

            #endregion Log Decisao Andamento Processo

            #region Parte

            services.AddScoped<IParteService, ParteService>();
            services.AddScoped<IParteRepository, ParteRepository>();

            #endregion Parte

            #region Log Valores Objetos/Sub-Objetos

            services.AddScoped<ILogValoresObjAdmProcessoAppService, LogValoresObjAdmProcessoAppService>();
            services.AddScoped<ILogValoresObjAdmProcessoService, LogValoresObjAdmProcessoService>();
            services.AddScoped<ILogValoresObjAdmProcessoRepository, LogValoresObjAdmProcessoRepository>();

            #endregion Log Valores Objetos/Sub-Objetos

            #region Log Valores dos Contratos

            services.AddScoped<ILogContratoPedidoProcessoAppService, LogContratoPedidoProcessoAppService>();
            services.AddScoped<ILogContratoPedidoProcessoService, LogContratoPedidoProcessoService>();
            services.AddScoped<ILogContratoPedidoProcessoRepository, LogContratoPedidoProcessoRepository>();

            #endregion Log Valores dos Contratos

            #region Log Contratos do processo

            services.AddScoped<ILogContratoProcessoAppService, LogContratoProcessoAppService>();
            services.AddScoped<ILogContratoProcessoService, LogContratoProcessoService>();
            services.AddScoped<ILogContratoProcessoRepository, LogContratoProcessoRepository>();

            #endregion Log Contratos do processo

            #region LogAndamentoLancamentoProcesso

            services.AddScoped<ILogAndamentoLancProcessoAppService, LogAndamentoLancProcessoAppService>();
            services.AddScoped<IlogAndamentoLancamentoProcessoService, LogAndamentoLancProcessoService>();
            services.AddScoped<ILogAndamentoLancProcessoRepository, LogAndamentoLacProcessoRepository>();

            #endregion LogAndamentoLancamentoProcesso

            #region LogUsuarioAgendamentoProcesso

            services.AddScoped<ILogUsuarioAgendamentoAppService, LogUsuarioAgendamentoAppService>();
            services.AddScoped<ILogUsuarioAgendamentoService, LogUsuarioAgendamentoService>();
            services.AddScoped<ILogUsuarioAgendamentoRepository, LogUsuarioAgendamentoRepository>();

            #endregion LogUsuarioAgendamentoProcesso

            #region Parte

            services.AddScoped<IPrepostoRepository, PrepostoRepository>();
            services.AddScoped<IPrepostoService, PrepostoService>();

            #endregion Parte

            #region Fornecedor

            services.AddScoped<IFornecedorAppService, FornecedorAppService>();
            services.AddScoped<IFornecedorService, FornecedorService>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();

            #endregion Fornecedor

            #region Classes Garantias

            services.AddScoped<IClassesGarantiasRepository, ClassesGarantiasRepository>();

            #endregion Classes Garantias

            #region Grupo Correções Garantias

            services.AddScoped<IGrupoCorrecoesGarantiasRepository, GrupoCorrecoesGarantiasRepository>();

            #endregion Grupo Correções Garantias

            #region EmpresasSapFornecedoras

            services.AddScoped<IEmpresasSapFornecedorasService, EmpresasSapFornecedorasService>();
            services.AddScoped<IEmpresasSapFornecedorasRepository, EmpresasSapFornecedorasRepository>();

            #endregion EmpresasSapFornecedoras

            #region Centro Custo

            services.AddScoped<ICentroCustoAppService, CentroCustoAppService>();
            services.AddScoped<ICentroCustoService, CentroCustoService>();
            services.AddScoped<ICentroCustoRepository, CentroCustoRepository>();

            #endregion Centro Custo

            #region Empresas_Sap

            services.AddScoped<IEmpresas_SapService, Empresas_SapService>();
            services.AddScoped<IEmpresas_SapAppService, Empresas_SapAppService>();
            services.AddScoped<IEmpresas_SapRepository, Empresas_SapRepository>();

            #endregion Empresas_Sap

            #region Pedido

            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            #endregion Pedido

            #region CompromissoProcesso, CompromissoProcessoCredor, CompromissoProcessoParcela

            services.AddScoped<ICompromissoProcessoRepository, CompromissoProcessoRepository>();
            services.AddScoped<ICompromissoProcessoCredorRepository, CompromissoProcessoCredorRepository>();
            services.AddScoped<ICompromissoProcessoParcelaRepository, CompromissoProcessoParcelaRepository>();

            services.AddScoped<ICompromissoProcessoCredorService, CompromissoProcessoCredorService>();
            services.AddScoped<ICompromissoProcessoService, CompromissoProcessoService>();
            services.AddScoped<ICompromissoProcessoParcelaService, CompromissoProcessoParcelaService>();

            #endregion CompromissoProcesso, CompromissoProcessoCredor, CompromissoProcessoParcela

            #region GruposLotesJuizados

            services.AddScoped<IGruposLotesJuizadosService, GruposLotesJuizadosService>();
            services.AddScoped<IGruposLotesJuizadosRepository, GruposLotesJuizadosRepository>();
            services.AddScoped<IGruposLotesJuizadosAppService, GruposLotesJuizadosAppService>();

            #endregion GruposLotesJuizados

            #region Tipo Lancamento

            services.AddScoped<ITipoLancamentoService, TipoLancamentoService>();
            services.AddScoped<ITipoLancamentoRepository, TipoLancamentoRepository>();

            #endregion Tipo Lancamento

            #region Categoria Pagamento

            services.AddScoped<ICategoriaPagamentoService, CategoriaPagamentoService>();
            services.AddScoped<ICategoriaPagamentoRepository, CategoriaPagamentoRepository>();
            services.AddScoped<ICategoriaPagamentoAppService, CategoriaPagamentoAppService>();
            services.AddScoped<IMigracaoCategoriaPagamentoService, MigracaoCategoriaPagamentoService>();
            services.AddScoped<IMigracaoCategoriaPagamentoRepository, MigracaoCategoriaPagamentoRepository>();

            services.AddScoped<IMigCategoriaPagamentoEstrategicoService, MigCategoriaPagamentoEstrategicoService>();
            services.AddScoped<IMigCategoriaPagamentoEstrategicoRepository, MigCategoriaPagamentoEstrategicoRepository>();

            services.AddScoped<ILog_CategoriaPagamentoService, Log_CategoriaPagamentoService>();
            services.AddScoped<ILog_CategoriaPagamentoRepository, Log_CategoriaPagamentoRepository>();

            


            #endregion Categoria Pagamento

            #region Categoria Finalização

            services.AddScoped<ICategoriaFinalizacaoRepository, CategoriaFinalizacaoRepository>();
            services.AddScoped<ICategoriaFinalizacaoService, CategoriaFinalizacaoService>();

            #endregion Categoria Finalização

            #region Lote Lancamento

            services.AddScoped<ILoteLancamentoService, LoteLancamentoService>();
            services.AddScoped<ILoteLancamentoRepository, LoteLancamentoRepository>();

            #endregion Lote Lancamento

            #region Pedido SAP

            services.AddScoped<IPedidoSAPService, PedidoSAPService>();
            services.AddScoped<IPedidoSAPRepository, PedidoSAPRepository>();

            #endregion Pedido SAP

            #region Fornecedor_Forma_Pagamento

            services.AddScoped<IFornecedorFormaPagamentoService, FornecedorFormaPagamentoService>();
            services.AddScoped<IFornecedorFormaPagamentoRepository, FornecedorFormaPagamentoRepository>();

            #endregion Fornecedor_Forma_Pagamento

            #region Forma Pagamento

            services.AddScoped<IFormaPagamentoService, FormaPagamentoService>();
            services.AddScoped<IFormaPagamentoRepository, FormaPagamentoRepository>();
            services.AddScoped<IFormaPagamentoAppService, FormaPagamentoAppService>();

            #endregion Forma Pagamento

            #region Pedido

            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();

            #endregion Pedido

            #region Status Compromisso Parcela

            services.AddScoped<IStatusCompromissoParcelaService, StatusCompromissoParcelaService>();
            services.AddScoped<IStatusCompromissoParcelaRepository, StatusCompromissoParcelaRepository>();

            #endregion Status Compromisso Parcela

            #region Motivo Suspensão Cancelamento Parcela

            services.AddScoped<IMotivoSuspensaoCancelamentoParcelaRepository, MotivoSuspensaoCancelamentoParcelaRepository>();
            services.AddScoped<IMotivoSuspensaoCancelamentoParcelaService, MotivoSuspensaoCancelamentoParcelaService>();

            #endregion Motivo Suspensão Cancelamento Parcela

            #region Credor Compromisso

            services.AddScoped<ICredorCompromissoRepository, CredorCompromissoRepository>();
            services.AddScoped<ICredorCompromissoService, CredorCompromissoService>();

            #endregion Credor Compromisso

            #region Credor Compromisso

            services.AddScoped<IFeriadosRepository, FeriadosRepository>();
            services.AddScoped<IFeriadosService, FeriadosService>();

            #endregion Credor Compromisso

            #region SaldoGarantias

            services.AddScoped<ISaldoGarantiaAppService, SaldoGarantiaAppService>();
            services.AddScoped<ISaldoGarantiaService, SaldoGarantiaService>();
            services.AddScoped<ISaldoGarantiaRepository, SaldoGarantiaRepository>();

            #endregion SaldoGarantias

            #region CriterioSaldoGarantia

            services.AddScoped<ICriterioSaldoGarantiaRepository, CriterioSaldoGarantiaRepository>();

            #endregion CriterioSaldoGarantia

            #region Estado

            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IEstadoAppService, EstadoAppService>();

            #endregion Estado

            services.AddScoped<ICargaCompromissoParcelaRepository, CargoCompromissoParcelaRepository>();

            #region Exportação Pre/Pos RJ

            services.AddScoped<IExportacaoPrePosRJAppService, ExportacaoPrePosRJAppService>();
            services.AddScoped<IExportacaoPrePosRJService, ExportacaoPrePosRJService>();
            services.AddScoped<IExportacaoPrePosRJRepository, ExportacaoPrePosRJRepository>();

            #endregion Exportação Pre/Pos RJ

            #region Log Decisao Objeto

            services.AddScoped<ILogDecisaoObjetoAppService, LogDecisaoObjetoAppService>();
            services.AddScoped<ILogDecisaoObjetoService, LogDecisaoObjetoService>();
            services.AddScoped<ILogDecisaoObjetoRepository, LogDecisaoObjetoRepository>();

            #endregion Log Decisao Objeto

            #region Log Composicao Pagamento

            services.AddScoped<ILogComposicaoPagamentoAppService, LogComposicaoPagamentoAppService>();
            services.AddScoped<ILogComposicaoPagamentoService, LogComposicaoPagamentoService>();
            services.AddScoped<ILogComposicaoPagamentoRepository, LogComposicaoPagamentoRepository>();

            #endregion Log Composicao Pagamento

            #region Fechamento Pex Media
            services.AddScoped<IFechamentoPexMediaAppService, FechamentoPexMediaAppService>();
            services.AddScoped<IFechamentoPexMediaService, FechamentoPexMediaService>();
            services.AddScoped<IFechamentoPexMediaRepository, FechamentoPexMediaRepository>();
            #endregion Fechamento Pex Media

            services.AddScoped<JwtSettings>();

            services.AddScoped<JuridicoContext>();

            //services.AddScoped<AlteracaoEmBlocoContext>();

            services.AddScoped<INasService, NasService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #region Profissional

            services.AddScoped<IProfissionalAppService, ProfissionalAppService>();
            services.AddScoped<IProfissionalService, ProfissionalService>();
            services.AddScoped<IProfissionalRepository, ProfissionalRepository>();

            #endregion Profissional

            #region BBComarca

            services.AddScoped<IBBComarcaAppService, BBComarcaAppService>();
            services.AddScoped<IBBComarcaService, BBComarcaService>();
            services.AddScoped<IBBComarcaRepository, BBComarcaRepository>();

            #endregion BBComarca

            #region BB Tribunais

            services.AddScoped<IBBTribunaisAppService, BBTribunaisAppService>();
            services.AddScoped<IBBTribunaisService, BBTribunaisService>();
            services.AddScoped<IBBTribunaisRepository, BBTribunaisRepository>();

            #endregion BB Tribunais

            #region BB Naturezas Acoes

            services.AddScoped<IBBNaturezasAcoesAppService, BBNaturezasAcoesAppService>();
            services.AddScoped<IBBNaturezasAcoesService, BBNaturezasAcoesService>();
            services.AddScoped<IBBNaturezasAcoesRepository, BBNaturezasAcoesRepository>();

            #endregion BB Naturezas Acoes

            #region BBModalidade

            services.AddScoped<IBBModalidadeAppService, BBModalidadeAppService>();
            services.AddScoped<IBBModalidadeService, BBModalidadeService>();
            services.AddScoped<IBBModalidadeRepository, BBModalidadeRepository>();

            #endregion BBModalidade

            #region BB Status Parcelas

            services.AddScoped<IBBStatusParcelasAppService, BBStatusParcelasAppService>();
            services.AddScoped<IBBStatusParcelasService, BBStatusParcelasService>();
            services.AddScoped<IBBStatusParcelasRepository, BBStatusParcelasRepository>();

            #endregion BB Status Parcelas

            #region BB Resumo Processamento

            services.AddScoped<IBBResumoProcessamentoAppService, BBResumoProcessamentoAppService>();
            services.AddScoped<IBBResumoProcessamentoService, BBResumoProcessamentoService>();
            services.AddScoped<IBBResumoProcessamentoRepository, BBResumoProcessamentoRepository>();

            #endregion BB Resumo Processamento

            #region BBOrgaos

            services.AddScoped<IBBOrgaosAppService, BBOrgaosAppService>();
            services.AddScoped<IBBOrgaosService, BBOrgaosService>();
            services.AddScoped<IBBOrgaosRepository, BBOrgaosRepository>();

            #endregion BBOrgaos

            #region Vara

            services.AddScoped<IVaraService, VaraService>();
            services.AddScoped<IVaraRepository, VaraRepository>();
            services.AddScoped<IVaraAppService, VaraAppService>();

            #endregion Vara

            #region TipoVara

            services.AddScoped<ITipoVaraService, TipoVaraService>();
            services.AddScoped<ITipoVaraRepository, TipoVaraRepository>();
            services.AddScoped<ITipoVaraAppService, TipoVaraAppService>();

            #endregion TipoVara

            #region Acao

            services.AddScoped<IAcaoService, AcaoService>();
            services.AddScoped<IAcaoRepository, AcaoRepository>();

            #endregion Acao

            #region LoteBB

            services.AddScoped<ILoteBBService, LoteBBService>();
            services.AddScoped<ILoteBBRepository, LoteBBRepository>();

            #endregion LoteBB

            #region BBStatusRemessa

            services.AddScoped<IBBStatusRemessaRepository, BBStatusRemessaRepository>();

            #endregion BBStatusRemessa

            #region Audiencia Processo

            services.AddScoped<IAudienciaProcessoAppService, AudienciaProcessoAppService>();
            services.AddScoped<IAudienciaProcessoService, AudienciaProcessoService>();
            services.AddScoped<IAudienciaProcessoRepository, AudienciaProcessoRepository>();
            services.AddScoped<IAudienciaProcessoAdoRepository, AudienciaProcessoAdoRepository>();

            #endregion Audiencia Processo

            #region Advogado Escritorio

            services.AddScoped<IAdvogadoEscritorioService, AdvogadoEscritorioService>();
            services.AddScoped<IAdvogadoEscritorioRepository, AdvogadoEscritorioRepository>();

            #endregion Advogado Escritorio

            #region Comarca

            services.AddScoped<IComarcaAppService, ComarcaAppService>();
            services.AddScoped<IComarcaService, ComarcaService>();
            services.AddScoped<IComarcaRepository, ComarcaRepository>();

            #endregion Comarca

            #region Juro_Correcao_Processo

            services.AddScoped<IJuroCorrecaoProcessoAppService, JuroCorrecaoProcessoAppService>();
            services.AddScoped<IJuroCorrecaoProcessoService, JuroCorrecaoProcessoService>();
            services.AddScoped<IJuroCorrecaoProcessoRepository, JuroCorrecaoProcessoRepository>();

            #endregion Juro_Correcao_Processo

            #region TipoAudiencia

            services.AddScoped<TipoAudienciaCommandHandler, TipoAudienciaCommandHandler>();
            services.AddScoped<ITipoAudienciaComposer, TipoAudienciaComposer>();
            services.AddScoped<ITipoAudienciaRepository, TipoAudienciaRepository>();
            services.AddScoped<ITipoAudienciaAdoRepository, TipoAudienciaAdoRepository>();

            #endregion TipoAudiencia

            #region BaseCalculo

            services.AddScoped<BaseCalculoCommandHandler, BaseCalculoCommandHandler>();
            services.AddScoped<IBaseCalculoComposer, BaseCalculoComposer>();
            services.AddScoped<IBaseCalculoRepository, BaseCalculoRepository>();
            services.AddScoped<IBaseCalculoAdoRepository, BaseCalculoAdoRepository>();

            #endregion BaseCalculo

            #region TipoParticipacao

            services.AddScoped<TipoParticipacaoCommandHandler, TipoParticipacaoCommandHandler>();
            services.AddScoped<ITipoParticipacaoComposer, TipoParticipacaoComposer>();
            services.AddScoped<ITipoParticipacaoRepository, TipoParticipacaoRepository>();
            services.AddScoped<ITipoParticipacaoAdoRepository, TipoParticipacaoAdoRepository>();

            #endregion TipoParticipacao

            #region PercentualAtm

            services.AddScoped<IPercentualATMRepository, PercentualATMRepository>();
            services.AddScoped<IPercentualAtmService, PercentualAtmService>();

            #endregion PercentualAtm

            #region ApuracaoOutliers

            services.AddScoped<IApuracaoOutliersAppService, ApuracaoOutliersAppService>();
            services.AddScoped<IBaseFechamentoJecCompletaService, BaseFechamentoJecCompletaService>();
            services.AddScoped<IBaseFechamentoJecCompletaRepository, BaseFechamentoJecCompletaRepository>();
            services.AddScoped<IAgendarApuracaoOutlierService, AgendarApuracaoOutlierService>();
            services.AddScoped<IAgendarApuracaoOutlierRepository, AgendarApuracaoOutlierRepository>();

            #endregion ApuracaoOutliers

            #region Janela
            services.AddScoped<IJanelaRepository, JanelaRepository>();
            #endregion Janela
        }
    }
}