using Oi.Juridico.Contextos.V2.PermissaoContext.Extensions;
using Oi.Juridico.Shared.V2.Enums;
using Oi.Juridico.Contextos.V2.AgendCargaCompromissoContext.Data;
using Oi.Juridico.WebApi.V2.Areas.Compromissos.DTOs;
using Oi.Juridico.Contextos.V2.AgendCargaCompromissoContext.Entities;
using Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.DTOs;
using Microsoft.AspNetCore.Http;
using System.IO;
using Oi.Juridico.Contextos.V2.ParametroJuridicoContext.Data;
using Perlink.Oi.Juridico.Application.Security;

namespace Oi.Juridico.WebApi.V2.Areas.AgendamentoCargaCompromissos.Services
{
    public class AgendamentoCargaCompromissoService
    {
        private ILogger<AgendamentoCargaCompromissoService> Logger { get; }
        private readonly AgendCargaCompromissoDbContext _db;
        private readonly ParametroJuridicoContext _parametroJuridico;

        public AgendamentoCargaCompromissoService(AgendCargaCompromissoDbContext db,
            ILogger<AgendamentoCargaCompromissoService> logger,
            ParametroJuridicoContext parametroJuridico)
        {
            _db = db;
            _parametroJuridico = parametroJuridico;
            Logger = logger;
        }


        public async Task<(string message, bool status)> Criar(IFormFileCollection arquivo, AgendamentoCargaCompromissoRequest requestDTO, string user, CancellationToken ct)
        {
            var caminhoNas = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_CARGA_COMPROM_RJ");

            var compromisso = new AgendCargaComp();
            compromisso.DatAgendamento = requestDTO.DatAgendamento.Value;
            compromisso.UsrCodUsuario = user;
            compromisso.Status = 1;     
            compromisso.NomArquivoBase = $"{Guid.NewGuid()}{Path.GetExtension(arquivo[0].FileName)}";
            compromisso.TipoProcesso = requestDTO.TipoProcesso;
            compromisso.ConfigExec = requestDTO.ConfigExec;
            compromisso.DatSolicitacao = DateTime.Now;
            compromisso.Mensagem = requestDTO.Mensagem;

            var diretorio = caminhoNas[0] + "\\agendados";

            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            using (var stream = System.IO.File.Create(Path.Combine(diretorio, compromisso.NomArquivoBase)))
                await arquivo[0].CopyToAsync(stream);

            await _db.AgendCargaComp.AddAsync(compromisso);
            await _db.SaveChangesAsync();

            return ("Arquivo agendado com sucesso.", true);
        }
         

        public async Task<(List<AgendamentoCargaCompromissoResponse>? dadosAgendamento, int? total, string? mensagemErro)> ObtemDadosAgendamentoAsync(ObterAgendamentoCargaCompromissoRequest requestDTO, CancellationToken ct)
        {
            try
            {
                requestDTO.DataInicioAgendamento = requestDTO.DataInicioAgendamento.HasValue ? requestDTO.DataInicioAgendamento.Value.Date : null;
                requestDTO.DataFimAgendamento = requestDTO.DataFimAgendamento.HasValue ? requestDTO.DataFimAgendamento.Value.Date : null;

                var query = _db.AgendCargaComp
                                            .AsNoTracking()
                                            .WhereIfNotEmpty(x => x.DatAgendamento >= requestDTO.DataInicioAgendamento.Value.Date.AddHours(0).AddMinutes(0), requestDTO.DataInicioAgendamento.ToString())
                                            .WhereIfNotEmpty(x => x.DatAgendamento <= requestDTO.DataFimAgendamento.Value.Date.AddHours(23).AddMinutes(59), requestDTO.DataFimAgendamento.ToString())
                                            .Where(x => x.Deletado != "S")
                                            .OrderByDescending(x => x.Status == (int)StatusAgendamentoResultadoEnum.Agendado)
                                            .ThenByDescending(o => o.DatAgendamento)
                                            .Select(x => new AgendamentoCargaCompromissoResponse
                                            {
                                                CodAgendCargaComp = x.CodAgendCargaComp,
                                                MensagemConfigExec = "", //ObterMensagemConfigExec(x.PeriodicidadeExecucao, x.DiaDaSemana),
                                                MensagemPeriodoExec = "",//ObterMensagemPeriodoExec(x.PeriodicidadeExecucao, x.DatInicioNegociacao, x.DatFimNegociacao, x.PeriodoSemanal, x.PeriodoMensal),
                                                MensagemTipoProcesso = "",//ObterMensagemTipoProcesso(x.IndProcessoCc, x.IndProcessoJec, x.IndProcessoProcon),
                                                MensagemStatusProcesso = "",//x.IndNegociacoesAtivas == "S" ? "Somente processos ativos" : "Processos ativos e inativos",
                                                MensagemPeriodicidade = x.Mensagem,
                                                MensagemErroTrace = x.MensagemErroTrace,
                                                MensagemUsrDatSolicitante = "",// ObterMensagemUsrDatSolicitante(x.PeriodicidadeExecucao, x.UsrCodUsuario, x.DatAgendamento),
                                                Status = ((StatusAgendamentoResultadoEnum)x.Status!).ToDescription(),

                                                DatAgendamento = x.DatAgendamento,
                                                DatIniExec = x.DatIniExec,
                                                DatFimExec = x.DatFimExec,
                                                EmpresaCentralizadora = x.EmpresaCentralizadora,
                                                NomArquivoBase = x.NomArquivoBase,
                                                ConfigExec = x.ConfigExec,
                                                DatSolicitacao = x.DatSolicitacao,
                                                Mensagem = x.Mensagem,
                                                NomArquivoGerado = x.NomArquivoGerado,
                                                TipoProcesso = x.TipoProcesso,
                                                UsrCodUsuario = x.UsrCodUsuario
                                            });

                var dadosAgendamento = await query
                                    .Skip(requestDTO.Page * requestDTO.PageSize).Take(requestDTO.PageSize)
                                    .ToListAsync(ct);

                var total = query.Count();

                return (dadosAgendamento, total, null);

            }
            catch (Exception e)
            {
                return (null, null, e.Message);
            }
        }

        public async Task<string> SalvarAgendamentoAsync(AgendamentoCargaCompromissoRequest requestDto, string user, CancellationToken ct)
        {
            try
            {
                var agendamento = new AgendCargaComp();

                agendamento.DatAgendamento = DateTime.Now;
                agendamento.UsrCodUsuario = user;
                agendamento.Status = (int)StatusAgendamentoResultadoEnum.Agendado;
                agendamento.Mensagem = "";// ObterMensagem(requestDto.PeriodicidadeExecucao, agendamento.DatProxExec);
                agendamento.Deletado = "N";
                // TIPOS DE PROCESSO
                agendamento.TipoProcesso = requestDto.TipoProcesso;

                await _db.AgendCargaComp.AddAsync(agendamento, ct);
                await _db.SaveChangesAsync(ct);

                return "Agendamento incluído com sucesso.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public async Task<(bool excluido, string? mensagem)> RemoveAgendamentoAsync(int CodAgendCargaCompromisso,string usuario, CancellationToken ct)
        {
            try
            {
                var agendamento = _db.AgendCargaComp.FirstOrDefault(x => x.CodAgendCargaComp == CodAgendCargaCompromisso);

                if (agendamento == null)
                {
                    return (false, "Não foi possivel encontrar o agendamento.");
                }

                //_db.AgendCargaComp.Remove(agendamento);
                agendamento.Deletado = "S";
                agendamento.DeletadoDatahora = DateTime.Now;
                agendamento.DeletadoLogin = usuario;
                _db.Update(agendamento);

                await _db.SaveChangesAsync(ct);

                return (true, "Agendamento excluído com sucesso.");

            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<(string? mensagem, string? nomeArquivo, string[]? nasArquivo)> DownloadAgendamentoAsync(int CodAgendCargaCompromisso,int tipo, CancellationToken ct)
        {
            try
            {
                var agendamento = _db.AgendCargaComp.FirstOrDefault(x => x.CodAgendCargaComp == CodAgendCargaCompromisso);
                var nomArquivo = tipo == 1 ?  Path.Combine("agendados",agendamento.NomArquivoBase) : Path.Combine("processados",agendamento.NomArquivoGerado);
                var nomArquivoDownload = tipo == 1 ? $"AgendamentoCarregado_{agendamento.CodAgendCargaComp}_{agendamento.DatAgendamento:yyyyMMdd_HHmmss}.csv" : agendamento.NomArquivoGerado;
                 

                //var diretorioNas = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync("DIR_NAS_CARGA_COMPROM_RJ");                
                //var diretorio = tipo == 1 ? Path.Combine(diretorioNas[0], "agendados", agendamento.NomArquivoBase) : Path.Combine(diretorioNas[0], "agendados", agendamento.NomArquivoBase);

                var nasArquivo = await _parametroJuridico.TratarCaminhoDinamicoArrayAsync(Shared.V2.Enums.ParametrosJuridicos.DIR_NAS_CARGA_COMPROM_RJ, nomArquivo);
                

                return (null, nomArquivoDownload, nasArquivo);
            }
            catch (Exception e)
            {
                return (e.Message, null, null);
            }
        }

        #region CALCULAR DATAS

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
