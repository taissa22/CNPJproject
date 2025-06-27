using AutoMapper;
using CsvHelper;
using CsvHelper.TypeConversion;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.Interface;
using Perlink.Oi.Juridico.Application.ApuracaoOutliers.ViewModel;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Entity;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Enum;
using Perlink.Oi.Juridico.Domain.ApuracaoOutliers.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Shared.Application.Impl;
using Shared.Application.Interface;
using Shared.Domain.Impl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Application.ApuracaoOutliers.Impl
{
    public class ApuracaoOutliersAppService : BaseCrudAppService<AgendarApuracaoOutliersViewModel, AgendarApuracaoOutliers, long>, IApuracaoOutliersAppService
    {

        public readonly IMapper mapper;
        public readonly IBaseFechamentoJecCompletaService baseFechamentoJecCompletaService;
        public readonly IAgendarApuracaoOutlierService agendarApuracaoOutlierService;
        public readonly IUsuarioService usuarioService;
        private readonly IParametroService parametroService;
        private readonly INasService nasService;

        public ApuracaoOutliersAppService(IAgendarApuracaoOutlierService agendarApuracaoOutlierService, IBaseFechamentoJecCompletaService baseFechamentoJecCompletaService, IMapper mapper, IUsuarioService usuarioService, IParametroService parametroService) : base(agendarApuracaoOutlierService, mapper)
        {
            this.mapper = mapper;
            this.baseFechamentoJecCompletaService = baseFechamentoJecCompletaService;
            this.agendarApuracaoOutlierService = agendarApuracaoOutlierService;
            this.usuarioService = usuarioService;
            this.parametroService = parametroService;
        }

        public async Task<IResultadoApplication<ICollection<FechamentoJecDisponivelViewModel>>> CarregarFechamentosDisponiveisParaAgendamento()
        {
            var resultado = new ResultadoApplication<ICollection<FechamentoJecDisponivelViewModel>>();
            try
            {
                var model = await baseFechamentoJecCompletaService.CarregarFechamentosDisponiveisParaAgendamento();
                var data = mapper.Map<ICollection<FechamentoJecDisponivelViewModel>>(model);

                resultado.DefinirData(data);
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();

            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        public async Task<IResultadoApplication> RemoverAgendamento(long id)
        {
            var resultado = new ResultadoApplication<AgendarApuracaoOutliersViewModel>();

            try
            {
                var agendamento = await agendarApuracaoOutlierService.RecuperarPorId(id);

                if (agendamento == null)
                    throw new Exception(Textos.Mensagem_Erro_Agendamento_Nao_Encontrado);

                if (!(agendamento.Status.Equals(AgendarApuracaoOutliersStatusEnum.Agendado)) && !(agendamento.Status.Equals(AgendarApuracaoOutliersStatusEnum.Erro)))
                    throw new Exception(Textos.Mensagem_Erro_Ao_Excluir_Agendamento_Status_Diferente_Agendado);

                agendarApuracaoOutlierService.RemoverAgendamento(agendamento);
                agendarApuracaoOutlierService.Commit();

                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso_Exclusao);
                resultado.ExecutadoComSuccesso();

            }
            catch (Exception ex)
            {
                resultado.ExecutadoComErro(ex);
            }
            return resultado;
        }

        public async Task<IResultadoApplication> AgendarApuracaoOutliers(AgendarApuracaoOutliersDTO data)
        {
            var result = new ResultadoApplication();
            var entidade = mapper.Map<AgendarApuracaoOutliers>(data);
            var nomeUsuario = usuarioService.ObterUsuarioLogado().Result.Nome;
            try
            {
                if (entidade.FatorDesvioPadrao <= 0)
                    throw new Exception("O campo Fator do desvio padrão é obrigatorio");
                if (!decimal.TryParse(entidade.FatorDesvioPadrao.ToString(), out decimal num))
                    throw new Exception(string.Format("Valor {0} do campo Fator do desvio padrão incorreto", entidade.FatorDesvioPadrao));

                if (entidade.FatorDesvioPadrao.ToString().Replace(",", "").Length > 5)
                    throw new Exception("Valor do Fator do desvio padrão não aceito. Definição no banco decimal(5,2)");

                var FatorDesvioPadraoSplit = entidade.FatorDesvioPadrao.ToString().Split(',');
                if (FatorDesvioPadraoSplit[0].Length == 3 && FatorDesvioPadraoSplit[1].Length > 0)
                    throw new Exception("Valor do Fator do desvio padrão não aceito. Definição no banco decimal(5,2)");
                if (FatorDesvioPadraoSplit[0].Length > 3)
                    throw new Exception("Valor do Fator do desvio padrão não aceito. Definição no banco decimal(5,2)");
                if (FatorDesvioPadraoSplit[1].Length > 2)
                    throw new Exception("Valor do Fator do desvio padrão não aceito. Definição no banco decimal(5,2)");

                var obj = await agendarApuracaoOutlierService.AgendarApuracaoOutliers(entidade);
                
                obj.NomeUsuario = nomeUsuario;
                obj.DataSolicitacao = DateTime.Now;
                agendarApuracaoOutlierService.Commit();

                result.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                result.ExecutadoComSuccesso();
            }
            catch (Exception e)
            {
                result.ExecutadoComErro(e);
            }
            return result;
        }

        public async Task<IPagingResultadoApplication<ICollection<AgendarApuracaoOutliersViewModel>>> CarregarAgendamento(int pagina, int qtd)
        {
            var resultado = new PagingResultadoApplication<ICollection<AgendarApuracaoOutliersViewModel>>();

            try
            {
                var model = await agendarApuracaoOutlierService.CarregarAgendamento(pagina, qtd);
                var data = mapper.Map<ICollection<AgendarApuracaoOutliersViewModel>>(model);

                resultado.DefinirData(data);
                resultado.Total = await agendarApuracaoOutlierService.ObterQuantidadeTotal();
                resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
                resultado.ExecutadoComSuccesso();

            }
            catch (Exception ex)
            {

                resultado.ExecutadoComErro(ex);
            }

            return resultado;
        }

        #region Executor
        string nomeAgendamento = string.Empty;        
        public async Task ExecutarAgendamentos(ILogger logger)
        {
            logger.LogInformation("Tratando agendamentos interrompidos.");
            await agendarApuracaoOutlierService.TratandoAgendamentosInterrompidos();

            logger.LogInformation("Buscando agendamentos ainda não executados");
            var agendamentos = agendarApuracaoOutlierService.ObterAgendados().Result;
            logger.LogInformation($"Foram encontrados \"{agendamentos.Count}\" agendamentos para execução.");

            foreach (var item in agendamentos)
            {
                try
                {
                    
                    nomeAgendamento = string.Format("{0}/{1} - {2} - {3} Meses {4} / Fator de desvio padrão: {4}", item.DataFechamento.ToString("MM"), item.DataFechamento.Year, item.DataFechamento.ToShortDateString(), item.FechamentosProcessosJEC.NumeroMeses, item.FatorDesvioPadrao,item.FechamentosProcessosJEC.IndicaFechamentoMes? "- Fechamento Mensal" : "");

                    item.Status = AgendarApuracaoOutliersStatusEnum.Processando;
                    await agendarApuracaoOutlierService.Atualizar(item);
                    agendarApuracaoOutlierService.Commit();
                    logger.LogInformation($"Agendamento \"{nomeAgendamento}\" ({item.Id}) em processamento.");

                    logger.LogInformation($"Início Execução procedure AO_CALCULAR_BASE_FECHAM_JEC");
                    await agendarApuracaoOutlierService.RealizarCalculos(item);
                    logger.LogInformation($"Fim Execução procedure AO_CALCULAR_BASE_FECHAM_JEC");
                    
                    logger.LogInformation($"Criando CSV base de fechamento completa do agendamento");
                    await ExecutarArquivoBaseFechamento(item, logger);
                    logger.LogInformation($"Criado CSV base de fechamento completa do agendamento");

                    logger.LogInformation($"Criando CSV do resultado");
                    await ExecutarArquivoResultado(item, logger);
                    logger.LogInformation($"Criado CSV do resultado");

                    item.Status = AgendarApuracaoOutliersStatusEnum.Finalizado;
                    item.DataFinalizacao = DateTime.Now;
                    await agendarApuracaoOutlierService.Atualizar(item);
                    agendarApuracaoOutlierService.Commit();
                    logger.LogInformation($"Agendamento \"{nomeAgendamento}\" ({item.Id}) Finalizado.\n");
                }
                catch (Exception ex)
                {
                    logger.LogInformation($"Erro ao executar o agendamento: \"{nomeAgendamento}\"");
                    item.Status = AgendarApuracaoOutliersStatusEnum.Erro;
                    item.MgsStatusErro = ex.Message;
                    await agendarApuracaoOutlierService.Atualizar(item);
                    agendarApuracaoOutlierService.Commit();
                }
            }
        }
        private async Task<string> ExecutarArquivoBaseFechamento(AgendarApuracaoOutliers agendamento, ILogger logger)
        {
            var resultado = "";
            try
            {
                var nomeArquivo = $"Base_Fechamento_Val_Corte_Outlier_{Convert.ToDateTime(agendamento.DataFechamento).ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.csv";

                var caminhoDoArquivo = parametroService.RecuperarPorNome("DIR_NAS_CALC_OUTLIERS_JEC").Conteudo;
#if DEBUG
                caminhoDoArquivo = "C:\\Temp\\APURACAOOUTLIERS";
#endif

                if (!Directory.Exists(string.Concat(caminhoDoArquivo)))
                {
                    Directory.CreateDirectory(string.Concat(caminhoDoArquivo));
                }

                var lista = await baseFechamentoJecCompletaService.ListarBaseFechamento(agendamento.CodigoEmpresaCentralizadora, agendamento.MesAnoFechamento, agendamento.DataFechamento);

                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };

                using (FileStream fs = File.Create(Path.Combine(caminhoDoArquivo, nomeArquivo)))
                {
                    using (var writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(writer, csvConfiguration))
                        {
                            var retorno = mapper.Map<ICollection<ApuracaoOutliersDownloadBaseFechamentoViewModel>>(lista);
                            var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy" } };
                            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(options);
                            csv.WriteRecords(retorno);                           

                            agendamento.ArquivoBaseFechamento = nomeArquivo;

                            await agendarApuracaoOutlierService.Atualizar(agendamento);
                            agendarApuracaoOutlierService.Commit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                resultado = ex.Message;
            }
            return resultado;
        }

        private async Task<string> ExecutarArquivoResultado(AgendarApuracaoOutliers agendamento, ILogger logger)
        {
            var resultado = "";
            try
            {
                var nomeArquivo = $"Valor_Corte_Outlier_{Convert.ToDateTime(agendamento.DataFechamento).ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.csv";

                var caminhoDoArquivo = parametroService.RecuperarPorNome("DIR_NAS_CALC_OUTLIERS_JEC").Conteudo;
#if DEBUG
                caminhoDoArquivo = "C:\\Temp\\APURACAOOUTLIERS";
#endif  
                if (!Directory.Exists(string.Concat(caminhoDoArquivo)))
                {
                    Directory.CreateDirectory(string.Concat(caminhoDoArquivo));
                }

                var listaProcessos = await baseFechamentoJecCompletaService.ListarProcessosResultado(agendamento.CodigoEmpresaCentralizadora, agendamento.MesAnoFechamento, agendamento.DataFechamento);                
                var primeiraLinha = true;
                var listaResultado = new List<ApuracaoOutliersDownloadResultadoViewModel>();
                ApuracaoOutliersDownloadResultadoViewModel ApuracaoOutliersDownloadResultadoViewModel;
                foreach (var dadosProcesso in listaProcessos) {
                    if (primeiraLinha) {
                        ApuracaoOutliersDownloadResultadoViewModel = new ApuracaoOutliersDownloadResultadoViewModel() {
                            CodigoProcesso = dadosProcesso.CodigoProcesso.ToString(),
                            TotalPagamentos = dadosProcesso.TotalPagamentos.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                            DesvioPadrao = agendamento.ValorDesvioPadrao.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                            MediaPagamentos = agendamento.ValorMedia.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                            FatorDesvioPadrao = agendamento.FatorDesvioPadrao.ToString(),
                            QtdProcessosPagamentos = agendamento.QtdProcessos.ToString().Replace(",00 ", " "),
                            ValorCorteOutliers = agendamento.ValorCorteOutliers.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR")),
                            ValorTotalPagamentos = agendamento.ValorTotalProcessos.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))
                        };
                        listaResultado.Add(ApuracaoOutliersDownloadResultadoViewModel);
                        primeiraLinha = false;
                    }else {
                        ApuracaoOutliersDownloadResultadoViewModel = new ApuracaoOutliersDownloadResultadoViewModel() {
                            CodigoProcesso = dadosProcesso.CodigoProcesso.ToString(),
                            TotalPagamentos = dadosProcesso.TotalPagamentos.ToString("N", CultureInfo.CreateSpecificCulture("pt-BR"))
                        };
                        listaResultado.Add(ApuracaoOutliersDownloadResultadoViewModel);
                    }
                }

                var csvConfiguration = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.GetCultureInfo("pt-BR"))
                {
                    Delimiter = ";",
                    TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                    HasHeaderRecord = true,
                    IgnoreBlankLines = true
                };

                using (FileStream fs = File.Create(Path.Combine(caminhoDoArquivo, nomeArquivo)))
                {
                    using (var writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(writer, csvConfiguration))
                        {
                            var retorno = listaResultado;
                            var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy" } };
                            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                            csv.Context.TypeConverterOptionsCache.AddOptions<DateTime?>(options);
                            csv.WriteRecords(retorno);                            

                            agendamento.ArquivoResultado = nomeArquivo;

                            await agendarApuracaoOutlierService.Atualizar(agendamento);
                            agendarApuracaoOutlierService.Commit();
                        }
                    }
                }
                CriandoPDF(agendamento, nomeArquivo);
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
                resultado = ex.Message;
            }
            return resultado;
        }

        private void CriandoPDF(AgendarApuracaoOutliers agendamento, string nomeArquivo)
        {
            Document doc = new Document(PageSize.A4);
            doc.AddCreationDate();

            var nomePDF = $"Resumo_Valor_Corte_Outlier_{Convert.ToDateTime(agendamento.DataFechamento).ToString("yyyyMMdd", CultureInfo.InvariantCulture)}.pdf";

            var caminho = parametroService.RecuperarPorNome("DIR_NAS_CALC_OUTLIERS_JEC").Conteudo;
#if DEBUG
            caminho = "C:\\Temp\\APURACAOOUTLIERS";
#endif
            PdfWriter writer = PdfWriter.GetInstance(doc, new  FileStream(Path.Combine(caminho, nomePDF), FileMode.Create));

            doc.Open();

            Paragraph titulo = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 14, (int)System.Drawing.FontStyle.Bold));
            titulo.Alignment = Element.ALIGN_CENTER;
            titulo.Add($"Resumo apuração valor de corte de outliers - Fechamento {agendamento.DataFechamento.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)}");
            doc.Add(titulo);

            doc.Add(new Paragraph(" "));
            doc.Add(new Paragraph(" "));

            Paragraph paragrafoNomeArquivo = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12, (int)System.Drawing.FontStyle.Regular));
            paragrafoNomeArquivo.Alignment = Element.ALIGN_LEFT;
            paragrafoNomeArquivo.Add("Nome do arquivo:                                   ");
            
            paragrafoNomeArquivo.Add(nomeArquivo);
            doc.Add(paragrafoNomeArquivo);

            Paragraph paragrafoData = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12, (int)System.Drawing.FontStyle.Regular));
            paragrafoData.Alignment = Element.ALIGN_LEFT;
            paragrafoData.Add("Data e hora da geração do arquivo:       ");

            var data = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            paragrafoData.Add(data);
            doc.Add(paragrafoData);

            Paragraph paragrafoFator = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12, (int)System.Drawing.FontStyle.Regular));
            paragrafoFator.Alignment = Element.ALIGN_LEFT;
            paragrafoFator.Add("Fator de desvio padrão:                          ");

            paragrafoFator.Add(agendamento.FatorDesvioPadrao.ToString());
            doc.Add(paragrafoFator);

            Paragraph paragrafoProcessos = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12, (int)System.Drawing.FontStyle.Regular));
            paragrafoProcessos.Alignment = Element.ALIGN_LEFT;
            paragrafoProcessos.Add("Quantidades de processos:                    ");

            paragrafoProcessos.Add(agendamento.QtdProcessos.ToString());
            doc.Add(paragrafoProcessos);

            Paragraph paragrafoPagamentos = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12, (int)System.Drawing.FontStyle.Regular)); 
            paragrafoPagamentos.Alignment = Element.ALIGN_LEFT;
            paragrafoPagamentos.Add("Total de pagamentos:                             ");

            paragrafoPagamentos.Add(agendamento.ValorTotalProcessos.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")));
            doc.Add(paragrafoPagamentos);
     
            Paragraph paragrafoValorC = new Paragraph("", new Font(Font.FontFamily.HELVETICA, 12, (int)System.Drawing.FontStyle.Regular));
            paragrafoValorC.Alignment = Element.ALIGN_LEFT;
            paragrafoValorC.Add("Valor de corte de outliers:                       ");

            paragrafoValorC.Add(agendamento.ValorCorteOutliers.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")));
            doc.Add(paragrafoValorC);

            doc.Close();

            var listaArquivos = new List<string>();

            listaArquivos.Add(nomePDF);
            listaArquivos.Add(agendamento.ArquivoResultado);

            var nomeArquivoZip = $"{Convert.ToDateTime(agendamento.DataFechamento).ToString("yyyyMMdd", CultureInfo.InvariantCulture)}" +
                                               $"_Val_Corte_Outlier" +
                                               $"_{DateTime.Now:yyyyMMdd_HHmmss}.zip";

            agendamento.ArquivoResultado = nomeArquivoZip;
            agendarApuracaoOutlierService.Atualizar(agendamento);
            agendarApuracaoOutlierService.Commit();

            CriandoZip(caminho, listaArquivos, nomeArquivoZip);
        }

        private static void CriandoZip(string path, List<string> lista, string criandoNomePastaZip)
        {
            using (var stream = File.OpenWrite(string.Concat(path, "\\", criandoNomePastaZip)))
            using (ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                foreach (var item in lista)
                {
                    archive.CreateEntryFromFile(String.Concat(path, "\\", item), item, CompressionLevel.Optimal);
                }
            }
        }

        #endregion Executor

        //public async Task<IResultadoApplication<string>> BaixarArquivosResultado(int id)
        //{
        //    var caminhoArquivo =  parametroService.RecuperarPorNome("DIR_NAS_CALC_OUTLIERS_JEC");
        //    var resultado = new ResultadoApplication<string>();

        //    try
        //    {
        //        var model = await agendarApuracaoOutlierService.RecuperarPorId(id);                

        //        resultado.DefinirData(string.Format("{0}{1}{2}", "", caminhoArquivo, model.ArquivoBaseFechamento));
        //        resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
        //        resultado.ExecutadoComSuccesso();
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado.ExecutadoComErro(ex);
        //    }
        //    return resultado;
        //}

        //public async Task<IResultadoApplication<ApuracaoOutlierDownloadArquivoViewModel>> DownloadBaseFechamento(int id)
        //{
        //    var resultado = new ResultadoApplication<ApuracaoOutlierDownloadArquivoViewModel>();

        //    try
        //    {
        //        var viewModel = await agendarApuracaoOutlierService.DownloadBaseFechamento(id);
        //        var data = mapper.Map<ApuracaoOutlierDownloadArquivoViewModel>(viewModel);

        //        resultado.DefinirData(data);
        //        resultado.ExibirMensagem(Textos.Geral_Mensagem_Sucesso);
        //        resultado.ExecutadoComSuccesso();
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado.ExecutadoComErro(ex);
        //    }
        //    return resultado;
        //}

        public async Task<Stream> Download(string fileName) {
            try {
                var filePath = ObterFilePathNas(fileName);
                if (!File.Exists(filePath))
                    return null;
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open)) {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return memory;
            } catch (Exception e) {
                throw;
            }
        }

        public string ObterFilePathNas(string nomeDoArquivo) {
            var path = parametroService.RecuperarPorId("DIR_NAS_CALC_OUTLIERS_JEC").Result.Conteudo;
              
            return Path.Combine(path, nomeDoArquivo);
        }
    }
}
