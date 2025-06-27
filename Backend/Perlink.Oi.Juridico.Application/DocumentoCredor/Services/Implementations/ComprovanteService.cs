using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Handlers;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using StatusAgendamento = Perlink.Oi.Juridico.Infra.Enums.StatusAgendamento;
using TipoDocumentoLancamentoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoDocumentoLancamento;
using TipoProcessoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoProcesso;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services.Implementations {

    internal class ComprovanteService : IComprovanteService {
        private const int COMPROMISSO_CLIENTE_INDEX = 23;
        private const string STATUS_CARREGADO = "Carregado";
        private const string STATUS_NAO_CARREGADO = "Nao carregado";

        private IDatabaseContext DatabaseContext { get; }
        private IPdfHandler PdfHandler { get; }
        private ILogger<IComprovanteService> Logger { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }
        private IFileHandler FileHandler { get; }
        private IAgendamentoCargaComprovanteService AgendamentoCargaComprovante { get; }

        public ComprovanteService(IDatabaseContext databaseContext, IPdfHandler pdfHandler, ILogger<IComprovanteService> logger, IParametroJuridicoProvider parametroJuridico, IFileHandler fileHandler, IAgendamentoCargaComprovanteService agendamentoCargaComprovante) {
            DatabaseContext = databaseContext;
            PdfHandler = pdfHandler;
            Logger = logger;
            ParametroJuridico = parametroJuridico;
            FileHandler = fileHandler;
            AgendamentoCargaComprovante = agendamentoCargaComprovante;
        }

        public CommandResult ExecutarRotinaPeloNAS(string arquivoPDFComprovantes, string arquivoBaseSAP) {
            string entity = "Carga Comprovante";
            string command = $"Executa Agendamento {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
            var parametroCaminhoNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE).Dequeue();

            if (!Directory.Exists(parametroCaminhoNasCargaComprovantes)) {
                Directory.CreateDirectory(parametroCaminhoNasCargaComprovantes);
            }

            Log.Logger.Information(Infra.Extensions.Logs.Obtendo(entity));
            var listaAgendamentos = DatabaseContext.AgendamentosCargasComprovantes
                                                    .Where(l => l.StatusAgendamentoId == StatusAgendamento.AGENDADO.Id || l.StatusAgendamentoId == StatusAgendamento.EM_EXECUCAO.Id)
                                                    .OrderBy(o => o.DataAgendamento).ThenBy(o => o.Id);

            try {

                string caminhoArquivoComprovante = string.Empty;
                string caminhoArquivoBaseSAP = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Comprovantes"));

                if (!Directory.Exists(parametroCaminhoNasCargaComprovantes)) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    Log.Logger.Error("Diretório Nas não encontrado");
                    throw new Exception("Diretório Nas não encontrado");
                }

                string arquivoComprovantes = Path.Combine(parametroCaminhoNasCargaComprovantes, arquivoPDFComprovantes);
                if (File.Exists(arquivoComprovantes)) {
                    caminhoArquivoComprovante = arquivoComprovantes;
                }

                string arquivoBaseSapTemp = Path.Combine(parametroCaminhoNasCargaComprovantes, arquivoBaseSAP);
                if (File.Exists(arquivoBaseSapTemp)) {
                    caminhoArquivoBaseSAP = arquivoBaseSapTemp;
                }

                if (string.IsNullOrEmpty(caminhoArquivoBaseSAP) || string.IsNullOrEmpty(caminhoArquivoComprovante)) {
                    string mensagem = $"Arquivos inválidos";
                    Logger.LogInformation(mensagem);
                    throw new Exception(mensagem);
                }

                var arquivoZipado = GeraCargaArquivoComprovantes(caminhoArquivoComprovante, caminhoArquivoBaseSAP);

                if (!arquivoZipado.IsValid) {
                    Logger.LogInformation(arquivoZipado.Mensagens.ToString());
                    throw new Exception(arquivoZipado.Mensagens.ToString());
                }

                Random r = new Random();
                int idRandomico = r.Next(0, 1000000000);

                var processamentoInfo = ProcessaCargaComprovantes(arquivoZipado.Dados, idRandomico);

                if (!processamentoInfo.IsValid) {
                    Logger.LogInformation(string.Join(',', processamentoInfo.Mensagens));
                    throw new Exception(string.Join(',', processamentoInfo.Mensagens));
                }

            }
            catch (Exception ex) {
                string mensagemResuladoCarga = "Erro ao realizar carga de comprovantes";
                Log.Logger.Error($"{mensagemResuladoCarga}:{ex.Message}");
                Logger.LogError(ex, mensagemResuladoCarga);
                return CommandResult.Invalid(ex.Message);
            }

            return CommandResult.Valid();
        }

        public CommandResult ExecutarRotina(string arquivoPDFComprovantes, string arquivoBaseSAP) {
            string entity = "Carga Comprovante";
            string command = $"Executa Agendamento {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {

                string caminhoArquivoComprovante = string.Empty;
                string caminhoArquivoBaseSAP = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Comprovantes"));


                if (File.Exists(arquivoPDFComprovantes)) {
                    caminhoArquivoComprovante = arquivoPDFComprovantes;
                }

                if (File.Exists(arquivoBaseSAP)) {
                    caminhoArquivoBaseSAP = arquivoBaseSAP;
                }

                if (string.IsNullOrEmpty(caminhoArquivoBaseSAP) || string.IsNullOrEmpty(caminhoArquivoComprovante)) {
                    string mensagem = $"Arquivos inválidos";
                    Logger.LogInformation(mensagem);
                    throw new Exception(mensagem);
                }

                var arquivoZipado = GeraCargaArquivoComprovantes(caminhoArquivoComprovante, caminhoArquivoBaseSAP);

                if (!arquivoZipado.IsValid) {
                    Logger.LogInformation(string.Join(',', arquivoZipado.Mensagens));
                    throw new Exception(string.Join(',', arquivoZipado.Mensagens));
                }

                Random r = new Random();
                int idRandomico = r.Next(0, 1000000000);

                var processamentoInfo = ProcessaCargaComprovantes(arquivoZipado.Dados, idRandomico);

                if (!processamentoInfo.IsValid) {
                    Logger.LogInformation(string.Join(',', processamentoInfo.Mensagens));
                    throw new Exception(string.Join(',', processamentoInfo.Mensagens));
                }

            }
            catch (Exception ex) {
                string mensagemResuladoCarga = "Erro ao realizar carga de comprovantes";
                Log.Logger.Error($"{mensagemResuladoCarga}:{ex.Message}");
                Logger.LogError(ex, mensagemResuladoCarga);
                return CommandResult.Invalid(ex.Message);
            }

            return CommandResult.Valid();
        }

        public CommandResult ExecutarAgendamento() {
            string entity = "Carga Comprovante";
            string command = $"Executa Agendamento {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
            var parametroCaminhoNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE).Dequeue();

            if (!Directory.Exists(parametroCaminhoNasCargaComprovantes)) {
                Directory.CreateDirectory(parametroCaminhoNasCargaComprovantes);
            }

            Log.Logger.Information(Infra.Extensions.Logs.Obtendo(entity));
            var listaAgendamentos = DatabaseContext.AgendamentosCargasComprovantes
                                                    .Where(l => l.StatusAgendamentoId == StatusAgendamento.AGENDADO.Id || l.StatusAgendamentoId == StatusAgendamento.EM_EXECUCAO.Id)
                                                    .OrderBy(o => o.DataAgendamento).ThenBy(o => o.Id);

            foreach (var agendamento in listaAgendamentos) {
                try {
                    if (agendamento.StatusAgendamento.Id == StatusAgendamento.EM_EXECUCAO.Id) {
                        agendamento.AtualizarStatusAgendamento(StatusAgendamento.ERRO.Id);
                        DatabaseContext.SaveChanges();
                        continue;
                    }

                    agendamento.AtualizarStatusAgendamento(StatusAgendamento.EM_EXECUCAO.Id);

                    DatabaseContext.SaveChanges();

                    string caminhoArquivoComprovante = string.Empty;
                    string caminhoArquivoBaseSAP = string.Empty;

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Comprovantes"));

                    if (!Directory.Exists(parametroCaminhoNasCargaComprovantes)) {
                        Logger.LogInformation("Diretório Nas não encontrado");
                        Log.Logger.Error("Diretório Nas não encontrado");
                        continue;
                    }

                    string arquivoComprovantes = Path.Combine(parametroCaminhoNasCargaComprovantes, agendamento.NomeArquivoComprovante);
                    if (File.Exists(arquivoComprovantes)) {
                        caminhoArquivoComprovante = arquivoComprovantes;
                    }

                    string arquivoBaseSap = Path.Combine(parametroCaminhoNasCargaComprovantes, agendamento.NomeArquivoBaseSap);
                    if (File.Exists(arquivoBaseSap)) {
                        caminhoArquivoBaseSAP = arquivoBaseSap;
                    }

                    if (string.IsNullOrEmpty(caminhoArquivoBaseSAP) || string.IsNullOrEmpty(caminhoArquivoComprovante)) {
                        string mensagem = $"Arquivos inválidos";
                        Logger.LogInformation(mensagem);
                        throw new Exception(mensagem);
                    }

                    var arquivoZipado = GeraCargaArquivoComprovantes(caminhoArquivoComprovante, caminhoArquivoBaseSAP);

                    if (!arquivoZipado.IsValid) {
                        Logger.LogInformation(string.Join(',', arquivoZipado.Mensagens));
                        throw new Exception(string.Join(',', arquivoZipado.Mensagens));
                    }

                    var processamentoInfo = ProcessaCargaComprovantes(arquivoZipado.Dados, agendamento.Id);

                    if (!processamentoInfo.IsValid) {
                        Logger.LogInformation(processamentoInfo.Mensagens.ToString());
                        throw new Exception(processamentoInfo.Mensagens.ToString());
                    }

                    agendamento.AtualizarResultadoAgendamento(StatusAgendamento.EXECUTADO.Id, processamentoInfo.Dados.ArquivoRetorno, processamentoInfo.Dados.MensagemResultadoCarga);

                    DatabaseContext.SaveChanges();
                }
                catch (Exception ex) {
                    string mensagemResuladoCarga = "Erro ao realizar carga de comprovantes";
                    Log.Logger.Error($"{mensagemResuladoCarga}:{ex.Message}");
                    Logger.LogError(ex, mensagemResuladoCarga);
                    agendamento.AtualizarErroAgendamento(StatusAgendamento.ERRO.Id, ex.Message, ex.StackTrace, mensagemResuladoCarga);
                    DatabaseContext.SaveChanges();
                    return CommandResult.Invalid(ex.Message);
                }
            }

            return CommandResult.Valid();
        }

        public CommandResult<string> GeraCargaArquivoComprovantes(string pdfFile, string sapFbl3nFile) {
            string caminhoNas = Path.GetDirectoryName(pdfFile);
            var diretorioTemporario = Path.Combine(caminhoNas, Guid.NewGuid().ToString());

            List<string> pastasTemporarias = new List<string>();
            pastasTemporarias.Add(diretorioTemporario);
            File.AppendAllLines(Path.Combine(caminhoNas, $"PastasTemporarias.tmp"), pastasTemporarias);

            try {
                Logger.LogInformation($"Usando '{ diretorioTemporario }';");
                Directory.CreateDirectory(diretorioTemporario);

                Logger.LogInformation($"Dividindo o arquivo em páginas;");
                var resultadoSeparacao = PdfHandler.SplitPdfToPages(pdfFile, diretorioTemporario);
                if (!resultadoSeparacao.IsValid) {
                    Logger.LogError($"Divisão do arquivo não realizada;", resultadoSeparacao.Mensagens);
                    Log.Logger.Information($"Divisão do arquivo não realizada;", resultadoSeparacao.Mensagens);
                    return CommandResult<string>.Invalid(resultadoSeparacao.Mensagens);
                }

                Logger.LogInformation($"Buscando Informações no pdf;");
                var pdfInfoList = new List<PdfInfo>();
                foreach (var pdf in Directory.GetFiles(diretorioTemporario)) {
                    Logger.LogInformation($"Buscando informação em: { Path.GetFileName(pdf) };");
                    var extractResult = PdfHandler.ExtractWordFromPdf(pdf, COMPROMISSO_CLIENTE_INDEX);
                    if (extractResult.IsValid) {
                        Logger.LogInformation($"Informação encontrada;");
                        pdfInfoList.Add(new PdfInfo() {
                            CompromissoCliente = extractResult.Dados,
                            NomeDoArquivo = Path.GetFileName(pdf)
                        });
                        continue;
                    }
                    Logger.LogError($"Número do Compromisso do Cliente não encontrado em: { Path.GetFileName(pdf) };");
                    Log.Logger.Error($"Número do Compromisso do Cliente não encontrado em: { Path.GetFileName(pdf) } - PDF Inválido;");
                    return CommandResult<string>.Invalid($"PDF Inválido - { Path.GetFileName(pdf) }");
                }

                var dePara = new StringBuilder();
                dePara.AppendLine("COD_PROCESSO;SEQUENCIAL_LANCAMENTO;COD_TIPO_PROCESSO;NOME_ARQUIVO;NRO_DOC_COMPENSACAO;STATUS;OCORRENCIA");

                Log.Logger.Information($"Abrindo base SAP");
                int contadorPaginasProcessadas = 0;
                using (FastExcel.FastExcel sapFile = new FastExcel.FastExcel(new FileInfo(sapFbl3nFile), true)) {
                    FastExcel.Worksheet sap = sapFile.Read(1);

                    foreach (var pdfInfo in pdfInfoList) {
                        Logger.LogInformation($"Extraindo informações para Compromisso Cliente '{ pdfInfo.CompromissoCliente }', paginas processadas { contadorPaginasProcessadas++ }");
                        FastExcel.Row row = sap.Rows.FirstOrDefault(x => x.GetCellByColumnName("L").Value is string strCell && strCell == pdfInfo.CompromissoCliente);

                        if ((row is null)) {
                            Logger.LogWarning($"Compromisso do Cliente '{ pdfInfo.CompromissoCliente }' não encontrado no SAP;");

                            Logger.LogInformation($"Gravando informações no 'csv';");
                            dePara.AppendLine($"\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{pdfInfo.NomeDoArquivo}\";\"{pdfInfo.CompromissoCliente}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaComprovantes.NRO_COMPROMISSO_NAO_ENCONTRADO}\";");
                            continue;
                        }

                        var textoSap = (string)row.GetCellByColumnName("E").Value;

                        if (string.IsNullOrEmpty(textoSap)) {
                            Logger.LogInformation($"Gravando informações no 'csv';");
                            dePara.AppendLine($"\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{pdfInfo.NomeDoArquivo}\";\"{pdfInfo.CompromissoCliente}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaComprovantes.TEXTO_SAP_NAO_ENCONTRADO}\";");
                            continue;
                        }

                        var valorPago = (decimal)Math.Abs(double.Parse(((string)row.GetCellByColumnName("M").Value).Replace(".", ",")));

                        if (string.IsNullOrEmpty(valorPago.ToString())) {
                            Logger.LogInformation($"Gravando informações no 'csv';");
                            dePara.AppendLine($"\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{pdfInfo.NomeDoArquivo}\";\"{pdfInfo.CompromissoCliente}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaComprovantes.VALOR_PAGO_NAO_ENCONTRADO}\";");
                            continue;
                        }

                        Logger.LogInformation($"Buscando lote no SISJUR");
                        var loteId = DatabaseContext.Lotes
                            .Where(lote => lote.Borderos.Any(bordero => bordero.Comentario == textoSap && bordero.ValorPago == valorPago))
                            .Select(x => x.Id);

                        if (loteId.Count() > 1) {
                            Logger.LogInformation($"Gravando informações no 'csv';");
                            dePara.AppendLine($"\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{pdfInfo.NomeDoArquivo}\";\"{pdfInfo.CompromissoCliente}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaComprovantes.PAGAMENTO_NAO_ENCONTRADO_SISJUR}\";");
                            continue;
                        }

                        Logger.LogInformation($"Buscando informações do processo e lancamento no SISJUR;");
                        var lotesLancamentos = DatabaseContext.LotesLancamentos.Where(x => x.Lote.Id == loteId.FirstOrDefault());

                        List<Lancamento> listaLancamentos = new List<Lancamento>();

                        foreach (var registro in lotesLancamentos) {
                            var lancamentoProcesso = DatabaseContext.Lancamentos.Where(x => x.Processo.Id == registro.ProcessoId && x.Sequencial == registro.Lancamento.Sequencial && x.Valor == valorPago);
                            if (lancamentoProcesso != null) {
                                listaLancamentos.Add(lancamentoProcesso.FirstOrDefault());
                            }
                        }

                        if (listaLancamentos != null && listaLancamentos.Count > 0) {
                            foreach (var lancamento in listaLancamentos) {
                                Logger.LogInformation($"Gravando informações no 'csv';");
                                dePara.AppendLine($"\"{(lancamento != null ? lancamento.Processo.Id.ToString() : string.Empty)}\";\"{(lancamento != null ? lancamento.Sequencial.ToString() : string.Empty)}\";\"{(lancamento != null ? lancamento.Processo.TipoProcessoId.ToString() : string.Empty)}\";\"{pdfInfo.NomeDoArquivo}\";\"{pdfInfo.CompromissoCliente}\";\"{string.Empty}\";\"{string.Empty}\";");
                            }
                        }
                        else {
                            Logger.LogInformation($"Gravando informações no 'csv';");
                            dePara.AppendLine($"\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{pdfInfo.NomeDoArquivo}\";\"{pdfInfo.CompromissoCliente}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaComprovantes.PAGAMENTO_NAO_ENCONTRADO_SISJUR}\";");
                            continue;
                        }

                    }
                }

                Logger.LogInformation($"Gerando 'de_para.csv';");
                File.WriteAllText(Path.Combine(diretorioTemporario, $"de_para.csv"), dePara.ToString());

                var nomeArquivoCarga = Path.Combine(caminhoNas, "CARGA_COMPROVANTE_" + Guid.NewGuid().ToString() + ".zip");
                Logger.LogInformation($"Compactando arquivos para '{ Path.GetFileName(nomeArquivoCarga) }';");
                ZipFile.CreateFromDirectory(diretorioTemporario, nomeArquivoCarga, CompressionLevel.Fastest, false);

                List<string> zipTemporario = new List<string>();
                zipTemporario.Add(nomeArquivoCarga);
                File.AppendAllLines(Path.Combine(caminhoNas, $"ZipTemporario.tmp"), zipTemporario);

                Logger.LogInformation($"Limpando arquivos temporários;");
                Directory.Delete(diretorioTemporario, true);

                Logger.LogInformation($"Comando concluído;");
                return CommandResult<string>.Valid(nomeArquivoCarga);
            }
            catch (Exception ex) {
                string mensagemResultadoDePara = "Erro ao gerar arquivo DE x PARA";
                Logger.LogInformation($"Limpando arquivos temporários;");
                Log.Logger.Error($"{mensagemResultadoDePara}:{ex.Message}");
                Directory.Delete(diretorioTemporario, true);
                Logger.LogError(ex, mensagemResultadoDePara);
                throw;
            }
        }

        public CommandResult<ProcessamentoCargaInfo> ProcessaCargaComprovantes(string zipFile, int agendamentoId) {
            string caminhoNasCarga = Path.GetDirectoryName(zipFile);
            string parametroCaminhoNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMP_RESULT).Dequeue();
            string caminhoResultadoTemp = Path.Combine(caminhoNasCarga, "Resultado Carga");
            var diretorioTemporario = Path.Combine(Path.GetDirectoryName(zipFile), Guid.NewGuid().ToString());

            List<string> pastasTemporarias = new List<string>();
            pastasTemporarias.Add(diretorioTemporario);
            pastasTemporarias.Add(caminhoResultadoTemp);
            File.AppendAllLines(Path.Combine(caminhoNasCarga, $"PastasTemporarias.tmp"), pastasTemporarias);

            if (!Directory.Exists(parametroCaminhoNasCargaComprovantes)) {
                Directory.CreateDirectory(parametroCaminhoNasCargaComprovantes);
            }

            ProcessamentoCargaInfo resultadoInfo = new ProcessamentoCargaInfo() {
                ArquivoRetorno = string.Empty,
                MensagemResultadoCarga = string.Empty
            };

            try {
                Directory.CreateDirectory(caminhoNasCarga);
                Directory.CreateDirectory(caminhoResultadoTemp);

                int contadorSucesso = 0;
                int contadorFalha = 0;

                Directory.CreateDirectory(diretorioTemporario);
                Logger.LogInformation($"Usando '{ diretorioTemporario }';");

                Logger.LogInformation($"Extraindo arquivo;");
                ZipFile.ExtractToDirectory(zipFile, diretorioTemporario);

                var parametroTipoDocumento = ParametroJuridico.Obter(ParametrosJuridicos.TP_DOCUMENTO_CREDOR).Dados.Conteudo;

                Usuario usuario = DatabaseContext.Usuarios.FirstOrDefault(x => x.Id == "SISJUR_JOB");

                Logger.LogInformation($"Iniciando leitura do 'de_para.csv'");

                var relatorio = new StringBuilder();
                relatorio.AppendLine("Nro doc. Compensacao;Codigo interno do processo;Numero do processo;Tipo processo;Seq. Lancamento;Valor;Credor;Data do pagamento;Nome do Comprovante;Status;Ocorrencia");

                using (var fs = File.OpenRead(Path.Combine(diretorioTemporario, "de_para.csv"))) {
                    using (var reader = new StreamReader(fs)) {
                        // ignora o header;
                        reader.ReadLine();

                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            var cells = line.Replace("\"", "").Split(";");

                            string numeroCompromissoCliente = cells[4];
                            string nomeDoArquivo = cells[3];

                            string comprovanteTemporario = Path.Combine(diretorioTemporario, nomeDoArquivo);
                            string comprovanteResultadoTemp = Path.Combine(caminhoResultadoTemp, nomeDoArquivo);

                            if (!string.IsNullOrEmpty(cells[6])) {
                                relatorio.AppendLine($"\"{numeroCompromissoCliente}\";\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{string.Empty}\";\"{nomeDoArquivo}\";\"{cells[5]}\";\"{cells[6]}\";");
                                File.Copy(
                                    comprovanteTemporario,
                                    comprovanteResultadoTemp, true);
                                contadorFalha++;
                                continue;
                            }

                            if (!string.IsNullOrEmpty(cells[0]) && !string.IsNullOrEmpty(cells[1])
                                && !string.IsNullOrEmpty(cells[2]) && !string.IsNullOrEmpty(cells[3])) {
                                int processoId = int.Parse(cells[0]);
                                int sequencialLancamento = int.Parse(cells[1]);
                                TipoProcessoEnum tipoProcesso = TipoProcessoEnum.PorId(int.Parse(cells[2]));

                                Logger.LogInformation($"Buscando processo");
                                var processo = DatabaseContext.Processos.FirstOrDefault(x => x.Id == processoId);

                                Logger.LogInformation($"Buscando lancamento;");
                                var lancamento = DatabaseContext.Lancamentos.FirstOrDefault(x => x.Processo == processo && x.Sequencial == sequencialLancamento);

                                string ocorrencia = string.Empty;

                                Logger.LogInformation($"Processo '{ tipoProcesso.Nome }' encontrado;");
                                if (tipoProcesso == TipoProcessoEnum.JEC || tipoProcesso == TipoProcessoEnum.CIVEL_CONSUMIDOR || tipoProcesso == TipoProcessoEnum.PEX) {

                                    dynamic documento = null;
                                    bool ehPex = tipoProcesso == TipoProcessoEnum.PEX;

                                    if (ehPex) {
                                        documento = DatabaseContext.DocumentosQuitacoesLancamentos.FirstOrDefault(x => x.Processo == processo && x.Lancamento == lancamento);
                                    }
                                    else {
                                        documento = DatabaseContext.DocumentosLancamentos.FirstOrDefault(x => x.Processo == processo && x.Lancamento == lancamento && x.TipoDocumentoId == TipoDocumentoLancamentoEnum.COMPROVANTE_DE_PAGAMENTO.Id);
                                    }

                                    if (documento != null) {
                                        Logger.LogInformation($"Atualizando Comprovante do Processo;");
                                        var resultado = SalvaAnexoNAS(tipoProcesso, documento, comprovanteTemporario, nomeDoArquivo);

                                        if (!resultado.IsValid) {
                                            string parteNome = lancamento.Parte != null ? lancamento.Parte.Nome : string.Empty;
                                            relatorio.AppendLine($"\"{numeroCompromissoCliente}\";\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{TipoProcessoEnum.PorId(processo.TipoProcessoId).Nome}\";\"{lancamento.Sequencial}\";\"{lancamento.Valor}\";\"{parteNome}\";\"{lancamento.DataPagamento}\";\"{nomeDoArquivo}\";\"{STATUS_NAO_CARREGADO}\";\"Falha ao anexar comprovante no banco de dados\";");
                                            File.Copy(
                                                comprovanteTemporario,
                                                comprovanteResultadoTemp, true);
                                            contadorFalha++;
                                            continue;
                                        }

                                        string nomeArquivoBanco = $"{Path.GetFileNameWithoutExtension(nomeDoArquivo)}.zip";

                                        if (ehPex) {
                                            nomeArquivoBanco = $"{GerarHashNomeArquivo()}#{nomeArquivoBanco}";
                                        }

                                        documento.AtulizarDocumentoAnexado(nomeArquivoBanco, usuario);

                                        ocorrencia = "Comprovante substituido";
                                    }
                                    else {
                                        Logger.LogInformation($"Criando Documento Lancamento do Processo;");
                                        if (ehPex) {
                                            documento = DocumentoQuitacaoLancamento.Criar(processo, nomeDoArquivo, lancamento, usuario);
                                            var autoDocumentoGEDId = DatabaseContext.GetNext("JUR", "GED_SEQ_01", documento);
                                            documento.AtualizarAutoDocumentoGed(autoDocumentoGEDId);
                                        }
                                        else {
                                            documento = DocumentoLancamento.Criar(processo, TipoDocumentoLancamentoEnum.COMPROVANTE_DE_PAGAMENTO, nomeDoArquivo, lancamento, usuario);
                                        }

                                        DatabaseContext.Add(documento);
                                        DatabaseContext.SaveChanges();

                                        var resultado = SalvaAnexoNAS(tipoProcesso, documento, comprovanteTemporario, nomeDoArquivo);

                                        if (!resultado.IsValid) {
                                            string parteNome = lancamento.Parte != null ? lancamento.Parte.Nome : string.Empty;
                                            relatorio.AppendLine($"\"{numeroCompromissoCliente}\";\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{TipoProcessoEnum.PorId(processo.TipoProcessoId).Nome}\";\"{lancamento.Sequencial}\";\"{lancamento.Valor}\";\"{parteNome}\";\"{lancamento.DataPagamento}\";\"{nomeDoArquivo}\";\"{STATUS_NAO_CARREGADO}\";\"Falha ao anexar comprovante no banco de dados\";");
                                            File.Copy(
                                                comprovanteTemporario,
                                                comprovanteResultadoTemp, true);
                                            contadorFalha++;
                                            continue;
                                        }

                                        string nomeArquivoBanco = $"{Path.GetFileNameWithoutExtension(nomeDoArquivo)}.zip";

                                        if (ehPex) {
                                            nomeArquivoBanco = $"{GerarHashNomeArquivo()}#{nomeArquivoBanco}";
                                        }

                                        documento.AtulizarDocumentoAnexado(nomeArquivoBanco, usuario);


                                    } //TODO: código comentado aguardando novas estórias para implementação dos demais tipos de processo;
                                }
                                else {//if (tipoProcesso == TipoProcessoEnum.TRABALHISTA || tipoProcesso == TipoProcessoEnum.TRABALHISTA_ADMINISTRATIVO) {
                                    string parteNome = lancamento.Parte != null ? lancamento.Parte.Nome : string.Empty;
                                    relatorio.AppendLine($"\"{numeroCompromissoCliente}\";\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{TipoProcessoEnum.PorId(processo.TipoProcessoId).Nome}\";\"{lancamento.Sequencial}\";\"{lancamento.Valor}\";\"{parteNome}\";\"{lancamento.DataPagamento}\";\"{nomeDoArquivo}\";\"{STATUS_NAO_CARREGADO}\";\"Tipo de processo diferente do esperado pela rotina.\";");
                                    File.Copy(
                                        comprovanteTemporario,
                                        comprovanteResultadoTemp, true);
                                    contadorFalha++;
                                    continue;
                                } //TODO: código comentado aguardando novas estórias para implementação dos demais tipos de processo;
                                  //else {
                                  //    TipoDocumento tipoDocumentoPadrao = DatabaseContext.TiposDocumentos.FirstOrDefault(x => x.Id == Int32.Parse(parametroTipoDocumento));
                                  //    Logger.LogInformation($"Usando Tipo Documento '{ tipoDocumentoPadrao.Id }' - '{ tipoDocumentoPadrao.Descricao }' como padrão para documentos;");

                                //    Logger.LogInformation($"Criando Documento do Processo");
                                //    var documentoProcesso = DocumentoProcesso.Criar(processo, tipoDocumentoPadrao, DataString.FromString(nomeDoArquivo), null, usuario);

                                //    DatabaseContext.Add(documentoProcesso);
                                //}
                                string parte = lancamento.Parte != null ? lancamento.Parte.Nome : string.Empty;
                                relatorio.AppendLine($"\"{numeroCompromissoCliente}\";\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{TipoProcessoEnum.PorId(processo.TipoProcessoId).Nome}\";\"{lancamento.Sequencial}\";\"{lancamento.Valor}\";\"{parte}\";\"{lancamento.DataPagamento}\";\"{nomeDoArquivo}\";\"{STATUS_CARREGADO}\";\"{ocorrencia}\";");

                                Logger.LogInformation($"Salvando informações no SISJUR;");
                                DatabaseContext.SaveChanges();
                                Logger.LogInformation($"Informações salvas para '{ nomeDoArquivo }';");

                                contadorSucesso++;
                            }
                        }
                    }
                    fs.Dispose();
                }

                Logger.LogInformation($"Gerando 'Relatório';");
                string arquivoRetorno = Path.Combine(caminhoResultadoTemp, "relatório_carga_comprovantes.csv");
                File.WriteAllText(arquivoRetorno, relatorio.ToString());

                string nomeArquivoResultadoCarga = Path.Combine(parametroCaminhoNasCargaComprovantes, $"Carga_Comprovantes_{agendamentoId}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.zip");
                Logger.LogInformation($"Compactando arquivos para '{ Path.GetFileName(nomeArquivoResultadoCarga) }';");
                ZipFile.CreateFromDirectory(caminhoResultadoTemp, nomeArquivoResultadoCarga, CompressionLevel.Fastest, false);

                Logger.LogInformation($"Limpando arquivos temporários;");
                Directory.Delete(caminhoResultadoTemp, true);

                Logger.LogInformation($"Limpando arquivos temporários;");
                Directory.Delete(diretorioTemporario, true);

                Logger.LogInformation($"Limpando arquivo DE x PARA;");
                FileHandler.DeleteFile(zipFile);

                string mensagemFalha = contadorFalha > 0 ? $" {contadorFalha} comprovantes não carregados" : string.Empty;

                resultadoInfo.ArquivoRetorno = Path.GetFileName(nomeArquivoResultadoCarga);
                resultadoInfo.MensagemResultadoCarga = $"{contadorSucesso} comprovantes carregados{mensagemFalha}";

                Logger.LogInformation($"Comando concluído;");
                Log.Logger.Information(resultadoInfo.MensagemResultadoCarga);
                return CommandResult<ProcessamentoCargaInfo>.Valid(resultadoInfo);
            }
            catch (Exception ex) {
                Logger.LogInformation($"Limpando arquivos temporários;");
                Directory.Delete(caminhoResultadoTemp, true);

                Logger.LogInformation($"Limpando arquivos temporários;");
                Directory.Delete(diretorioTemporario, true);

                Logger.LogInformation($"Limpando arquivo DE x PARA;");
                FileHandler.DeleteFile(zipFile);

                resultadoInfo.MensagemResultadoCarga = "Erro ao processar agendamento";
                Log.Logger.Error($"{resultadoInfo.MensagemResultadoCarga}:{ex.Message}");
                Logger.LogError(ex, "Comando não concluído;");
                throw;
            }
        }

        public CommandResult Expurgar() {
            Log.Logger.Information("################### Início da rotina expurgar ###################");

            Log.Logger.Information(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
            int paramDias = Convert.ToInt32(ParametroJuridico.Obter(ParametrosJuridicos.DIAS_EXPURGO_CARGA_COMPROVANTE).Dados.Conteudo);
            var parametroCaminhoNasCargaComprovantes = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE).Dequeue();

            DateTime dataExpurgo = DateTime.Today.AddDays(-paramDias);
            Log.Logger.Information($"Verificando agendamentos com data inferior a {dataExpurgo.ToString("dd/MM/yyyy")} - {paramDias}");

            LimpaArquivosExecucoesAnteriores(parametroCaminhoNasCargaComprovantes);

            AgendamentoCargaComprovante[] AgendamentosCargaComprovante = DatabaseContext.AgendamentosCargasComprovantes
                .Where(a => a.DataAgendamento <= dataExpurgo)
                .AsNoTracking()
                .ToArray();

            Log.Logger.Information($"Existe(m) {AgendamentosCargaComprovante.Length} agendamento(s) para expurgo");
            Log.Logger.Information("Buscando o NAS do diretório do servidor para expurgar agendamentos");

            int cont = 0;
            foreach (AgendamentoCargaComprovante agendamento in AgendamentosCargaComprovante) {
                try {
                    RemoverArquivosGerados(agendamento.Id);
                    AgendamentoCargaComprovante.Remover(agendamento.Id);

                    Log.Logger.Information($"Agendamento \"{agendamento.NomeArquivoComprovante})\" e seus critérios foram expurgados.\n");
                    cont++;
                }
                catch (Exception ex) {
                    Log.Logger.Information($"Ocorreu um erro ao expurgar o agendamento ({agendamento.NomeArquivoGerado}, {agendamento.Id})");
                    return CommandResult.Invalid(ex.Message);
                }
            }
            Log.Logger.Information($"Foram expurgados {cont} de {AgendamentosCargaComprovante.Length}.");
            Log.Logger.Information("################### Fim da rotina expurgar ###################\n\n");

            return CommandResult.Valid();
        }

        private void RemoverArquivosGerados(int agendamentoId) {
            string entity = "Agendamento Carga Comprovante";
            string command = $"Obter Resultado Carga {entity}";
            Log.Logger.Information(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Log.Logger.Information(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasComprovantes
                    .FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    Log.Logger.Information($"Não foi possivel encontrar agendamentos de {agendamentoId} para expurgo dos arquivos. ");
                    return;
                }

                string finalFilePath = string.Empty;
                var filePath = string.Empty;

                var pathsDir = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_COMPROVANTE);

                Log.Logger.Information(Infra.Extensions.Logs.Obtendo("Arquivo de Comprovantes"));
                foreach (var pathDir in pathsDir) {
                    if (!Directory.Exists(pathDir)) {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoGerado)) {
                        filePath = Path.Combine(pathDir, dadoArquivo.NomeArquivoGerado);
                        Log.Logger.Information(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoGerado}"));
                        FileHandler.DeleteFile(filePath);
                        Log.Logger.Information($"Arquivo {filePath} excluído.");
                    }

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoBaseSap)) {
                        filePath = Path.Combine(pathDir, dadoArquivo.NomeArquivoBaseSap);
                        Log.Logger.Information(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoGerado}"));
                        FileHandler.DeleteFile(filePath);
                        Log.Logger.Information($"Arquivo {filePath} excluído.");
                    }

                    if (!string.IsNullOrEmpty(dadoArquivo.NomeArquivoComprovante)) {
                        filePath = Path.Combine(pathDir, dadoArquivo.NomeArquivoComprovante);
                        Log.Logger.Information(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoGerado}"));
                        FileHandler.DeleteFile(filePath);
                        Log.Logger.Information($"Arquivo {filePath} excluído.");
                    }
                }
            }
            catch (Exception ex) {
                Log.Logger.Information($"Não foi possivel encontrar agendamentos de {agendamentoId} para expurgo dos arquivos. {ex.Message}  ");
            }
        }

        private CommandResult SalvaAnexoNAS(TipoProcessoEnum tipoProcesso, dynamic documento, string arquivoTemporario, string nomeNovoArquivo) {
            string operacao = "Salvar arquivos no NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));

            try {
                Logger.LogInformation("Recuperando parâmetro jurídico");
                var parametroCaminhoNasAnexoLancamento = RecuperaParametroJuridicoPorTipoProcesso(tipoProcesso.NomeEnum).Dados.Dequeue();
                string diretorioTemporarioZip = Path.Combine(Path.GetDirectoryName(arquivoTemporario), "zip");
                var nomeArquivoParaZipar = Path.Combine(diretorioTemporarioZip, nomeNovoArquivo);

                //cria pasta temporária
                if (!Directory.Exists(diretorioTemporarioZip)) {
                    Directory.CreateDirectory(diretorioTemporarioZip);
                }

                if (!Directory.Exists(parametroCaminhoNasAnexoLancamento)) {
                    Directory.CreateDirectory(parametroCaminhoNasAnexoLancamento);
                }

                //movendo arquivo para pasta temporária antes de zipar
                File.Copy(
                   arquivoTemporario,
                   nomeArquivoParaZipar, true);

                var nomeArquivoZipado = Path.Combine(Path.GetDirectoryName(arquivoTemporario), $"{GerarHashNomeArquivo()}{Path.GetFileNameWithoutExtension(nomeNovoArquivo)}.zip");
                Logger.LogInformation($"Compactando arquivos para '{ Path.GetFileName(nomeArquivoZipado) }';");
                ZipFile.CreateFromDirectory(diretorioTemporarioZip, nomeArquivoZipado, CompressionLevel.Fastest, false);

                //Limpa arquivo pasta temporária Zip
                Directory.Delete(diretorioTemporarioZip, true);

                RemoveAnexoAnteriorNas(documento, tipoProcesso);

                string diretorioAdicionalAnexo = Path.Combine(parametroCaminhoNasAnexoLancamento, ObterDiretorioAdicional(documento));

                //Cria diretório adicional antes de anexar o arquivo
                if (!Directory.Exists(diretorioAdicionalAnexo)) {
                    Directory.CreateDirectory(diretorioAdicionalAnexo);
                }

                Logger.LogInformation($"Anexando comprovante '{ Path.GetFileName(arquivoTemporario) }'");
                string comprovanteParaAnexar = Path.Combine(parametroCaminhoNasAnexoLancamento, MontaNomeArquivoCompleto(documento, Path.GetExtension(nomeArquivoZipado)));

                File.Copy(
                    nomeArquivoZipado,
                    comprovanteParaAnexar, true);

                return CommandResult.Valid();

            }
            catch (Exception ex) {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(operacao));
                return CommandResult.Invalid(ex.Message);
            }
        }

        private CommandResult RemoveAnexoAnteriorNas(dynamic documento, TipoProcessoEnum tipoProcesso) {
            try {

                var parametroCaminhosNasAnexoLancamentos = RecuperaParametroJuridicoPorTipoProcesso(tipoProcesso.NomeEnum).Dados;

                if (parametroCaminhosNasAnexoLancamentos.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation("Buscando anexo anterior");
                foreach (var caminho in parametroCaminhosNasAnexoLancamentos) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }

                    string caminhoArquivoAnterior = Path.Combine(caminho, MontaNomeArquivoCompleto(documento, Path.GetExtension(documento.Nome)));
                    if (File.Exists(caminhoArquivoAnterior)) {
                        Logger.LogInformation("Apagando anexo anterior");
                        FileHandler.DeleteFile(caminhoArquivoAnterior);
                        break;
                    }
                }

                return CommandResult.Valid();
            }
            catch (Exception ex) {
                return CommandResult.Invalid(ex.Message);
            }

        }

        private string ObterDiretorioAdicional(dynamic documento) {
            if (documento.Processo.TipoProcessoId == TipoProcesso.PEX.Id) {
                return $"I{(documento.AutoDocumentoGedId / 10000) + 1}";
            }
            return $"ARQUIVOS_{Math.Truncate(Convert.ToDecimal(documento.Processo.Id) / 10000) + 1}\\{documento.Processo.Id}";
        }

        private string MontaNomeArquivoCompleto(dynamic documento, string extensaoArquivo) {
            if (documento.Processo.TipoProcessoId == TipoProcesso.PEX.Id) {
                return $"I{(documento.AutoDocumentoGedId / 10000) + 1}\\{documento.AutoDocumentoGedId}{extensaoArquivo}";
            }

            return $"ARQUIVOS_{Math.Truncate(Convert.ToDecimal(documento.Processo.Id) / 10000) + 1}\\{documento.Processo.Id}\\{documento.Processo.Id}_{documento.Lancamento.Sequencial}_{documento.Id}{extensaoArquivo}";
        }

        private CommandResult<Queue<string>> RecuperaParametroJuridicoPorTipoProcesso(string tipoProcesso) {

            string parametroJuridicoId;

            switch (tipoProcesso) {
                case nameof(TipoProcessoEnum.CIVEL_CONSUMIDOR):
                    parametroJuridicoId = ParametrosJuridicos.DIR_NAS_LANC_CIV_CONS;
                    break;
                case nameof(TipoProcessoEnum.JEC):
                    parametroJuridicoId = ParametrosJuridicos.DIR_NAS_LANC_JEC;
                    break;
                case nameof(TipoProcessoEnum.PEX):
                    parametroJuridicoId = ParametrosJuridicos.DIR_NAS_LANC_PEX;
                    break;
                default:
                    return CommandResult<Queue<string>>.Invalid($"Tipo processo diferente do esperado pela rotina: {tipoProcesso}");
            }

            return CommandResult<Queue<string>>.Valid(ParametroJuridico.TratarCaminhoDinamico(parametroJuridicoId));

        }

        private string GerarHashNomeArquivo() {
            return Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }

        private void LimpaArquivosExecucoesAnteriores(string CaminhoNas) {
            string caminhoArquivoPastasTemporarias = Path.Combine(CaminhoNas, "PastasTemporarias.tmp");
            string caminhoArquivoZipTemporario = Path.Combine(CaminhoNas, "ZipTemporario.tmp");

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Limpando arquivos temporarios"));
            if (File.Exists(caminhoArquivoZipTemporario)) {
                using (var fs = File.OpenRead(caminhoArquivoZipTemporario)) {
                    using (var reader = new StreamReader(fs)) {
                        string arquivoZip;
                        while ((arquivoZip = reader.ReadLine()) != null) {

                            if (string.IsNullOrEmpty(arquivoZip)) {
                                continue;
                            }
                            FileHandler.DeleteFile(arquivoZip);
                        }
                        fs.Dispose();
                    }
                }

                FileHandler.DeleteFile(caminhoArquivoZipTemporario);
            }

            if (File.Exists(caminhoArquivoPastasTemporarias)) {
                using (var fs = File.OpenRead(caminhoArquivoPastasTemporarias)) {
                    using (var reader = new StreamReader(fs)) {
                        string diretorioTemp;
                        while ((diretorioTemp = reader.ReadLine()) != null) {

                            if (string.IsNullOrEmpty(diretorioTemp)) {
                                continue;
                            }

                            if (Directory.Exists(diretorioTemp)) {
                                Directory.Delete(diretorioTemp, true);
                            }
                        }
                        fs.Dispose();
                    }
                }

                FileHandler.DeleteFile(caminhoArquivoPastasTemporarias);
            }
        }
    }

}
