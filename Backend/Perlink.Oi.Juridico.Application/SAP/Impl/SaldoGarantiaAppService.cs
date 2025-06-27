
using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.SAP.Interface;
using Perlink.Oi.Juridico.Application.SAP.ViewModel.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.DTO.SaldoGarantia;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Enum;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Application.Utilidades.Compression;
using Shared.Domain.Impl;
using Shared.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.SAP.Impl
{
    public class SaldoGarantiaAppService : BaseCrudAppService<AgendamentoSaldoGarantiaViewModel, AgendamentoSaldoGarantia, long>, ISaldoGarantiaAppService
    {
        private readonly IMapper mapper;
        private readonly IBancoService bancoService;
        private readonly IEmpresaDoGrupoService empresaDoGrupoService;
        private readonly IEstadoService estadoService;
        private readonly ISaldoGarantiaService service;
        private readonly IParametroRepository parametroRepository;
        // private readonly IHostingEnvironment _env;

        public SaldoGarantiaAppService(ISaldoGarantiaService service,
                                        IMapper mapper,
                                        IBancoService bancoService,
                                        IEmpresaDoGrupoService empresaDoGrupo,
                                        IEstadoService estadoService,
                                        IParametroRepository parametroRepository
                                       // IHostingEnvironment env
                                       ) : base(service, mapper)
        {
            this.mapper = mapper;
            this.bancoService = bancoService;
            this.empresaDoGrupoService = empresaDoGrupo;
            this.estadoService = estadoService;
            this.service = service;
            this.parametroRepository = parametroRepository;
            // _env = env;
        }

        public async Task<IResultadoApplication<SaldoGarantiaFiltrosViewModel>> CarregarFiltros(long codigoTipoProcesso)
        {
            var result = new ResultadoApplication<SaldoGarantiaFiltrosViewModel>();

            try
            {
                var model = new SaldoGarantiaFiltrosDTO()
                {
                    ListaBancos = await bancoService.RecuperarListaBancos(),
                    ListaEmpresaDoGrupo = await empresaDoGrupoService.RecuperarEmpresaDoGrupo(),
                    ListaEstados = await estadoService.RecuperarListaEstados()
                };

                result.DefinirData(mapper.Map<SaldoGarantiaFiltrosViewModel>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;
        }

        public async Task<IPagingResultadoApplication<ICollection<AgendamentoResultadoViewModel>>> ConsultarAgendamento(OrdernacaoPaginacaoDTO filtroDTO)
        {
            var result = new PagingResultadoApplication<ICollection<AgendamentoResultadoViewModel>>();

            try
            {
                var model = await service.ConsultarAgendamentos(filtroDTO);
                if (filtroDTO.Total <= 0)
                {
                    result.Total = await service.RecuperarTotalRegistros();
                }
                else
                {
                    if (Math.Floor(Convert.ToDecimal(filtroDTO.Total / filtroDTO.Quantidade)) == filtroDTO.Pagina)
                    {
                        result.Total = await service.RecuperarTotalRegistros();
                    }
                    else
                    {
                        result.Total = filtroDTO.Total;
                    }
                }

                result.DefinirData(mapper.Map<IList<AgendamentoResultadoViewModel>>(model));
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (System.Exception ex)
            {
                result.ExecutadoComErro(ex);
            }

            return result;

        }

        public async Task Expurgar(ILogger logger)
        {
            logger.LogInformation("################### Início da rotina Expurgar ###################");

            int paramDias = Convert.ToInt32(parametroRepository.RecuperarPorNome("EXPURGO_SALDO_GARANTIAS").Conteudo);
            DateTime dataExpurgo = DateTime.Today.AddDays(-paramDias);
            logger.LogInformation($"Verificando agendamentos com data superior a {dataExpurgo.ToString("dd/MM/yyyy")}");

            var agendamentosAntigos = await service.Pesquisar()
                .Include(a => a.CriteriosSaldoGarantias)
                .Where(a => a.DataAgendamento <= dataExpurgo)
                .AsNoTracking()
                .ToListAsync();
            logger.LogInformation($"Existe(m) {agendamentosAntigos.Count} agendamento(s) para expurgo");
            logger.LogInformation("Buscando o NAS do diretório do servidor para expurgar arquivos");
            string diretorioArquivo = this.parametroRepository.RecuperarPorNome("DIR_NAS_AGEND_SAL_GAR").Conteudo;
            long cont = 0;
            foreach (AgendamentoSaldoGarantia agendamento in agendamentosAntigos)
            {
                try
                {
                    // Expurgando arquivo
                    if (agendamento.NomeArquivoGerado != null)
                    {
                        string arquivo = Path.Combine(diretorioArquivo, agendamento.NomeArquivoGerado);
                        if (File.Exists(arquivo))
                        {
                            File.Delete(arquivo);
                            logger.LogInformation($"Arquivo {Path.Combine(diretorioArquivo, agendamento.NomeArquivoGerado)} excluído.");
                        }
                    }

                    // Remove agendamento com cascade delete
                    await ExcluirAgendamento(agendamento.Id);

                    logger.LogInformation($"Agendamento \"{agendamento.NomeAgendamento})\" e seus critérios foram expurgados.\n");
                    cont++;
                }
                catch (Exception)
                {
                    logger.LogInformation($"Ocorreu um erro ao expurgar o agendamento ({agendamento.NomeArquivoGerado}, {agendamento.Id})");
                }
            }
            logger.LogInformation($"Foram expurgados {cont} de {agendamentosAntigos.Count}.");
            logger.LogInformation("################### Fim da rotina Expurgar ###################\n\n");
        }

        public async Task ExecutarAgendamentoSaldoGarantia(ILogger logger)
        {
            logger.LogInformation("################### Início da rotina ExecutarAgendamentoSaldoGarantia ###################");

            logger.LogInformation("Tratando agendamentos interrompidos.");
            TratamentoDeInterrupcaoBat();

            logger.LogInformation("Buscando agendamentos com status de agendados");
            var agendamentos = await service.Pesquisar()
                    .Include(a => a.CriteriosSaldoGarantias)
                    .Where(a => a.CodigoStatusAgendamento == StatusAgendamento.Agendado.GetHashCode())
                    .ToListAsync();

            logger.LogInformation($"Foram encontrados \"{agendamentos.Count}\" agendamentos para execução.");

            foreach (var agendamento in agendamentos)
            {
                try
                {
                    agendamento.DataExecucao = DateTime.Now;
                    agendamento.CodigoStatusAgendamento = StatusAgendamento.EmExecucao.GetHashCode();
                    service.Commit();
                    logger.LogInformation($"Agendamento \"{agendamento.NomeAgendamento}\" ({agendamento.Id}) em processamento.");

                    logger.LogInformation($"Iniciando a execução dos filtros do agendamento \"{agendamento.NomeAgendamento}\".");
                    var model = await service.ExecutarAgendamentoSaldoGarantia(agendamento);
                    logger.LogInformation($"Filtros do agendamento \"{agendamento.NomeAgendamento}\" foi executado e retornou {model.Count} registros.");

                    logger.LogInformation($"Iniciando a criação do arquivo para o agendamento \"{agendamento.NomeAgendamento}\".");
                    var nomeArquivo = CriarArquivoAgendamentoExecutado(model, agendamento.CodigoTipoProcesso, logger);
                    agendamento.NomeArquivoGerado = nomeArquivo;
                    agendamento.DataFinalizacao = DateTime.Now;
                    agendamento.CodigoStatusAgendamento = StatusAgendamento.Executado.GetHashCode();
                    service.Commit();

                    logger.LogInformation($"Fim da execução do agendamento \"{agendamento.NomeAgendamento}\".\n");
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"Erro ao executar o agendamento: \"{agendamento.NomeAgendamento}\" ({agendamento.Id}).\n");
                    agendamento.CodigoStatusAgendamento = StatusAgendamento.Erro.GetHashCode();
                    agendamento.MensagemErro = ex.Message;
                    agendamento.MensagemErroTrace = ex.StackTrace;
                    await service.Atualizar(agendamento);
                    service.Commit();
                }
            }

            logger.LogInformation("################### Fim da rotina ExecutarAgendamentoSaldoGarantia ###################\n\n");
        }

        private async void TratamentoDeInterrupcaoBat()
        {
            var StatusProcessandos = await service.Pesquisar()
              .Include(a => a.CriteriosSaldoGarantias)
              .Where(a => a.CodigoStatusAgendamento == StatusAgendamento.EmExecucao.GetHashCode())
              .ToListAsync();
            foreach (var StatusProcessando in StatusProcessandos)
            {
                StatusProcessando.DataExecucao = DateTime.Now;
                StatusProcessando.CodigoStatusAgendamento = StatusAgendamento.Erro.GetHashCode();
                service.Commit();
            }
        }

        private string CriarArquivoAgendamentoExecutado(ICollection<SaldoGarantiaResultadoDTO> saldosGarantias, long tipoProcesso, ILogger logger)
        {
            try
            {
                logger.LogInformation("Buscando o NAS do diretório do servidor");
                var path = parametroRepository.RecuperarPorId("DIR_NAS_AGEND_SAL_GAR").Result.Conteudo;
#if DEBUG
                path = "C:\\Temp\\AGENDAMENTO_SALDO_GARANTIA";
#endif          
                Directory.CreateDirectory(path);
                var nomeProcesso = EnumExtensions.GetNomeFromValue<TipoProcessoEnum>(tipoProcesso);
                var nomeArquivo = $"Saldo_Garantias_{nomeProcesso}_{DateTime.Now.ToString("yyyyMMdd_hhmmssfff", CultureInfo.InvariantCulture)}";
                string filePathCSV = CriarCSV(saldosGarantias, tipoProcesso, path, nomeArquivo);
                var filePathZIP = Path.Combine(path, nomeArquivo + ".zip");
                ZipparArquivo(filePathCSV, filePathZIP);
                logger.LogInformation($"Arquivo criado em: {filePathZIP}");
                File.Delete(filePathCSV);
                return filePathZIP;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro durante a criação do arquivo: {e.Message}");
            }
        }

        private string CriarCSV(ICollection<SaldoGarantiaResultadoDTO> saldosGarantias, long tipoProcesso, string path, string nomeArquivo)
        {
            byte[] dados;
            var consultaVazia = new List<object> { new { Resultado = "Nenhum resultado encontrado para os filtros selecionados." } };
            AdicionarTotaisRodape(saldosGarantias);
            if (saldosGarantias.Count == 0)
                tipoProcesso = 0;

            var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
            {
                Delimiter = ";",
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                HasHeaderRecord = true,
                IgnoreBlankLines = true
            };

            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, csvConfiguration))
            {
                switch (tipoProcesso)
                {
                    case (long)TipoProcessoEnum.Trabalhista:
                        csv.WriteRecords(mapper.Map<ICollection<SaldoGarantiaExportacaoTrabalhistaViewModel>>(saldosGarantias));
                        break;
                    case (long)TipoProcessoEnum.CivelConsumidor:
                        csv.WriteRecords(mapper.Map<ICollection<SaldoGarantiaExportacaoCCViewModel>>(saldosGarantias));
                        break;
                    case (long)TipoProcessoEnum.CivelEstrategico:
                        csv.WriteRecords(mapper.Map<ICollection<SaldoGarantiaExportacaoCEViewModel>>(saldosGarantias));
                        break;
                    case (long)TipoProcessoEnum.JuizadoEspecial:
                        csv.WriteRecords(mapper.Map<ICollection<SaldoGarantiaExportacaoJECViewModel>>(saldosGarantias));
                        break;
                    case (long)TipoProcessoEnum.TributarioAdministrativo:
                        csv.WriteRecords(mapper.Map<ICollection<SaldoGarantiaExportacaoTributarioADMViewModel>>(saldosGarantias));
                        break;
                    case (long)TipoProcessoEnum.TributarioJudicial:
                        csv.WriteRecords(mapper.Map<ICollection<SaldoGarantiaExportacaoTributarioJudicialViewModel>>(saldosGarantias));
                        break;
                    case 0:
                        csv.WriteRecords(consultaVazia);
                        break;
                }

                streamWriter.Flush();
                dados = memoryStream.ToArray();
            }

            var filePathCSV = Path.Combine(path, nomeArquivo + ".csv");
            File.WriteAllBytes(filePathCSV, dados);
            return filePathCSV;
        }

        private static void ZipparArquivo(string filePathCSV, string filePathZIP)
        {
            var zip = new ZipFile();
            zip.Files.Add(filePathCSV);
            zip.Save(filePathZIP);
        }

        private static void AdicionarTotaisRodape(ICollection<SaldoGarantiaResultadoDTO> saldosGarantias)
        {
            var totais = new SaldoGarantiaResultadoDTO
            {
                //TODO: [MARCUS]
                //IdProcesso = "Total: ",
                NumeroProcesso = saldosGarantias.Count().ToString(),
                ValorPrincipal = saldosGarantias.Sum(a => a.ValorPrincipal),
                ValorCorrecaoPrincipal = saldosGarantias.Sum(a => a.ValorCorrecaoPrincipal),
                ValorAjusteCorrecao = saldosGarantias.Sum(a => a.ValorAjusteCorrecao),
                ValorJurosPrincipal = saldosGarantias.Sum(a => a.ValorJurosPrincipal),
                ValorAjusteJuros = saldosGarantias.Sum(a => a.ValorAjusteJuros),
                ValorPagamentoPrincipal = saldosGarantias.Sum(a => a.ValorPagamentoPrincipal),
                ValorPagamentoCorrecao = saldosGarantias.Sum(a => a.ValorPagamentoCorrecao),
                ValorPagamentosJuros = saldosGarantias.Sum(a => a.ValorPagamentosJuros),
                ValorLevantadoPrincipal = saldosGarantias.Sum(a => a.ValorLevantadoPrincipal),
                ValorLevantadoCorrecao = saldosGarantias.Sum(a => a.ValorLevantadoCorrecao),
                ValorLevantadoJuros = saldosGarantias.Sum(a => a.ValorLevantadoJuros),
                ValorSaldoPrincipal = saldosGarantias.Sum(a => a.ValorSaldoPrincipal),
                ValorSaldoCorrecao = saldosGarantias.Sum(a => a.ValorSaldoCorrecao),
                ValorSaldoJuros = saldosGarantias.Sum(a => a.ValorSaldoJuros),
                ValorTotalPagoGarantia = saldosGarantias.Sum(a => a.ValorTotalPagoGarantia),
                SaldoDepositoBloqueio = saldosGarantias.Sum(a => a.SaldoDepositoBloqueio),
                SaldoGarantia = saldosGarantias.Sum(a => a.SaldoGarantia),
            };
            saldosGarantias.Add(null);
            saldosGarantias.Add(totais);
        }

        public async Task<IResultadoApplication<ICollection<KeyValuePair<string, string>>>> ConsultarCriteriosPesquisa(long codigoAgendamento)
        {
            var result = new ResultadoApplication<ICollection<KeyValuePair<string, string>>>();

            try
            {

                var model = await service.ConsultarCriteriosPesquisa(codigoAgendamento);

                result.DefinirData(model);
                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;
        }

        public async Task<IResultadoApplication> SalvarAgendamento(SaldoGarantiaAgendamentoDTO filtroDTO)
        {
            var result = new ResultadoApplication();
            try
            {
                await service.SalvarAgendamento(filtroDTO);

                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Inclusao);
                result.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                result.ExecutadoComErro(ex);
            }
            return result;

        }

        public async Task<IResultadoApplication> ExcluirAgendamento(long codigoAgendamento)
        {
            var resultado = new ResultadoApplication();

            try
            {
                var agendamento = await service.RecuperarAgendamento(codigoAgendamento);
                await service.Remover(agendamento);

                service.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();
            }
            catch (Exception ex)
            {
                resultado.ExibirMensagem(ex.Message);
                resultado.ExecutadoComErro(ex);
                resultado.ExibeNotificacao = true;
            }
            return resultado;
        }

        public async Task<Stream> DownloadSaldoGarantia(string fileName)
        {
            try
            {
                var filePath = ObterFilePathNas(fileName);
                if (!File.Exists(filePath))
                    return null;
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return memory;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public string ObterFilePathNas(string nomeDoArquivo)
        {
            var path = parametroRepository.RecuperarPorId("DIR_WEB_AGEND_SAL_GAR").Result.Conteudo;
#if DEBUG
            path = "C:\\Temp\\AGENDAMENTO_SALDO_GARANTIA";
#endif                
            return Path.Combine(path, nomeDoArquivo);
        }
    }
}