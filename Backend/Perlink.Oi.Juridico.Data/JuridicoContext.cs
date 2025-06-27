using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;
using Perlink.Oi.Juridico.Domain.Logs.DTO;
using Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.Entity;
using Perlink.Oi.Juridico.Domain.Processos.DTO.AgendaAudiencia;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.DTO.LoteCriacao;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.InterfaceBB;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Entity.Processos;
using Shared.Domain.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Perlink.Oi.Juridico.Domain.Logs.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.DTO;
using Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.Entity;
using Perlink.Oi.Juridico.Domain.GrupoDeEstados.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.Entity;
using Perlink.Oi.Juridico.Domain.ContingenciaPex.DTO;
using System;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.DTO;
using Perlink.Oi.Juridico.Domain.Processos.Entity;

namespace Perlink.Oi.Juridico.Data
{
    public class JuridicoContext : DbContext
    {
        protected readonly IAuthenticatedUser user;

        public JuridicoContext(DbContextOptions<JuridicoContext> options, IAuthenticatedUser user) : base(options)
        {
            this.user = user;
        }

        #region Controle de Acesso

        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<TokenSeguranca> TokensSeguranca { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioGrupo> UsuarioGrupos { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbQuery<UsuariosPerfilDTO> UsuariosPerfis { get; set; }        
        public DbQuery<PerfilDTO> Perfis { get; set; }

        public DbQuery<PermissaoDTO> PermissoesUsuarioLogado { get; set; }

        #endregion Controle de Acesso

        #region Grupo de Estados

        public DbSet<GrupoEstados> GrupoEstados { get; set; }
        public DbSet<Domain.GrupoDeEstados.Entity.GrupoDeEstados> GrupoDeEstados { get; set; }

        #endregion Grupo de Estados

        #region Compartilhados

        public DbSet<Parte> Parte { get; set; }
        public DbSet<Banco> Banco { get; set; }
        public DbSet<TiposSaldosGarantias> TiposSaldosGarantias { get; set; }
        public DbSet<Profissional> Profissional { get; set; }
        public DbSet<StatusPagamento> StatusPagamento { get; set; }
        public DbSet<TipoProcesso> TipoProcesso { get; set; }
        public DbSet<LancamentoProcesso> LancamentoProcessos { get; set; }
        public DbSet<FormaPagamento> FormaPagamentos { get; set; }
        public DbSet<CentroCusto> CentroCusto { get; set; }
        public DbSet<Fornecedor> Fornecedor { get; set; }
        public DbSet<TipoLancamento> TipoLancamento { get; set; }
        public DbSet<ParteProcesso> ParteProcesso { get; set; }
        public DbSet<Processo> Processos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoProcesso> PedidoProcessos { get; set; }
        public DbSet<CompromissoProcesso> CompromissoProcessos { get; set; }
        public DbSet<CompromissoProcessoCredor> CompromissoProcessoCredores { get; set; }
        public DbSet<CompromissoProcessoParcela> CompromissoProcessoParcelas { get; set; }
        public DbSet<GrupoCorrecaoGarantia> GrupoCorrecaoGarantias { get; set; }
        public DbSet<CategoriaFinalizacao> CategoriaFinalizacoes { get; set; }

        public DbSet<ContratoPedidoProcesso> ContratoPedidoProcesso { get; set; }
        public DbSet<ContratoProcesso> ContratoProcesso { get; set; }
        public DbSet<EmpresasCentralizadoras> EmpresasCentralizadoras { get; set; }
        public DbSet<Sequencial> Sequencial { get; set; }
        public DbSet<ClassesGarantias> ClassesGarantias { get; set; }
        public DbSet<Vara> Varas { get; set; }

        public DbSet<Procedimento> Procedimentos { get; set; }        

        #endregion Compartilhados

        #region SAP

        public DbSet<Lote> Lotes { get; set; }

        public DbSet<LoteLancamento> LoteLancamentos { get; set; }
        public DbSet<Bordero> Bordero { get; set; }

        public DbSet<Log_Lote> Log_Lote { get; set; }

        public DbSet<Comarca> Comarca { get; set; }
        public DbSet<BBTribunais> BBTribunais { get; set; }
        public DbSet<BBResumoProcessamento> BBResumoProcessamento { get; set; }
        public DbSet<BBNaturezasAcoes> BBNaturezasAcoes { get; set; }
        public DbSet<TipoVara> TipoVaraProcessos { get; set; }

        public DbSet<BBStatusParcelas> BBStatusParcelas { get; set; }
        public DbSet<BBStatusRemessa> BBStatusRemessa { get; set; }
        public DbSet<CategoriaPagamento> CategoriaPagamentos { get; set; }
        public DbSet<MigracaoCategoriaPagamento> MigracaoCategoriaPagamento { get; set; }
        public DbSet<MigracaoCategoriaPagamentoEstrategico> MigracaoCategoriaPagamentoEstrategico { get; set; }
        public DbSet<Empresas_Sap> Empresas_sap { get; set; }
        public DbSet<EmpresasSapFornecedoras> EmpresasSapFornecedoras { get; set; }
        public DbSet<GruposLotesJuizados> GruposLotesJuizados { get; set; }
        public DbSet<BBComarca> BBComarcas { get; set; }
        public DbSet<BBModalidade> BBModalidades { get; set; }
        public DbSet<CriterioSaldoGarantia> CriterioSaldoGarantias { get; set; }

        public DbSet<PedidoSAP> PedidoSAPs { get; set; }
        public DbSet<FornecedorasContratos> fornecedorasContratos { get; set; }
        public DbSet<FornecedorasFaturas> FornecedorasFaturas { get; set; }
        public DbSet<FornecedorFormaPagamento> FornecedorasFormasPagamentos { get; set; }
        public DbSet<Feriados> Feriados { get; set; }
        public DbSet<BBOrgaos> BBOrgaos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<CargaCompromissoParcela> CargaCompromissoParcelas { get; set; }
        public DbSet<Acao> Acoes { get; set; }
        public DbSet<EmpresasCentralizadorasConvenio> EmpresasCentralizadorasConvenios { get; set; }

        public DbSet<Log_ExecucaoLote> Log_ExecucaoLotes { get; set; }
        public DbSet<AgendamentoSaldoGarantia> AgendamentoSaldoGarantias { get; set; }

        public DbQuery<LoteCriacaoResultadoEmpresaCentralizadoraDTO> LoteCriacaoResultadoEmpresaCentralizadoraDTO { get; set; }
        public DbQuery<LoteCriacaoResultadoEmpresaGrupoDTO> LoteCriacaoResultadoEmpresaGrupoDTO { get; set; }
        public DbQuery<EscritorioDTO> EscritorioDTO { get; set; }
        public DbQuery<AdvogadoEscritorioDTO> AdvogadoEscritorioDTO { get; set; }
        public DbQuery<EscritorioStringDTO> EscritorioStringDTO { get; set; }
        public DbQuery<PrepostoDTO> PrepostoDTO { get; set; }
        public DbQuery<LoteCriacaoResultadoLancamentoDTO> LoteCriacaoResultadoLancamentoDTO { get; set; }
        public DbQuery<SaldoGarantiaResultadoDTO> SaldoGarantiaResultadoDTODTO { get; set; }
        public DbQuery<ProcessoDTO> ProcessoDTO { get; set; }
        public DbQuery<LogUsuarioDTO> LogListaLogUsuario { get; set; }
        public DbQuery<LogPropostaContatoProcessoDTO> logPropostaContatoProcesso { get; set; }
        #endregion SAP

        #region Log Processo

        public DbSet<LogProcesso> LogProcessos { get; set; }
        public DbSet<LogPedidoProcesso> LogPedidoProcessos { get; set; }
        public DbSet<LogParteProcesso> LogParteProcessos { get; set; }
        public DbSet<LogValorProcesso> LogValorProcessos { get; set; }
        public DbSet<LogAndamentoProcesso> LogAndamentoProcessos { get; set; }
        public DbSet<LogAndamentoPedidoProcesso> LogAndamentoPedidoProcessos { get; set; }
        public DbSet<LogAndamentoServicoProcesso> LogAndamentoServicoProcessos { get; set; }

        public DbSet<LogAndamentoLancProcesso> LogAndamentoLancamentos { get; set; }
        public DbSet<LogObservacaoProcesso> LogObservacaoProcessos { get; set; }
        public DbSet<LogAudienciaProcesso> LogAudienciaProcessos { get; set; }
        public DbSet<LogLancamentoProcesso> LogLancamentoProcessos { get; set; }
        public DbSet<LogAdvogadoAutorProcesso> LogAdvogadoAutorProcessos { get; set; }
        public DbSet<LogLinhaProcesso> LogLinhaProcessos { get; set; }
        public DbSet<LogApuracaoBloqueio> LogApuracaoBloqueios { get; set; }
        public DbSet<LogApuracaoCartaCobranca> LogApuracaoCartaCobrancas { get; set; }
        public DbSet<LogApuracaoContestacao> LogApuracaoContestacoes { get; set; }
        public DbSet<LogApuracaoDossie> LogApuracaoDossies { get; set; }
        public DbSet<LogApuracaoContaEmDebito> LogApuracaoContaEmDebitos { get; set; }
        public DbSet<LogApuracaoProtecaoCredito> LogApuracaoProtecaoCreditos { get; set; }
        public DbSet<LogApuracaoMedicaoPulso> LogApuracaoMedicaoPulsos { get; set; }
        public DbSet<LogMotivacaoProcesso> LogMotivacaoProcessos { get; set; }
        public DbSet<LogContatoProcesso> LogContatoProcessos { get; set; }
        public DbSet<LogServicoProcesso> LogServicoProcessos { get; set; }
        public DbSet<LogPartePedidoProcesso> LogPartePedidoProcessos { get; set; }
        public DbSet<LogProcessosConexos> LogProcessosConexos { get; set; }
        public DbSet<LogPendenciaProcesso> LogPendenciasProcessos { get; set; }
        public DbSet<LogEncaminhamentosProcessos> LogEncaminhamentosProcessos { get; set; }
        public DbSet<LogDecisaoAndamentoProcesso> LogDecisaoAndamentoProcessos { get; set; }
        public DbSet<AdvogadoEscritorio> AdvogadoEscritorio { get; set; }
        public DbSet<AudienciaProcesso> AudienciaProcesso { get; set; }
        public DbSet<Preposto> Preposto { get; set; }

        #endregion Processos
        public DbSet<JuroCorrecaoProcesso> JuroCorrecaoProcesso { get; set; }
        public DbSet<AlteracaoEmBloco> AlteracaoEmBlocos { get; set; }

        public DbSet<TipoContratoEscritorio> TipoContratoEscritorios { get; set; }

        #region Contingencia

        public DbQuery<FechamentoContingenciaCCPorMediaDTO> FechamentoContingenciaCCPorMediaDTO { get; set; }
        public DbSet<FechamentoCivelConsumidorPorMedia> FechamentoCCPorMedias { get; set; }

        public DbQuery<FechamentoContingenciaPexMediaDTO> FechamentoContingenciaPexMediaDTO { get; set; }
        public DbSet<FechamentoPexMedia> FechamentoPexMedias { get; set; }
        
        #endregion

        #region Manutenção
        public DbSet<TipoAudiencia> TiposAudiencias { get; set; }

        public DbSet<BaseCalculo> BaseCalculos { get; set; }

        public DbSet<TipoParticipacao> TipoParticipacoes { get; set; }
        #endregion

        #region Apuracao Outliers
        public DbQuery<AgendamentoApuracaoValoresCalculadosDTO> AgendamentoApuracaoValoresCalculadosDTO { get; set; }
        #endregion Apuracao Outliers

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }

        public override int SaveChanges()
        {
            var returnedId = 0;

            //TODO: [MARCUS]
            //this.user = this.GetInfrastructure().GetRequiredService<IAuthenticatedUser>();

            if (string.IsNullOrEmpty(user.Login)) throw new AuthenticationException("Usuário não autenticado, não foi possível efetuar a chamada da procedure SP_ACA_INSERE_LOG_USUARIO");

            using (var scope = Database.BeginTransaction())
            {
                base.Database.ExecuteSqlCommand(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'S')", user.Login));
                returnedId = base.SaveChanges(true);
                scope.Commit();
            }

            return returnedId;
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var returnedId = 0;

            //TODO: [MARCUS]
            //this.user = this.GetInfrastructure().GetRequiredService<IAuthenticatedUser>();

            if (string.IsNullOrEmpty(user.Login)) throw new AuthenticationException("Usuário não autenticado, não foi possível efetuar a chamada da procedure SP_ACA_INSERE_LOG_USUARIO");

            using (var scope = Database.BeginTransaction())
            {
                base.Database.ExecuteSqlCommand(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'S')", user.Login));
                returnedId = base.SaveChanges(acceptAllChangesOnSuccess);
                scope.Commit();
            }

            return returnedId;
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                var returnedId = 0;

                //TODO: [MARCUS]
                //this.user = this.GetInfrastructure().GetRequiredService<IAuthenticatedUser>();

                if (string.IsNullOrEmpty(user.Login)) throw new AuthenticationException("Usuário não autenticado, não foi possível efetuar a chamada da procedure SP_ACA_INSERE_LOG_USUARIO");

                using (var scope = await Database.BeginTransactionAsync())
                {
                    await base.Database.ExecuteSqlCommandAsync(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'S')", user.Login));
                    returnedId = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
                    scope.Commit();
                }

                return returnedId;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //try {

            var returnedId = 0;
            //TODO: [MARCUS]
            //this.user = this.GetInfrastructure().GetRequiredService<IAuthenticatedUser>();

            if (string.IsNullOrEmpty(user.Login)) throw new AuthenticationException("Usuário não autenticado, não foi possível efetuar a chamada da procedure SP_ACA_INSERE_LOG_USUARIO");

            using (var scope = await Database.BeginTransactionAsync())
            {
                await base.Database.ExecuteSqlCommandAsync(string.Format("call jur.SP_ACA_INSERE_LOG_USUARIO('{0}', 'S')", user.Login));
                returnedId = await base.SaveChangesAsync(true, cancellationToken);
                scope.Commit();
            }

            return returnedId;
            //}
            //catch (System.Exception ex) {
            //    throw ex;
            //}
        }

        public async Task<string> ExecutarQuery(string query, object[] parametros)
        {
            try
            {
                string resultado = string.Empty;
                using (var command = base.Database.GetDbConnection().CreateCommand())
                {
                    await base.Database.GetDbConnection().OpenAsync();
                    command.Connection = base.Database.GetDbConnection();
                    command.CommandText = query;
                    command.Parameters.AddRange(parametros);

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        var reader = Serialize(result);
                        resultado = JsonConvert.SerializeObject(reader, Formatting.Indented);
                    }
                    base.Database.GetDbConnection().Close();
                }

                return resultado;
            }
            catch (System.Exception e)
            {
                base.Database.GetDbConnection().Close();
                throw new System.Exception("Erro na execução SQL: " + e.Message);
            }
        }
        public IEnumerable<Dictionary<string, object>> Serialize(DbDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        DbDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }

        public IDataReader ExecutarComandoSql(string sql)
        {
            IDataReader data;
            using (var command = base.Database.GetDbConnection().CreateCommand())
            {
                base.Database.GetDbConnection().Open();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                data = command.ExecuteReader();
            }
            return data;
        }
    }
}