using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Infra
{
    public interface IDatabaseContext
    {
        #region OPERATORS

        /// <summary>
        /// Begins tracking the given entity, and any other reachable entities that are not
        /// already being tracked, in the <see cref="EntityState.Added"/>
        /// state such that they will be inserted into the database when <see cref="IDatabaseContext.SaveChanges()"/>
        /// is called.
        /// <para>Use <see cref="EntityEntry.State"/> to set the state of only a single entity.</para>
        /// </summary>
        /// <typeparam name="IEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to add.</param>
        void Add(IEntity entity);

        /// <summary>
        /// Begins tracking the given entity in the <see cref="EntityState.Deleted"/> state such that it will be removed from the database when <see cref="IDatabaseContext.SaveChanges()"/> is called.
        /// </summary>
        /// <typeparam name="IEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to remove.</param>
        void Remove(IEntity entity);

        /// <inheritdoc cref="DbContext.SaveChanges()"/>
        int SaveChanges();        

        /// <inheritdoc cref="DbContext.SaveChangesAsync(CancellationToken)"/>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int GetNext(string schema, string sequenceName, IEntity entity);

        #endregion OPERATORS

        int ExecuteSqlInterpolated(string query);
       
        IQueryable<Orgao> Orgaos { get; }

        IQueryable<Usuario> Usuarios { get; }

        IQueryable<Permissao> Permissoes { get; }

        IQueryable<Lote> Lotes { get; }

        IQueryable<LoteLancamento> LotesLancamentos { get; }

        IQueryable<TipoDocumento> TiposDocumentos { get; }

        IQueryable<TipoAudiencia> TipoAudiencias { get; }

        IQueryable<Processo> Processos { get; }

        IQueryable<Lancamento> Lancamentos { get; }

        IQueryable<DocumentoLancamento> DocumentosLancamentos { get; }

        IQueryable<EmpresaDoGrupo> EmpresasDoGrupo { get; }

        IQueryable<Regional> Regionais { get; }

        IQueryable<EmpresaSap> EmpresasSap { get; }

        IQueryable<EmpresaCentralizadora> EmpresasCentralizadoras { get; }

        IQueryable<ValorProcesso> ValoresProcesso { get; }

        IQueryable<AlocacaoPreposto> AlocacoesPrepostos { get; }

        IQueryable<PagamentoProcesso> PagamentosProcesso { get; }

        IQueryable<PagamentoObjetoProcesso> PagamentosObjetosProcesso { get; }

        IQueryable<Parte> Partes { get; }

        IQueryable<ParteProcesso> PartesProcessos { get; }

        IQueryable<AdvogadoDoEscritorio> AdvogadosDosEscritorios { get; }

        IQueryable<Comarca> Comarcas { get; }

        IQueryable<FatoGerador> FatosGeradores { get; }

        IQueryable<CentroCusto> CentrosCustos { get; }

        IQueryable<ParametroJuridico> ParametrosJuridicos { get; }

        IQueryable<Fornecedor> Fornecedores { get; }

        IQueryable<FechamentoTrabalhista> FechamentosTrabalhistas { get; }

        IQueryable<FechamentoCivel> FechamentosCiveis { get; }

        IQueryable<FechamentoProcessoJuizado> FechamentosProcessosJuizados { get; }

        IQueryable<InterfaceBB> InterfacesBB { get; }

        IQueryable<Bordero> Borderos { get; }

        IQueryable<ProcessoConexo> ProcessosConexos { get; }

        IQueryable<Profissional> Profissionais { get; }

        IQueryable<Assunto> Assuntos { get; }

        IQueryable<Acao> Acoes { get; }

        IQueryable<NaturezaAcaoBB> NaturezasDasAcoesBB { get; }

        IQueryable<FechamentoCCMedia> FechamentosCiveisConsumidoresMedia { get; }
        IQueryable<EmpresaCentralizadoraAgendamentoFechAuto> EmpresaCentralizadoraAgendamentoFechAuto { get; }

        IQueryable<AndamentoDoProcesso> AndamentosDosProcessos { get; }

        IQueryable<Estabelecimento> Estabelecimentos { get; }

        IQueryable<Pedido> Pedidos { get; }

        IQueryable<AndamentoPedido> AndamentosPedidos { get; }

        IQueryable<Despesa> Despesas { get; }

        IQueryable<FielDepositario> FieisDepositarios { get; }
        IQueryable<EscritorioDoUsuario> EscritoriosDosUsuarios { get; }

        IQueryable<DespesaProfissional> DespesasDosProfissionais { get; }

        IQueryable<AdvogadoDoAutor> AdvogadosDosAutores { get; }

        IQueryable<AudienciaDoProcesso> AudienciasDosProcessos { get; }

        IQueryable<Protocolo> Protocolos { get; }

        IQueryable<EscritorioEstado> EscritoriosEstados { get; }

        IQueryable<AgendamentoCargaComprovante> AgendamentosCargasComprovantes { get; }

        IQueryable<AgendamentoCargaDocumento> AgendamentosCargasDocumentos { get; }

        IQueryable<DocumentoQuitacaoLancamento> DocumentosQuitacoesLancamentos { get; }

        IQueryable<Escritorio> Escritorios { get; }


        IQueryable<Preposto> Prepostos { get; }

        IQueryable<Feriado> Feriados { get; }

        IQueryable<EmpresaDoGrupo> EmpresasDoGrupoParaAgendaDeAudiencias { get; }

        IQueryable<PedidoProcesso> PedidosDoProcesso { get; }

        IQueryable<AgendamentoMigracaoPedidosSap> AgendamentosMigracaoPedidosSap { get; }

        IQueryable<FechamentoProvisaoTrabalhista> FechamentosProvisaoTrabalhista { get; }
        
        IQueryable<PercentualATM> PercentualATM { get; }
        
        IQueryable<BaseDeCalculo> BasesDeCalculo { get; }

        IQueryable<PartePedidoProcesso> PartesPedidosProcesso { get; }

        IQueryable<JurosCorrecaoProcesso> JurosCorrecoesProcessos { get; }
        
        IQueryable<TipoDeParticipacao> TiposDeParticipacoes { get; }

        IQueryable<Procedimento> Procedimentos { get; }
        
        IQueryable<TipoPrazo> TiposPrazos { get; }

        IQueryable<DocumentoProcesso> DocumentosProcessos { get; }

        IQueryable<AndamentoProcessoDocumento> AndamentosProcessosDocumentos { get; }

        IQueryable<TipoVara> TiposDeVara { get; }

        IQueryable<Vara> Varas { get; }

        IQueryable<ComplementoDeAreaEnvolvida> ComplementoDeAreasEnvolvidas { get; }

        IQueryable<ApuracaoProcesso> ApuracaoProcessos { get; }

        IQueryable<AtmIndiceUfPadrao> AtmIndicesUfPadrao { get; }

        IQueryable<Indice> Indices { get; }

        IQueryable<AgendamentoDeFechamentoAtmUfPex> AgendamentoDeFechamentoAtmUfPex { get; }

        IQueryable<FechamentoAtmPex> FechamentosAtmPex { get; }

        IQueryable<AgendamentoDeFechamentoAtmPex> AgendamentosDeFechamentosAtmPex { get; }

        IQueryable<Cotacao> Cotacoes { get; }

        IQueryable<FechamentoAtmJec> FechamentosAtmJec { get; }

        IQueryable<AgendamentoDeFechamentoAtmJec> AgendamentosDeFechamentosAtmJec { get; }
        
        IQueryable<TipoPendencia> TipoPendencias { get; }

        IQueryable<PendenciaProcesso> PendenciasProcessos { get; }

        IQueryable<PrazoProcesso> PrazosDoProcesso { get; }

        IQueryable<TipoOrientacaoJuridica> TiposOrientacoesJuridicas { get; }

        IQueryable<OrientacaoJuridica> OrientacoesJuridicas { get; }

        IQueryable<ComarcaBB> ComarcaBBs { get; }

        IQueryable<OrgaoBB> OrgaoBBs { get; }
        
        IQueryable<TribunalBB> TribunalBBs { get; }

        IQueryable<GrupoJuizadoVara> GrupoJuizadoVaras { get; }
        
        IQueryable<ProcessoCpfCnpj> ProcessoCpfCnpjs { get; }
        
        IQueryable<Estado> Estados { get; }

        IQueryable<Municipio> Municipios { get; }

        IQueryable<LinhaProcesso> LinhaProcessos { get; }
           
        IQueryable<GrupoPedido> GruposPedidos { get; }

        IQueryable<ValorObjetoProcesso> ValorObjetoProcessos { get; }
        
        IQueryable<DecisaoObjetoProcesso> DecisaoObjetoProcessos { get; }

        IQueryable<Esfera> Esferas { get; }

        IQueryable<IndiceCorrecaoEsfera> IndiceCorrecaoEsferas { get; }

        IQueryable<CotacaoIndiceTrabalhista> CotacaoIndiceTrabalhistas { get; }

        IQueryable<TempCotacaoIndiceTrab> TempCotacaoIndiceTrabalhistas { get; }

        IQueryable<MigracaoAssunto> MigracaoAssunto { get; }

        IQueryable<MigracaoAssuntoEstrategico> MigracaoAssuntoEstrategico { get; }

        IQueryable<MigracaoAcao> MigracaoAcao { get; }
        IQueryable<MigracaoAcaoConsumidor> MigracaoAcaoConsumidor { get; }
        IQueryable<MigracaoPedido> MigracaoPedido { get; }
        IQueryable<MigracaoPedidoConsumidor> MigracaoPedidoConsumidor { get; }
        IQueryable<TipoDocumentoMigracao> TipoDocumentoMigracao { get; }
        IQueryable<TipoDocumentoMigracaoEstrategico> TipoDocumentoMigracaoEstrategico { get; }
        IQueryable<MigracaoTipoPrazoEstrategico> MigracaoTipoPrazoEstrategico { get; }
        IQueryable<MigracaoTipoPrazoConsumidor> MigracaoTipoPrazoConsumidor { get; }
        IQueryable<MigracaoTipoAudienciaEstrategico> MigracaoTipoAudienciaEstrategico { get; }
        IQueryable<MigracaoTipoAudienciaConsumidor> MigracaoTipoAudienciaConsumidor { get; }

        IQueryable<UsuarioOperacaoRetroativa> UsuarioOperacaoRetroativas { get; }
        IQueryable<Evento> Eventos { get; }
        IQueryable<EventoDependente> EventosDependentes { get; }
        IQueryable<DecisaoEvento> DecisaoEventos { get; }

        IQueryable<FatoGeradorProcesso> FatoGeradorProcessos { get; }

        IQueryable<FaseProcesso> FaseProcessos { get; }
        IQueryable<MigracaoEventoCivelConsumidor> MigracaoEventosCivelConsumidor { get; }
        IQueryable<MigracaoEventoCivelEstrategico> MigracaoEventosCivelEstrategico { get; }
        IQueryable<ColunasRelatorioEficienciaEvento> ColunasRelatorioEficienciaEventos { get; }

        IQueryable<ProcessoInconsistente> ProcessoInconsistentes { get; }
        IQueryable<ProcessoTributarioInconsistente> ProcessoTributarioInconsistentes { get; }
       
        IQueryable<Janela> Janelas { get; }

        IQueryable<IndiceCorrecaoProcesso> IndiceVigencias { get; }

        IQueryable<MotivoProvavelZero> MotivoProvavelZeros { get; }

    }
}