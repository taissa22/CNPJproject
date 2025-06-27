using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations;
using Perlink.Oi.Juridico.Application.Manutencao.Services;
using Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Providers.Implementations;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ExtensionMethods
    {
        public static IServiceCollection AddManutencao(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            services.AddDatabaseContext(optionsAction);

            services.AddScoped<IOrgaoRepository, OrgaoRepository>();
            services.AddScoped<IEmpresaDoGrupoRepository, EmpresaDoGrupoRepository>();
            services.AddScoped<ICentroDeCustoRepository, CentroDeCustoRepository>();
            services.AddScoped<IEmpresaSapRepository, EmpresaSapRepository>();
            services.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEmpresaCentralizadoraRepository, EmpresaCentralizadoraRepository>();
            services.AddScoped<IInterfaceBBRepository, InterfaceBBRepository>();
            services.AddScoped<IRegionaisRepository, RegionaisRepository>();
            services.AddScoped<IParteRepository, ParteRepository>();
            services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
            services.AddScoped<IAssuntoRepository, AssuntoRepository>();
            services.AddScoped<IAcaoRepository, AcaoRepository>();
            services.AddScoped<INaturezaAcaoBBRepository, NaturezaAcaoBBRepository>();
            services.AddScoped<IFielDepositarioRepository, FielDepositarioRepository>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IBaseDeCalculoRepository, BaseDeCalculoRepository>();
            services.AddScoped<ITipoAudienciaRepository, TipoAudienciaRepository>();
            services.AddScoped<IJurosVigenciasCiveisRepository, JurosVigenciasCiveisRepository>();
            services.AddScoped<ITipoDeParticipacaoRepository, TipoDeParticipacaoRepository>();
            services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
            services.AddScoped<ICotacaoRepository, CotacaoRepository>();
            services.AddScoped<IIndiceRepository, IndiceRepository>();
            services.AddScoped<IIndicesVigenciasRepository, IndicesVigenciasRepository>();
            services.AddScoped<ITipoPendenciaRepository, TipoPendenciaRepository>();
            services.AddScoped<ITipoPrazoRepository, TipoPrazoRepository>();
            services.AddScoped<ITipoProcedimentoRepository, TipoProcedimentoRepository>();
            services.AddScoped<ITipoDocumentoRepository, TipoDocumentoRepository>();
            services.AddScoped<ITipoOrientacaoJuridicaRepository, TipoOrientacaoJuridicaRepository>();
            services.AddScoped<ITipoDeParticipacaoRepository, TipoDeParticipacaoRepository>();
            services.AddScoped<ITipoVaraRepository, TipoVaraRepository>();
            services.AddScoped<IComplementoDeAreaEnvolvidaRepository, ComplementoDeAreaEnvolvidaRepository>();
            services.AddScoped<IComarcaRepository, ComarcaRepository>();
            services.AddScoped<IVaraRepository,VaraRepository>();
            services.AddScoped<IComarcaBBRepository,ComarcaBBRepository>();
            services.AddScoped<ITribunalBBRepository,TribunalBBRepository>();
            services.AddScoped<IOrgaoBBRepository, OrgaoBBRepository>();
            services.AddScoped<IEstadoRepository, EstadoRepository>();
            services.AddScoped<IMunicipioRepository, MunicipioRepository>();
            services.AddScoped<IObjetoRepository, ObjetoRepository>();
            services.AddScoped<IGrupoPedidoRepository, GrupoPedidoRepository>();
            services.AddScoped<IEsferaRepository, EsferaRepository>();
            services.AddScoped<IIndiceCorrecaoEsferaRepository, IndiceCorrecaoEsferaRepository>();
            services.AddScoped<ICotacaoIndiceTrabalhistaRepository, CotacaoIndiceTrabalhistaRepository>();
            services.AddScoped<IOrientacaoJuridicaRepository, OrientacaoJuridicaRepository>();
            services.AddScoped<IPrepostosRepository, PrepostosRepository>();
            services.AddScoped<IMotivoProvavelZeroRepository, MotivoProvavelZeroRepository>();


            services.AddScoped<IUsuarioOperacaoRetroativaRepository, UsuarioOperacaoRetroativaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IDecisaoEventoRepository, DecisaoEventoRepository>();
            services.AddScoped<IFatoGeradorRepository, FatoGeradorRepository>();
            services.AddScoped<IProcessoTributarioInconsistenteRepository, ProcessoTributarioInconsistenteRepository>();
            services.AddScoped<IEscritorioRepository, EscritorioRepository>();
            services.AddScoped<IEscritorioEstadoRepository, EscritorioEstadoRepository>();
            services.AddScoped<IAdvogadoDoEscritorioRepository, AdvogadodoEscritorioRepository>();
            

            services.AddScoped<IOrgaoService, OrgaoService>();
            services.AddScoped<IComplementoDeAreaEnvolvidaService, ComplementoDeAreaEnvolvidaService>();
            services.AddScoped<IAcaoService, AcaoService>();
            services.AddScoped<IEmpresaDoGrupoService, EmpresaDoGrupoService>();
            services.AddScoped<IEmpresaCentralizadoraService, EmpresaCentralizadoraService>();
            services.AddScoped<IEstabelecimentoService, EstabelecimentoService>();
            services.AddScoped<IParteService, ParteService>();
            services.AddScoped<IProfissionalService, ProfissionalService>();
            services.AddScoped<IAssuntoService, AssuntoService>();
            services.AddScoped<IFielDepositarioService, FielDepositarioService>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IBaseDeCalculoService, BaseDeCalculoService>();
            services.AddScoped<ITipoAudienciaService, TipoAudienciaService>();
            services.AddScoped<IJurosVigenciasCiveisService, JurosVigenciasCiveisService>();
            services.AddScoped<ITipoDeParticipacaoService, TipoDeParticipacaoService>();
            services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
            services.AddScoped<ICotacaoService, CotacaoService>();
            services.AddScoped<IIndiceService, IndiceService>();
            services.AddScoped<ITipoPendenciaService, TipoPendenciaService>();
            services.AddScoped<ITipoPrazoService, TipoPrazoService>();
            services.AddScoped<ITipoProcedimentoService, TipoProcedimentoService>();
            services.AddScoped<ITipoDocumentoService, TipoDocumentoService>();
            services.AddScoped<ITipoOrientecaoJuridicaService, TipoOrientacaoJuridicaService>();
            services.AddScoped<ITipoDeParticipacaoService, TipoDeParticipacaoService>();
            services.AddScoped<ITipoVaraService, TipoVaraService>();
            services.AddScoped<ICotacaoService, CotacaoService>();
            services.AddScoped<IIndiceService, IndiceService>();
            services.AddScoped<IIndicesVigenciasService, IndicesVigenciasService>();
            services.AddScoped<IComarcaService, ComarcaService>();
            services.AddScoped<IVaraService, VaraService>();
            services.AddScoped<IEstadoService, EstadoService>();
            services.AddScoped<IMunicipioService, MunicipioService>();
            services.AddScoped<IIndiceCorrecaoEsferaService, IndiceCorrecaoEsferaService>();
            services.AddScoped<IEsferaService, EsferaService>();
            services.AddScoped<ICotacaoIndiceTrabalhistaService, CotacaoIndiceTrabalhistaService>();

            services.AddScoped<IAgendamentoDeFechamentoAtmUfPexRepository, AgendamentoDeFechamentoAtmUfPexRepository>();
            services.AddScoped<IAgendamentoDeFechamentoAtmPexRepository, AgendamentoDeFechamentoAtmPexRepository>();
            services.AddScoped<IParametroJuridicoProvider, ParametroJuridicoProvider>();

            services.AddScoped<IFechamentoCCMediaRepository, FechamentoCCMediaRepository>();
            services.AddScoped<IObjetoService, ObjetoService>();
            services.AddScoped<IGrupoPedidoService, GrupoPedidoService>();
            services.AddScoped<IMigracaoAssuntoConsumidorService, MigracaoAssuntoConsumidorService>();
            services.AddScoped<IMigracaoAcaoService, MigracaoAcaoService>();
            services.AddScoped<IMigracaoAssuntoEstrategicoService, MigracaoAssuntoEstrategicoService>();
            services.AddScoped<IMigracaoAcaoConsumidorService, MigracaoAcaoConsumidorService>();
            services.AddScoped<ITipoDocumentoMigracaoService, TipoDocumentoMigracaoService>();            
            services.AddScoped<IOrientacaoJuridicaService, OrientacaoJuridicaService>();
            services.AddScoped<IEventoService, EventoService>();
            services.AddScoped<IDecisaoEventoService, DecisaoEventoService>();
            services.AddScoped<IPrepostosService, PrepostosService>();
            services.AddScoped<IFatoGeradorService, FatoGeradorService>();
            services.AddScoped<IProcessoTributarioInconsistenteService, ProcessoTributarioInconsistenteService>();
            services.AddScoped<IUsuarioOperacaoRetroativaService, UsuarioOperacaoRetroativaService>();

            services.AddScoped<IMotivoProvavelZeroService,  MotivoProvavelZeroService>();
            services.AddScoped<IEscritorioService,  EscritorioService>();
            services.AddScoped<IAdvogadoDoEscritorioService,  AdvogadoDoEscritorioService>();


            return services;
        }
    }
}