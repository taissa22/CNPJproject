using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.ApuracaoOutliers.Repository {
    public class AgendarApuracaoOutlierRepository : BaseCrudRepository<AgendarApuracaoOutliers, long>, IAgendarApuracaoOutlierRepository {
        public AgendarApuracaoOutlierRepository(JuridicoContext context, IAuthenticatedUser user) : base(context, user) { }        

        public override Task<IDictionary<string, string>> RecuperarDropDown() {
            throw new NotImplementedException();
        }
        public async Task<AgendarApuracaoOutliers> AgendarApuracaoOutliers(AgendarApuracaoOutliers agendarApuracaoOutliers) {
            await base.Inserir(agendarApuracaoOutliers);
            return agendarApuracaoOutliers;
        }

        public void RemoverAgendamento(AgendarApuracaoOutliers agendamento) {
            context.Set<AgendarApuracaoOutliers>().Remove(agendamento);
        }

        public async Task<IEnumerable<ListarAgendamentosApuracaoOutliersDTO>> CarregarAgendamento(int pagina, int qtd) {
            return await context.Set<AgendarApuracaoOutliers>()
                .AsNoTracking()
                .Include(x=>x.FechamentosProcessosJEC)
                .OrderByDescending(o => o.DataSolicitacao)
                .Skip((pagina - 1) * qtd)
                .Take(qtd)
                .Select(AgendarApuracaoOutliers => new ListarAgendamentosApuracaoOutliersDTO {
                    Id = AgendarApuracaoOutliers.Id,
                    CodigoEmpresaCentralizadora = AgendarApuracaoOutliers.CodigoEmpresaCentralizadora,
                    MesAnoFechamento = AgendarApuracaoOutliers.MesAnoFechamento,
                    DataFechamento = AgendarApuracaoOutliers.DataFechamento,
                    DataFinalizacao = AgendarApuracaoOutliers.DataFinalizacao,
                    DataSolicitacao = AgendarApuracaoOutliers.DataSolicitacao,
                    NumeroMeses = AgendarApuracaoOutliers.FechamentosProcessosJEC.NumeroMeses,
                    FatorDesvioPadrao = AgendarApuracaoOutliers.FatorDesvioPadrao,
                    Observacao = AgendarApuracaoOutliers.Observacao,
                    NomeUsuario = AgendarApuracaoOutliers.NomeUsuario,
                    ArquivoBaseFechamento = AgendarApuracaoOutliers.ArquivoBaseFechamento,
                    ArquivoResultado = AgendarApuracaoOutliers.ArquivoResultado,
                    Status = AgendarApuracaoOutliers.Status,
                    MgsStatusErro = AgendarApuracaoOutliers.MgsStatusErro,
                    IndFechamentoMensal = AgendarApuracaoOutliers.FechamentosProcessosJEC.IndicaFechamentoMes
                })
                .ToListAsync();
        }

        public async Task<int> ObterQuantidadeTotal() {
            return await context.Set<AgendarApuracaoOutliers>()
                .AsNoTracking()
                .CountAsync();
        }

        #region Executor
        public async Task<ICollection<AgendarApuracaoOutliers>> ObterAgendados() {
            return await context.Set<AgendarApuracaoOutliers>()
                .AsTracking()
                .Include(a => a.FechamentosProcessosJEC)
                .Where(a => a.Status == AgendarApuracaoOutliersStatusEnum.Agendado)                
                .ToListAsync();
        }

        public async Task<ApuracaoOutliersDownloadResultadoDTO> ObterResultadoDoAgendamento(long id)
        {
            return await context.Set<AgendarApuracaoOutliers>()
           .Where(a => a.Id == id)
           .Select(AgendarApuracaoOutliers => new ApuracaoOutliersDownloadResultadoDTO
           {
               DesvioPadrao = AgendarApuracaoOutliers.ValorDesvioPadrao,
               MediaPagamentos = AgendarApuracaoOutliers.ValorMedia,
               FatorDesvioPadrao = AgendarApuracaoOutliers.FatorDesvioPadrao,
               ValorCorteOutliers = AgendarApuracaoOutliers.ValorCorteOutliers,
               QtdProcessosPagamentos = AgendarApuracaoOutliers.QtdProcessos,
               ValorTotalPagamentos = AgendarApuracaoOutliers.ValorTotalProcessos
               
           })
           .FirstOrDefaultAsync();
        }

        public async Task<AgendarApuracaoOutliers> RealizarCalculos(AgendarApuracaoOutliers agendamento) {
            await context.Database.ExecuteSqlCommandAsync(string.Format("call JUR.AO_CALCULAR_BASE_FECHAM_JEC({0}, TO_DATE('{1}','DD/MM/YYYY'), TO_DATE('{2}','DD/MM/YYYY'), {3}, {4})",
                agendamento.CodigoEmpresaCentralizadora, agendamento.MesAnoFechamento.ToString("dd/MM/yyyy"), agendamento.DataFechamento.ToString("dd/MM/yyyy"), agendamento.Id, agendamento.FatorDesvioPadrao.ToString().Replace(",",".")));

            var sql = @"select jur.agendar_apuracao_outliers.cod_agendar_apuracao_outlier as Id,
                                     jur.agendar_apuracao_outliers.val_desvio_padrao DesvioPadrao,
                                     jur.agendar_apuracao_outliers.val_media MediaPagamentos,
                                     jur.agendar_apuracao_outliers.val_corte_outliers ValorCorteOutliers,
                                     jur.agendar_apuracao_outliers.qtd_processos QtdProcessosPagamentos,
                                     jur.agendar_apuracao_outliers.val_total_processos ValorTotalPagamentos
                                from jur.agendar_apuracao_outliers
                               where jur.agendar_apuracao_outliers.cod_agendar_apuracao_outlier = {0}";
            var ValoresCalculadosDto = await this.context.Query<AgendamentoApuracaoValoresCalculadosDTO>().FromSql(sql, agendamento.Id).ToListAsync();

            agendamento.ValorCorteOutliers = ValoresCalculadosDto.FirstOrDefault().ValorCorteOutliers;
            agendamento.ValorDesvioPadrao = ValoresCalculadosDto.FirstOrDefault().DesvioPadrao;
            agendamento.ValorMedia = ValoresCalculadosDto.FirstOrDefault().MediaPagamentos;
            agendamento.ValorTotalProcessos = ValoresCalculadosDto.FirstOrDefault().ValorTotalPagamentos;
            agendamento.QtdProcessos = ValoresCalculadosDto.FirstOrDefault().QtdProcessosPagamentos;

            return agendamento;
        }
        #endregion Executor
    }
}
