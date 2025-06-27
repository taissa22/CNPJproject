using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Data;
using Oi.Juridico.Contextos.V2.AgendamentoRelatorioNegociacaoContext.Entities;
using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.DTOs;
using Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Extensions;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;

namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoRelatorioNegociacao.Services
{
    public class AgendamentoRelatorioNegociacaoService
    {
        private readonly AgendamentoRelatorioNegociacaoDbContext _db;
        private readonly ParametroJuridicoContext _parametroJuridico;

        public AgendamentoRelatorioNegociacaoService(AgendamentoRelatorioNegociacaoDbContext db, ParametroJuridicoContext parametroJuridico)
        {
            _db = db;
            _parametroJuridico = parametroJuridico;
        }

        public async Task<(List<AgendamentoRelatorioNegociacaoResponse>? dadosAgendamento, int? total, string? mensagemErro)> ObtemDadosAgendamentoAsync(ObterAgendamentoRelatorioNegociacaoRequest requestDTO, CancellationToken ct)
        {
            try
            {
                requestDTO.DataInicioAgendamento = requestDTO.DataInicioAgendamento.HasValue ? requestDTO.DataInicioAgendamento.Value.Date : null;
                requestDTO.DataFimAgendamento = requestDTO.DataFimAgendamento.HasValue ? requestDTO.DataFimAgendamento.Value.Date : null;

                var query = _db.AgendExecRelNegociacao
                                            .AsNoTracking()
                                            .WhereIfNotEmpty(x => x.DatAgendamento.Date >= requestDTO.DataInicioAgendamento, requestDTO.DataInicioAgendamento.ToString())
                                            .WhereIfNotEmpty(x => x.DatAgendamento.Date <= requestDTO.DataFimAgendamento, requestDTO.DataFimAgendamento.ToString())
                                            .OrderByDescending(o => o.DatAgendamento)
                                            .Select(x => new AgendamentoRelatorioNegociacaoResponse
                                            {
                                                CodAgendExecRelNegociacao = x.CodAgendExecRelNegociacao,
                                                MensagemConfigExec = ObterMensagemConfigExec(x.PeriodicidadeExecucao, x.DiaDaSemana),
                                                MensagemPeriodoExec = ObterMensagemPeriodoExec(x.PeriodicidadeExecucao, x.DatInicioNegociacao, x.DatFimNegociacao, x.PeriodoSemanal, x.PeriodoMensal),
                                                MensagemTipoProcesso = ObterMensagemTipoProcesso(x.IndProcessoCc, x.IndProcessoJec, x.IndProcessoProcon),
                                                MensagemStatusProcesso = x.IndNegociacoesAtivas == "S" ? "Somente processos ativos" : "Processos ativos e inativos",
                                                MensagemPeriodicidade = x.Mensagem,
                                                MensagemErroTrace = x.MensagemErroTrace,
                                                MensagemUsrDatSolicitante = ObterMensagemUsrDatSolicitante(x.PeriodicidadeExecucao, x.UsrCodUsuario, x.DatAgendamento),
                                                Status = ((StatusAgendamentoResultadoNegociacaoEnum)x.Status!).ToDescription()
                                            });

                var dadosAgendamento = await query
                                    .Skip(requestDTO.Page * 5).Take(5)
                                    .ToListAsync(ct);

                var total = query.Count();

                return (dadosAgendamento, total, null);

            }
            catch (Exception e)
            {
                return (null, null, e.Message);
            }
        }

        public async Task<string> SalvarAgendamentoAsync(AgendamentoRelatorioNegociacaoRequest requestDto, string user, CancellationToken ct)
        {
            try
            {
                var agendamento = new AgendExecRelNegociacao();

                agendamento.DatAgendamento = DateTime.Now;
                agendamento.UsrCodUsuario = user;
                agendamento.DatProxExec = !requestDto.DatProxExec.HasValue ? CalcularDataProximoAgendamento(requestDto) : requestDto.DatProxExec;
                agendamento.Status = 1; // Agendado
                agendamento.Mensagem = ObterMensagem(requestDto.PeriodicidadeExecucao, agendamento.DatProxExec);
                // TIPOS DE PROCESSO
                agendamento.IndProcessoCc = requestDto.IndProcessoCc ? "S" : "N";
                agendamento.IndProcessoJec = requestDto.IndProcessoJec ? "S" : "N";
                agendamento.IndProcessoProcon = requestDto.IndProcessoProcon ? "S" : "N";
                // CONFIGURAÇÃO DE EXECUÇÃO
                agendamento.PeriodicidadeExecucao = requestDto.PeriodicidadeExecucao;
                agendamento.DiaDaSemana = requestDto.DiaDaSemana;
                agendamento.IndUltimoDiaMes = requestDto.IndUltimoDiaMes ? "S" : "N";
                agendamento.DiaDoMes = requestDto.DiaDoMes;
                // PERIODO DAS NEGOCIAÇÕES
                agendamento.DatInicioNegociacao = requestDto.DatInicioNegociacao;
                agendamento.DatFimNegociacao = requestDto.DatFimNegociacao;
                agendamento.PeriodoSemanal = requestDto.PeriodoSemanal;
                agendamento.PeriodoMensal = requestDto.PeriodoMensal;
                agendamento.IndNegociacoesAtivas = requestDto.IndNegociacoesAtivas ? "S" : "N";

                await _db.AgendExecRelNegociacao.AddAsync(agendamento, ct);
                await _db.SaveChangesAsync(ct);

                return "Agendamento incluído com sucesso.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public async Task<(bool excluido, string? mensagem)> RemoveAgendamentoAsync(int CodAgendExecRelNegociacao, CancellationToken ct)
        {
            try
            {
                var agendamento = _db.AgendExecRelNegociacao.FirstOrDefault(x => x.CodAgendExecRelNegociacao == CodAgendExecRelNegociacao);

                if (agendamento == null)
                {
                    return (false, "Não foi possivel encontrar o agendamento.");
                }

                _db.AgendExecRelNegociacao.Remove(agendamento);
                await _db.SaveChangesAsync(ct);

                return (true, "Agendamento excluído com sucesso.");

            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<(string? mensagem, string? nomeArquivo, string[]? nasArquivo)> DownloadAgendamentoAsync(int id, CancellationToken ct)
        {
            try
            {
                var nomeArquivo = _db.AgendExecRelNegociacao.FirstOrDefault(x => x.CodAgendExecRelNegociacao == id).NomArquivoGerado;

                if (nomeArquivo == null)
                {
                    return ("Não foi possivel encontrar o agendamento.", null, null);
                }

                var nasArquivo = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(ParametrosJuridicos.DIR_SERV_REL_NEGOCIACOES, nomeArquivo);

                return (null, nomeArquivo, nasArquivo);
            }
            catch (Exception e)
            {
                return (e.Message, null, null);
            }
        }


        #region CALCULAR DATAS
        private DateTime? CalcularDataProximoAgendamento(AgendamentoRelatorioNegociacaoRequest requestDto)
        {
            //Imediato
            if (requestDto.PeriodicidadeExecucao == 0)
                return DateTime.Now.Date;

            //Semanal
            if (requestDto.PeriodicidadeExecucao == 3)
            {
                return DateTime.Now.Date.ToNextDayOfWeek((DayOfWeek)requestDto.DiaDaSemana!.Value - 1);
            }

            //Mensal
            if (requestDto.PeriodicidadeExecucao == 4)
                return CalcularDataProximoAgendamentoMensal(DateTime.Now.Date, requestDto);

            return null;
        }

        private DateTime? CalcularDataProximoAgendamentoMensal(DateTime dataAtual, AgendamentoRelatorioNegociacaoRequest model)
        {

            var dataUltimoDiaMes = dataAtual.GetLastDayOfMonth();

            var dataPreviaInicial = model.IndUltimoDiaMes || dataUltimoDiaMes.Day <= model.DiaDoMes ? dataUltimoDiaMes : dataAtual.ToDayOfMonth(model.DiaDoMes!.Value);

            if (!model.IndUltimoDiaMes && (model.DiaDoMes.HasValue && model.DiaDoMes.Value < dataAtual.Day))
            {
                dataPreviaInicial = dataPreviaInicial.AddMonths(1);
            }

            //var dataInicialAgendamento = dataPreviaInicial.AddDays(1);
            return dataPreviaInicial;
        }

        private static string ObterDescricaoDoDiaDaSemana(int diaDaSemana)
        {
            if (diaDaSemana == ((int)DayOfWeek.Sunday)) return "Domingo";
            if (diaDaSemana == ((int)DayOfWeek.Monday)) return "Segunda-feira";
            if (diaDaSemana == ((int)DayOfWeek.Tuesday)) return "Terça-feira";
            if (diaDaSemana == ((int)DayOfWeek.Wednesday)) return "Quarta-feira";
            if (diaDaSemana == ((int)DayOfWeek.Thursday)) return "Quinta-feira";
            if (diaDaSemana == ((int)DayOfWeek.Friday)) return "Sexta-feira";
            if (diaDaSemana == ((int)DayOfWeek.Saturday)) return "Sábado";
            return string.Empty;
        }
        #endregion

        #region PRIVATE METHODS

        private static string ObterMensagem(int periodicidadeExecucao, DateTime? proximaExecucao)
        {
            switch (periodicidadeExecucao)
            {
                case 1: // Imediata
                    return "Este agendamento será processado em breve. Por favor, aguarde.";
                case 2: // Especifica
                    return $"Execução agendada para {proximaExecucao!.Value.ToString("dd/MM/yyyy")}.";
                case 3: // Semanal
                    return $"Próxima execução semanal agendada automaticamente para {proximaExecucao!.Value.ToString("dd/MM/yyyy")}.";
                case 4: // Mensal
                    return $"Próxima execução mensal agendada automaticamente para {proximaExecucao!.Value.ToString("dd/MM/yyyy")}.";
                default:
                    return string.Empty;
            }
        }

        private static string ObterMensagemConfigExec(int periodicidade, int? diaSemana)
        {
            switch (periodicidade)
            {
                case 1: // Imediata
                    return "Execução Imediata";
                case 2: // Especifica
                    return $"Execução data Específica";
                case 3: // Semanal
                    return $"Execução Semanal ({ObterDescricaoDoDiaDaSemana(diaSemana.HasValue ? diaSemana.Value - 1 : 0)})";
                case 4: // Mensal
                    return $"Execução Mensal";
                default:
                    return string.Empty;
            }
        }

        private static string ObterMensagemPeriodoExec(int periodicidade, DateTime? datInicio, DateTime? datFim, int? periodoSemanal, int? periodoMensal)
        {
            switch (periodicidade)
            {
                case 1: // Imediata
                case 2: // Especifica
                    return $"Negociações criadas no período: {datInicio!.Value.ToString("dd/MM/yyyy")} até {datFim!.Value.ToString("dd/MM/yyyy")}";
                case 3: // Semanal
                    return periodoSemanal == 1 ? "Última semana" : "Último mês";
                case 4: // Mensal
                    return periodoMensal == 1 ? "Último mês" : periodoMensal == 2 ? "Últimos 6 meses" : "Último ano";
                default:
                    return string.Empty;
            }
        }

        private static string ObterMensagemTipoProcesso(string indCC, string indJEC, string indPROCON)
        {
            List<string> tipos = new List<string>();

            if (indCC == "S")
                tipos.Add("Consumidor");

            if (indJEC == "S")
                tipos.Add("Juizado Especial");

            if (indPROCON == "S")
                tipos.Add("PROCON");

            return $"Negociações de processos: {string.Join(", ", tipos)}";
        }

        private static string ObterMensagemUsrDatSolicitante(int periodicidadeExecucao, string usuario, DateTime dataAgendamento)
        {
            switch (periodicidadeExecucao)
            {
                case 1: // Imediata
                case 2: // Especifica
                    return $"Agendamento solicitado por {usuario} \n em {dataAgendamento.ToString("dd/MM/yyyy 'às' HH:mm")}.";
                
                case 3: // Semanal
                case 4: // Mensal
                    return $"Agendamento recorrente solicitado por {usuario} \n em {dataAgendamento.ToString("dd/MM/yyyy 'às' HH:mm")}.";
                default:
                    return string.Empty;
            }
        }

        #endregion
    }
}
