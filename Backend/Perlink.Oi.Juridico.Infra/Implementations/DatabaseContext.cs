using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Infra.Implementations
{
    internal class DatabaseContext : DbContext, IDatabaseContext
    {
        private Lazy<IUsuarioAtualProvider> UsuarioProvider { get; }

        private string CurrentUser => UsuarioProvider.Value.Login;

        public ILogger<IDatabaseContext> Logger { get; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, Lazy<IUsuarioAtualProvider> usuarioProvider, ILogger<IDatabaseContext> logger) : base(options)
        {
            Logger = logger;
            UsuarioProvider = usuarioProvider;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Notification>();
            builder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        }

        #region OPERATORS

        /// <inheritdoc/>
        void IDatabaseContext.Add(IEntity entity)
        {
            Type entityType = entity.GetType();

            Logger.LogInformation($"Adicionando entidade { entityType.Name } para inserção");
            Add<IEntity>(entity);

            Logger.LogInformation($"Buscando entidade base");
            Type entityBaseType = GetEntityBaseType(entityType);

            if (entityBaseType is null)
            {
                Logger.LogInformation($"Sem entidade base, adicionada para inserção");
                return;
            }

            Logger.LogInformation($"Buscando os relacionamentos da entidade base");
            Type[] entityBaseRelationTypes = GetEntityBaseRelationsTypes(entityBaseType);

            Logger.LogInformation($"Instanciando os relacionamentos da entidade base");
            IEnumerable<IEntity> entityBaseRelations = GetRelationsForEntityBase(entity, entityBaseRelationTypes);

            Logger.LogInformation($"Instanciando e populando entidade base");
            IEntity newEntityBase = GetNewEntityBasePopulated(entity, entityBaseType, entityBaseRelations);

            Logger.LogInformation($"Adicionando entidade base para inserção");
            Add(newEntityBase);
            Logger.LogInformation($"Entidade { entityType.Name } adicionada para inserção");

            static IEnumerable<IEntity> GetRelationsForEntityBase(IEntity entity, Type[] relationTypes)
            {
                var name = entity.GetType().Name;
                var properties = entity.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                var relations = new List<IEntity>(relationTypes.Length);

                foreach (var entityType in relationTypes.Where(x => x.Name != entity.GetType().Name))
                {
                    var newEntityConstructor = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();

                    var newEntityProperties = entityType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).ToArray();

                    var newEntity = newEntityConstructor.Invoke(null);

                    foreach (var property in properties)
                    {
                        if (newEntityProperties.Any(x => x.Name == property.Name))
                        {
                            newEntityProperties.First(x => x.Name == property.Name).SetValue(newEntity, property.GetValue(entity));
                        }
                    }

                    relations.Add((IEntity)newEntity);
                }

                relations.Add(entity);

                return relations;
            }

            static IEntity GetNewEntityBasePopulated(IEntity entity, Type entityBaseType, IEnumerable<IEntity> entityBaseRelations)
            {
                PropertyInfo[] entityProperties = entity.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Where(x => x.CanWrite).ToArray();

                var entityBaseConstructor = entityBaseType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();

                var entityBaseProperties = entityBaseType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

                var newEntityBase = entityBaseConstructor.Invoke(null);

                foreach (var property in entityProperties.Where(x => entityBaseProperties.Any(z => z.Name == x.Name)))
                {
                    entityBaseProperties.First(x => x.Name == property.Name).SetValue(newEntityBase, property.GetValue(entity));
                }

                foreach (var relation in entityBaseRelations)
                {
                    entityBaseProperties.First(x => x.Name == relation.GetType().Name).SetValue(newEntityBase, relation);
                }

                return (IEntity)newEntityBase;
            }
        }

        /// <inheritdoc/>
        void IDatabaseContext.Remove(IEntity entity)
        {
            Type entityType = entity.GetType();

            Logger.LogInformation($"Buscando entidade base");
            Type entityBaseType = GetEntityBaseType(entityType);

            if (entityBaseType is null)
            {
                Remove(entity);
                Logger.LogInformation($"Sem entidade base, adicionada para remoção");
                return;
            }

            Logger.LogInformation($"Carregando entidade base");
            var entityBaseReference = Entry(entity).Reference<IEntity>(entityBaseType.Name);
            entityBaseReference.Query().IgnoreQueryFilters().Load();

            Logger.LogInformation($"Buscando os relacionamentos da entidade base");
            Type[] entityBaseRelationTypes = GetEntityBaseRelationsTypes(entityBaseType);

            Logger.LogInformation($"Foram encontrados { entityBaseRelationTypes.Length } relacionamentos");
            Logger.LogInformation($"Carregando relacionamentos da entidade base");
            foreach (var relationType in entityBaseRelationTypes)
            {
                Logger.LogInformation($"Carregando { relationType.Name }");
                var relationReference = Entry(entityBaseReference.CurrentValue).Reference<IEntity>(relationType.Name);
                relationReference.Query().IgnoreQueryFilters().Load();
                Remove(relationReference.CurrentValue);
                Logger.LogInformation($"Relacionamento { relationType.Name } adicionado para remoção");
            }
            Logger.LogInformation($"Relacionamentos da entidade base adicionados para remoção");

            Remove(entityBaseReference.CurrentValue);
            Logger.LogInformation($"Entidade { entityType.Name } adicionada para remoção");
        }

        /// <inheritdoc />
        public override int SaveChanges()
        {
            try
            {
                var saveResult = 0;
                Logger.LogInformation($"Buscando usuário logado.");
                if (string.IsNullOrEmpty(CurrentUser))
                {
                    throw new AuthenticationException("Usuário não autenticado, não foi possível efetuar a chamada da procedure SP_ACA_INSERE_LOG_USUARIO");
                }

                using (var scope = Database.BeginTransaction())
                {
                    Logger.LogInformation($"Definindo log no banco para usuário: { CurrentUser }.");
                    var logResult = Database.ExecuteSqlCommand(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'N')", CurrentUser));
                    Logger.LogInformation($"SP_ACA_INSERE_LOG_USUARIO retornou: { logResult }.");
                    //Task.Delay(1_000).GetAwaiter().GetResult();
                    saveResult = base.SaveChanges();
                    scope.Commit();
                }

                Logger.LogInformation($"SaveChanges retornou: { saveResult }.");
                return saveResult;
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(ex, $"SaveChanges não completo. DbUpdateException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"SaveChanges não completo. Exception: {ex.Message}");
                throw;
            }
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var saveResult = 0;
                Logger.LogInformation($"Buscando usuário logado.");
                if (string.IsNullOrEmpty(CurrentUser))
                {
                    throw new AuthenticationException("Usuário não autenticado, não foi possível efetuar a chamada da procedure SP_ACA_INSERE_LOG_USUARIO");
                }

                using (var scope = await Database.BeginTransactionAsync())
                {
                    Logger.LogInformation($"Definindo log no banco para usuário: { CurrentUser }.");
                    var logResult = await Database.ExecuteSqlCommandAsync(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'N')", CurrentUser));
                    Logger.LogInformation($"SP_ACA_INSERE_LOG_USUARIO retornou: { logResult }.");
                    saveResult = await base.SaveChangesAsync(true, cancellationToken);
                    scope.Commit();
                }

                Logger.LogInformation($"SaveChangesAsync retornou: { saveResult }.");
                return saveResult;
            }
            catch (DbUpdateException ex)
            {
                Logger.LogError(ex, $"SaveChangesAsync não completo. DbUpdateException: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"SaveChangesAsync não completo. Exception: {ex.Message}");
                throw;
            }
        }


        public int ExecuteSqlInterpolated(string query )
        {
            using (var scope = Database.BeginTransaction())
            {
                Database.SetCommandTimeout(0);
                var retorno = Database.ExecuteSqlCommand(query, CurrentUser);
                scope.Commit();
                return retorno;
            }
        }   

        #endregion OPERATORS

        #region REFLECTS

        private static Type GetEntityBaseType(Type type)
        {
            return type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                            .Where(x => typeof(IEntity).IsAssignableFrom(x.PropertyType)).Select(x => x.PropertyType)
                            .SingleOrDefault(x => x.GetProperty(type.Name, BindingFlags.NonPublic | BindingFlags.Instance) != null);
        }

        private static Type[] GetEntityBaseRelationsTypes(Type entityBaseType)
        {
            return entityBaseType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                                .Where(x => typeof(IEntity).IsAssignableFrom(x.PropertyType))
                                                .Select(x => x.PropertyType).ToArray();
        }

        public int GetNext(string schema, string sequenceName, IEntity entity)
        {
            using var command = Entry(entity).Context.Database.GetDbConnection().CreateCommand();
            command.CommandText = $"SELECT {schema}.{sequenceName}.NEXTVAL FROM DUAL";
            Entry(entity).Context.Database.OpenConnection();
            using var reader = command.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0);
        }

        #endregion REFLECTS

        #region IQUERYABLES

        public DbSet<Orgao> Orgaos { get; set; }
        IQueryable<Orgao> IDatabaseContext.Orgaos => Orgaos.WithIncludes();

        public DbSet<Usuario> Usuarios { get; set; }
        IQueryable<Usuario> IDatabaseContext.Usuarios => Usuarios.WithIncludes();

        public DbSet<Parte> Partes { get; set; }
        IQueryable<Parte> IDatabaseContext.Partes => Partes.WithIncludes();

        public DbSet<Permissao> Permissoes { get; set; }
        IQueryable<Permissao> IDatabaseContext.Permissoes => Permissoes;

        public DbSet<Janela> Janelas{ get; set; }
        IQueryable<Janela> IDatabaseContext.Janelas => Janelas;

        public DbSet<Lote> Lotes { get; set; }
        IQueryable<Lote> IDatabaseContext.Lotes => Lotes;

        public DbSet<LoteLancamento> LotesLancamentos { get; set; }
        IQueryable<LoteLancamento> IDatabaseContext.LotesLancamentos => LotesLancamentos.WithIncludes();

        public DbSet<TipoDocumento> TiposDocumentos { get; set; }
        IQueryable<TipoDocumento> IDatabaseContext.TiposDocumentos => TiposDocumentos.WithIncludes();

        public DbSet<Processo> Processos { get; set; }
        IQueryable<Processo> IDatabaseContext.Processos => Processos.WithIncludes();

        public DbSet<Lancamento> Lancamentos { get; set; }
        IQueryable<Lancamento> IDatabaseContext.Lancamentos => Lancamentos.WithIncludes();

        public DbSet<DocumentoLancamento> DocumentosLancamentos { get; set; }
        IQueryable<DocumentoLancamento> IDatabaseContext.DocumentosLancamentos => DocumentosLancamentos.WithIncludes();

        public DbSet<EmpresaDoGrupo> EmpresasDoGrupo { get; set; }
        IQueryable<EmpresaDoGrupo> IDatabaseContext.EmpresasDoGrupo => EmpresasDoGrupo.WithIncludes();

        public DbSet<Regional> Regionais { get; set; }
        IQueryable<Regional> IDatabaseContext.Regionais => Regionais;

        public DbSet<EmpresaSap> EmpresasSap { get; set; }
        IQueryable<EmpresaSap> IDatabaseContext.EmpresasSap => EmpresasSap;

        public DbSet<EmpresaCentralizadora> EmpresasCentralizadoras { get; set; }
        IQueryable<EmpresaCentralizadora> IDatabaseContext.EmpresasCentralizadoras => EmpresasCentralizadoras.WithIncludes();

        public DbSet<ValorProcesso> ValoresProcesso { get; set; }
        IQueryable<ValorProcesso> IDatabaseContext.ValoresProcesso => ValoresProcesso;

        public DbSet<PagamentoProcesso> PagamentosProcesso { get; set; }
        IQueryable<PagamentoProcesso> IDatabaseContext.PagamentosProcesso => PagamentosProcesso;

        public DbSet<PagamentoObjetoProcesso> PagamentosObjetosProcesso { get; set; }
        IQueryable<PagamentoObjetoProcesso> IDatabaseContext.PagamentosObjetosProcesso => PagamentosObjetosProcesso;

        public DbSet<AdvogadoDoEscritorio> AdvogadosDosEscritorios { get; set; }
        IQueryable<AdvogadoDoEscritorio> IDatabaseContext.AdvogadosDosEscritorios => AdvogadosDosEscritorios.WithIncludes();

        ////public DbSet<Comarca> Comarcas { get; set; }
        ////IQueryable<Comarca> IDatabaseContext.Comarcas => Comarcas.WithIncludes();

        public DbSet<FatoGerador> FatosGeradores { get; set; }
        IQueryable<FatoGerador> IDatabaseContext.FatosGeradores => FatosGeradores.WithIncludes();

        public DbSet<CentroCusto> CentrosCustos { get; set; }
        IQueryable<CentroCusto> IDatabaseContext.CentrosCustos => CentrosCustos;

        public DbSet<Fornecedor> Fornecedores { get; set; }
        IQueryable<Fornecedor> IDatabaseContext.Fornecedores => Fornecedores;

        public DbSet<ParametroJuridico> ParametrosJuridicos { get; set; }
        IQueryable<ParametroJuridico> IDatabaseContext.ParametrosJuridicos => ParametrosJuridicos;

        public DbSet<FechamentoTrabalhista> FechamentosTrabalhistas { get; set; }
        IQueryable<FechamentoTrabalhista> IDatabaseContext.FechamentosTrabalhistas => FechamentosTrabalhistas;

        public DbSet<FechamentoCivel> FechamentosCiveis { get; set; }
        IQueryable<FechamentoCivel> IDatabaseContext.FechamentosCiveis => FechamentosCiveis;

        public DbSet<FechamentoProcessoJuizado> FechamentosProcessosJuizados { get; set; }
        IQueryable<FechamentoProcessoJuizado> IDatabaseContext.FechamentosProcessosJuizados => FechamentosProcessosJuizados;

        public DbSet<InterfaceBB> InterfacesBB { get; set; }
        IQueryable<InterfaceBB> IDatabaseContext.InterfacesBB => InterfacesBB;

        public DbSet<Bordero> Borderos { get; set; }
        IQueryable<Bordero> IDatabaseContext.Borderos => Borderos;

        public DbSet<ProcessoConexo> ProcessosConexos { get; set; }
        IQueryable<ProcessoConexo> IDatabaseContext.ProcessosConexos => ProcessosConexos;

        public DbSet<Profissional> Profissionais { get; set; }
        IQueryable<Profissional> IDatabaseContext.Profissionais => Profissionais;

        public DbSet<EmpresaCentralizadoraAgendamentoFechAuto> EmpresaCentralizadoraAgendamentoFechAutos { get; set; }
        IQueryable<EmpresaCentralizadoraAgendamentoFechAuto> IDatabaseContext.EmpresaCentralizadoraAgendamentoFechAuto => EmpresaCentralizadoraAgendamentoFechAutos;

        public DbSet<Assunto> Assuntos { get; set; }
        IQueryable<Assunto> IDatabaseContext.Assuntos => Assuntos.WithIncludes();        

        public DbSet<Acao> Acoes { get; set; }
        IQueryable<Acao> IDatabaseContext.Acoes => Acoes;

        public DbSet<NaturezaAcaoBB> NaturezasDasAcoesBB { get; set; }
        IQueryable<NaturezaAcaoBB> IDatabaseContext.NaturezasDasAcoesBB => NaturezasDasAcoesBB;

        public DbSet<FechamentoCCMedia> FechamentosCiveisConsumidoresMedia { get; set; }
        IQueryable<FechamentoCCMedia> IDatabaseContext.FechamentosCiveisConsumidoresMedia => FechamentosCiveisConsumidoresMedia;

        public DbSet<AndamentoDoProcesso> AndamentosDosProcessos { get; set; }
        IQueryable<AndamentoDoProcesso> IDatabaseContext.AndamentosDosProcessos => AndamentosDosProcessos;

        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        IQueryable<Estabelecimento> IDatabaseContext.Estabelecimentos => Estabelecimentos;

        public DbSet<ParteProcesso> PartesProcessos { get; set; }
        IQueryable<ParteProcesso> IDatabaseContext.PartesProcessos => PartesProcessos.WithIncludes();

        public DbSet<Pedido> Pedidos { get; set; }
        IQueryable<Pedido> IDatabaseContext.Pedidos => Pedidos.WithIncludes();

        public DbSet<AndamentoPedido> AndamentosPedidos { get; set; }
        IQueryable<AndamentoPedido> IDatabaseContext.AndamentosPedidos => AndamentosPedidos;

        public DbSet<Despesa> Despesas { get; set; }
        IQueryable<Despesa> IDatabaseContext.Despesas => Despesas;

        public DbSet<FielDepositario> FieisDepositarios { get; set; }
        IQueryable<FielDepositario> IDatabaseContext.FieisDepositarios => FieisDepositarios;

        public DbSet<EscritorioDoUsuario> EscritoriosDosUsuarios { get; set; }
        IQueryable<EscritorioDoUsuario> IDatabaseContext.EscritoriosDosUsuarios => EscritoriosDosUsuarios.WithIncludes();

        public DbSet<DespesaProfissional> DespesasDosProfissionais { get; set; }
        IQueryable<DespesaProfissional> IDatabaseContext.DespesasDosProfissionais => DespesasDosProfissionais;

        public DbSet<AdvogadoDoAutor> AdvogadosDosAutores { get; set; }
        IQueryable<AdvogadoDoAutor> IDatabaseContext.AdvogadosDosAutores => AdvogadosDosAutores;

        public DbSet<TipoAudiencia> TipoAudiencias { get; set; }
        IQueryable<TipoAudiencia> IDatabaseContext.TipoAudiencias => TipoAudiencias;

        public DbSet<AudienciaDoProcesso> AudienciasDosProcessos { get; set; }
        IQueryable<AudienciaDoProcesso> IDatabaseContext.AudienciasDosProcessos => AudienciasDosProcessos.WithIncludes();

        public DbSet<Protocolo> Protocolos { get; set; }
        IQueryable<Protocolo> IDatabaseContext.Protocolos => Protocolos;

        public DbSet<EscritorioEstado> EscritoriosEstados { get; set; }
        IQueryable<EscritorioEstado> IDatabaseContext.EscritoriosEstados => EscritoriosEstados;

        public DbSet<AlocacaoPreposto> AlocacoesPrepostos { get; set; }
        IQueryable<AlocacaoPreposto> IDatabaseContext.AlocacoesPrepostos => AlocacoesPrepostos;

        public DbSet<AgendamentoCargaComprovante> AgendamentosCargasComprovantes { get; set; }
        IQueryable<AgendamentoCargaComprovante> IDatabaseContext.AgendamentosCargasComprovantes => AgendamentosCargasComprovantes;

        public DbSet<AgendamentoCargaDocumento> AgendamentosCargasDocumentos { get; set; }
        IQueryable<AgendamentoCargaDocumento> IDatabaseContext.AgendamentosCargasDocumentos => AgendamentosCargasDocumentos;

        public DbSet<DocumentoQuitacaoLancamento> DocumentosQuitacoesLancamentos { get; set; }
        IQueryable<DocumentoQuitacaoLancamento> IDatabaseContext.DocumentosQuitacoesLancamentos => DocumentosQuitacoesLancamentos.WithIncludes();

        public DbSet<Escritorio> Escritorios { get; set; }
        IQueryable<Escritorio> IDatabaseContext.Escritorios => Escritorios;

        public DbSet<Preposto> Prepostos { get; set; }
        IQueryable<Preposto> IDatabaseContext.Prepostos => Prepostos;

        public DbSet<Feriado> Feriados { get; set; }
        IQueryable<Feriado> IDatabaseContext.Feriados => Feriados;

        public DbSet<EmpresaDoGrupo> EmpresasDoGrupoParaAgendaDeAudiencias { get; set; }
        IQueryable<EmpresaDoGrupo> IDatabaseContext.EmpresasDoGrupoParaAgendaDeAudiencias => EmpresasDoGrupoParaAgendaDeAudiencias;

        public DbSet<PedidoProcesso> PedidosDoProcesso { get; set; }
        IQueryable<PedidoProcesso> IDatabaseContext.PedidosDoProcesso => PedidosDoProcesso.WithIncludes();

        public DbSet<PercentualATM> PercentualATM { get; set; }
        IQueryable<PercentualATM> IDatabaseContext.PercentualATM => PercentualATM;

        public DbSet<AgendamentoMigracaoPedidosSap> AgendamentosMigracaoPedidosSap { get; set; }
        IQueryable<AgendamentoMigracaoPedidosSap> IDatabaseContext.AgendamentosMigracaoPedidosSap => AgendamentosMigracaoPedidosSap;

        public DbSet<TipoDeParticipacao> TiposDeParticipacoes { get; set; }
        IQueryable<TipoDeParticipacao> IDatabaseContext.TiposDeParticipacoes => TiposDeParticipacoes;

        public DbSet<Procedimento> Procedimentos { get; set; }
        IQueryable<Procedimento> IDatabaseContext.Procedimentos => Procedimentos.WithIncludes();

        public DbSet<FechamentoProvisaoTrabalhista> FechamentosProvisaoTrabalhista { get; set; }
        IQueryable<FechamentoProvisaoTrabalhista> IDatabaseContext.FechamentosProvisaoTrabalhista => FechamentosProvisaoTrabalhista.WithIncludes();

        public DbSet<BaseDeCalculo> BasesDeCalculo { get; set; }
        IQueryable<BaseDeCalculo> IDatabaseContext.BasesDeCalculo => BasesDeCalculo;

        public DbSet<JurosCorrecaoProcesso> JurosCorreacoesProcessos { get; set; }
        IQueryable<JurosCorrecaoProcesso> IDatabaseContext.JurosCorrecoesProcessos => JurosCorreacoesProcessos;

        public DbSet<TipoPendencia> TipoPendencias { get; set; }
        IQueryable<TipoPendencia> IDatabaseContext.TipoPendencias => TipoPendencias;

        public DbSet<PendenciaProcesso> PendenciasProcessos { get; set; }
        IQueryable<PendenciaProcesso> IDatabaseContext.PendenciasProcessos => PendenciasProcessos.WithIncludes();

        public DbSet<PrazoProcesso> PrazosDoProcesso { get; set; }
        IQueryable<PrazoProcesso> IDatabaseContext.PrazosDoProcesso => PrazosDoProcesso.WithIncludes();

        public DbSet<PartePedidoProcesso> PartesPedidosProcesso { get; set; }
        IQueryable<PartePedidoProcesso> IDatabaseContext.PartesPedidosProcesso => PartesPedidosProcesso.WithIncludes();
        
        public DbSet<TipoPrazo> TiposPrazos { get; set; }
        IQueryable<TipoPrazo> IDatabaseContext.TiposPrazos => TiposPrazos;

        public DbSet<DocumentoProcesso> DocumentosProcessos { get; set; }
        IQueryable<DocumentoProcesso> IDatabaseContext.DocumentosProcessos => DocumentosProcessos;

        public DbSet<AndamentoProcessoDocumento> AndamentosProcessosDocumentos { get; set; }
        IQueryable<AndamentoProcessoDocumento> IDatabaseContext.AndamentosProcessosDocumentos => AndamentosProcessosDocumentos;

        public DbSet<TipoOrientacaoJuridica> TiposOrientacoesJuridicas { get; set; }
        IQueryable<TipoOrientacaoJuridica> IDatabaseContext.TiposOrientacoesJuridicas => TiposOrientacoesJuridicas;

        public DbSet<OrientacaoJuridica> OrientacoesJuridicas { get; set; }
        IQueryable<OrientacaoJuridica> IDatabaseContext.OrientacoesJuridicas => OrientacoesJuridicas.WithIncludes();

        public DbSet<TipoVara> TiposDeVara { get; set; }
        IQueryable<TipoVara> IDatabaseContext.TiposDeVara => TiposDeVara;

        public DbSet<Vara> Varas { get; set; }    
        IQueryable<Vara> IDatabaseContext.Varas => Varas.WithIncludes();

        public IQueryable<ApuracaoProcesso> ApuracaoProcessos => Set<ApuracaoProcesso>();

        public DbSet<OrgaoBB> OrgaoBBs { get; set; }
        IQueryable<OrgaoBB> IDatabaseContext.OrgaoBBs => OrgaoBBs.WithIncludes();

        public DbSet<TribunalBB> TribunalBBs { get; set; }
        IQueryable<TribunalBB> IDatabaseContext.TribunalBBs => TribunalBBs;

        public DbSet<GrupoJuizadoVara> GrupoJuizadoVaras { get; set; }
        IQueryable<GrupoJuizadoVara> IDatabaseContext.GrupoJuizadoVaras => GrupoJuizadoVaras;
        
        public DbSet<Comarca> Comarcas { get; set; }
        IQueryable<Comarca> IDatabaseContext.Comarcas => Comarcas.WithIncludes();

        public DbSet<ComarcaBB> ComarcaBBs { get; set; }
        IQueryable<ComarcaBB> IDatabaseContext.ComarcaBBs => ComarcaBBs;

        public DbSet<ProcessoCpfCnpj> ProcessoCpfCnpjs { get; set; }
        IQueryable<ProcessoCpfCnpj> IDatabaseContext.ProcessoCpfCnpjs => ProcessoCpfCnpjs;

        public IQueryable<ComplementoDeAreaEnvolvida> ComplementoDeAreasEnvolvidas => Set<ComplementoDeAreaEnvolvida>();

        public IQueryable<AtmIndiceUfPadrao> AtmIndicesUfPadrao => Set<AtmIndiceUfPadrao>();

        public IQueryable<Indice> Indices => Set<Indice>();

        public IQueryable<AgendamentoDeFechamentoAtmUfPex> AgendamentoDeFechamentoAtmUfPex => Set<AgendamentoDeFechamentoAtmUfPex>();

        public IQueryable<FechamentoAtmPex> FechamentosAtmPex => Set<FechamentoAtmPex>();

        public DbSet<CotacaoIndiceTrabalhista> CotacaoIndiceTrabalhistas { get; set; }

        IQueryable<CotacaoIndiceTrabalhista> IDatabaseContext.CotacaoIndiceTrabalhistas => CotacaoIndiceTrabalhistas;

        public DbSet<TempCotacaoIndiceTrab> TempCotacaoIndiceTrabalhistas { get; set; }

        IQueryable<TempCotacaoIndiceTrab> IDatabaseContext.TempCotacaoIndiceTrabalhistas => TempCotacaoIndiceTrabalhistas;

        public IQueryable<AgendamentoDeFechamentoAtmPex> AgendamentosDeFechamentosAtmPex => Set<AgendamentoDeFechamentoAtmPex>();

        public IQueryable<Cotacao> Cotacoes => Set<Cotacao>().WithIncludes();

        public IQueryable<FechamentoAtmJec> FechamentosAtmJec => Set<FechamentoAtmJec>();

        public IQueryable<AgendamentoDeFechamentoAtmJec> AgendamentosDeFechamentosAtmJec => Set<AgendamentoDeFechamentoAtmJec>();
        
        public DbSet<Estado> Estados { get; set; }
        IQueryable<Estado> IDatabaseContext.Estados => Estados.WithIncludes();

        public DbSet<Municipio> Municipios { get; set; }
        IQueryable<Municipio> IDatabaseContext.Municipios => Municipios;

        public DbSet<LinhaProcesso> LinhaProcessos { get; set; }
        IQueryable<LinhaProcesso> IDatabaseContext.LinhaProcessos => LinhaProcessos;
       
        public DbSet<GrupoPedido> GruposPedidos { get; set; }        
        IQueryable<GrupoPedido> IDatabaseContext.GruposPedidos => GruposPedidos;

        public DbSet<DecisaoObjetoProcesso> DecisaoObjetoProcessos { get; set; }
        IQueryable<DecisaoObjetoProcesso> IDatabaseContext.DecisaoObjetoProcessos => DecisaoObjetoProcessos;

        public DbSet<ValorObjetoProcesso> ValorObjetoProcessos { get; set; }
        IQueryable<ValorObjetoProcesso> IDatabaseContext.ValorObjetoProcessos => ValorObjetoProcessos;
            
        public IQueryable<Esfera> Esferas => Set<Esfera>();            

        public IQueryable<IndiceCorrecaoEsfera> IndiceCorrecaoEsferas => Set<IndiceCorrecaoEsfera>().WithIncludes();
        
        public IQueryable<Evento> Eventos => Set<Evento>();
        public IQueryable<DecisaoEvento> DecisaoEventos => Set<DecisaoEvento>();
        public IQueryable<EventoDependente> EventosDependentes => Set<EventoDependente>(); 

        IQueryable<MigracaoAssunto> IDatabaseContext.MigracaoAssunto => Set<MigracaoAssunto>();
        IQueryable<MigracaoAcao>IDatabaseContext.MigracaoAcao => Set<MigracaoAcao>();               

        public IQueryable<MigracaoAssuntoEstrategico> MigracaoAssuntoEstrategico => Set<MigracaoAssuntoEstrategico>();

        IQueryable<MigracaoAcaoConsumidor> IDatabaseContext.MigracaoAcaoConsumidor => Set<MigracaoAcaoConsumidor>();

        IQueryable<MigracaoPedido> IDatabaseContext.MigracaoPedido => Set<MigracaoPedido>();

        IQueryable<MigracaoPedidoConsumidor> IDatabaseContext.MigracaoPedidoConsumidor => Set<MigracaoPedidoConsumidor>();

        IQueryable<TipoDocumentoMigracao> IDatabaseContext.TipoDocumentoMigracao => Set<TipoDocumentoMigracao>();

        IQueryable<TipoDocumentoMigracaoEstrategico> IDatabaseContext.TipoDocumentoMigracaoEstrategico => Set<TipoDocumentoMigracaoEstrategico>();
        public IQueryable<UsuarioOperacaoRetroativa> UsuarioOperacaoRetroativas => Set<UsuarioOperacaoRetroativa>().WithIncludes();

        public IQueryable<FatoGeradorProcesso> FatoGeradorProcessos => Set<FatoGeradorProcesso>();


        public IQueryable<FaseProcesso> FaseProcessos => Set<FaseProcesso>();
        public IQueryable<MigracaoEventoCivelConsumidor> MigracaoEventosCivelConsumidor => Set<MigracaoEventoCivelConsumidor>();
        public IQueryable<MigracaoEventoCivelEstrategico> MigracaoEventosCivelEstrategico => Set<MigracaoEventoCivelEstrategico>();
        public IQueryable<ColunasRelatorioEficienciaEvento> ColunasRelatorioEficienciaEventos => Set<ColunasRelatorioEficienciaEvento>();

        public IQueryable<ProcessoInconsistente> ProcessoInconsistentes => Set<ProcessoInconsistente>();
        public IQueryable<ProcessoTributarioInconsistente> ProcessoTributarioInconsistentes => Set<ProcessoTributarioInconsistente>();


        IQueryable<MigracaoTipoPrazoEstrategico> IDatabaseContext.MigracaoTipoPrazoEstrategico => Set<MigracaoTipoPrazoEstrategico>();

        IQueryable<MigracaoTipoPrazoConsumidor> IDatabaseContext.MigracaoTipoPrazoConsumidor => Set<MigracaoTipoPrazoConsumidor>();

        IQueryable<MigracaoTipoAudienciaEstrategico> IDatabaseContext.MigracaoTipoAudienciaEstrategico => Set<MigracaoTipoAudienciaEstrategico>();
        IQueryable<MigracaoTipoAudienciaConsumidor> IDatabaseContext.MigracaoTipoAudienciaConsumidor => Set<MigracaoTipoAudienciaConsumidor>();

        public DbSet<IndiceCorrecaoProcesso> IndiceVigencias { get; set; }
        IQueryable<IndiceCorrecaoProcesso> IDatabaseContext.IndiceVigencias =>  Set<IndiceCorrecaoProcesso>();

        public IQueryable<MotivoProvavelZero> MotivoProvavelZeros => Set<MotivoProvavelZero>();

        #endregion IQUERYABLES
    }
}