using Microsoft.EntityFrameworkCore;
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
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using StatusAgendamento = Perlink.Oi.Juridico.Infra.Enums.StatusAgendamento;
using TipoProcessoEnum = Perlink.Oi.Juridico.Infra.Enums.TipoProcesso;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services.Implementations {

    internal class DocumentoService : IDocumentoService {
        private const string STATUS_CARREGADO = "Carregado";
        private const string STATUS_NAO_CARREGADO = "Nao carregado";

        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IDocumentoService> Logger { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }
        private IFileHandler FileHandler { get; }
        private IAgendamentoCargaDocumentoService AgendamentoCargaDocumento { get; }

        public DocumentoService(IDatabaseContext databaseContext, ILogger<IDocumentoService> logger,
                                IParametroJuridicoProvider parametroJuridico, IFileHandler fileHandler, IAgendamentoCargaDocumentoService agendamentoCargaDocumento) {
            DatabaseContext = databaseContext;
            Logger = logger;
            ParametroJuridico = parametroJuridico;
            FileHandler = fileHandler;
            AgendamentoCargaDocumento = agendamentoCargaDocumento;
        }

        public CommandResult ExecutarRotinaPeloNAS(string arquivoDePara) {
            string entity = "Carga Documentos";
            string command = $"Execução manual da rotina de {entity} utilizando NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
            var caminhoNasOrigemDePara = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS).Dequeue();

            if (!Directory.Exists(caminhoNasOrigemDePara)) {
                Directory.CreateDirectory(caminhoNasOrigemDePara);
            }

            try {
                string caminhoArquivoBaseFGV = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo DE x PARA de documentos"));

                if (File.Exists(Path.Combine(caminhoNasOrigemDePara, arquivoDePara))) {
                    caminhoArquivoBaseFGV = Path.Combine(caminhoNasOrigemDePara, arquivoDePara);
                }

                if (string.IsNullOrEmpty(caminhoArquivoBaseFGV)) {
                    string mensagem = $"Arquivo de DE x PARA não encontrado em {arquivoDePara}";
                    Logger.LogInformation(mensagem);
                    throw new Exception(mensagem);
                }

                Random r = new Random();
                int idRandomico = r.Next(0, 1000000000);

                var processamentoInfo = ProcessaCargaDocumentos(caminhoArquivoBaseFGV, idRandomico);

                if (!processamentoInfo.IsValid) {
                    Logger.LogInformation(string.Join(',', processamentoInfo.Mensagens));
                    throw new Exception(string.Join(',', processamentoInfo.Mensagens));
                }

            }
            catch (Exception ex) {
                string mensagemResuladoCarga = "Erro ao realizar carga de comprovantes";
                Log.Logger.Error($"{mensagemResuladoCarga}:{ex.Message}");
                return CommandResult.Invalid(ex.Message);
            }

            return CommandResult.Valid();
        }

        public CommandResult ExecutarRotina(string arquivoDePara) {
            string entity = "Carga Documentos";
            string command = $"Execução manual da rotina de {entity} informando arquivo";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                string caminhoArquivoBaseFGV = string.Empty;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo DE x PARA de documentos"));

                if (File.Exists(arquivoDePara)) {
                    caminhoArquivoBaseFGV = arquivoDePara;
                }

                if (string.IsNullOrEmpty(caminhoArquivoBaseFGV)) {
                    string mensagem = $"Arquivo de DE x PARA não encontrado em {arquivoDePara}";
                    Logger.LogInformation(mensagem);
                    throw new Exception(mensagem);
                }

                Random r = new Random();
                int idRandomico = r.Next(0, 1000000000);

                var processamentoInfo = ProcessaCargaDocumentos(caminhoArquivoBaseFGV, idRandomico);

                if (!processamentoInfo.IsValid) {
                    Logger.LogInformation(string.Join(',', processamentoInfo.Mensagens));
                    throw new Exception(string.Join(',', processamentoInfo.Mensagens));
                }

            }
            catch (Exception ex) {
                string mensagemResuladoCarga = "Erro ao realizar carga de comprovantes";
                Log.Logger.Error($"{mensagemResuladoCarga}:{ex.Message}");
                return CommandResult.Invalid(ex.Message);
            }

            return CommandResult.Valid();
        }

        public CommandResult ExecutarAgendamento() {
            string entity = "Carga Documentos";
            string command = $"Executa Agendamento {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
            var caminhoNasOrigemDePara = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS).Dequeue();

            if (!Directory.Exists(caminhoNasOrigemDePara)) {
                Directory.CreateDirectory(caminhoNasOrigemDePara);
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
            var listaAgendamentos = DatabaseContext.AgendamentosCargasDocumentos
                                                    .Where(l => l.StatusAgendamentoId == StatusAgendamento.AGENDADO.Id || l.StatusAgendamentoId == StatusAgendamento.EM_EXECUCAO.Id)
                                                    .OrderBy(o => o.DataAgendamento);

            Log.Logger.Information($"{listaAgendamentos.Count()} agendamentos encontrados para execução");

            foreach (var agendamento in listaAgendamentos) {
                try {
                    if (agendamento.StatusAgendamento.Id == StatusAgendamento.EM_EXECUCAO.Id) {
                        agendamento.AtualizarStatusAgendamento(StatusAgendamento.ERRO.Id);
                        DatabaseContext.SaveChanges();
                        continue;
                    }

                    agendamento.AtualizarStatusAgendamento(StatusAgendamento.EM_EXECUCAO.Id);

                    DatabaseContext.SaveChanges();

                    string caminhoArquivoBaseFGV = string.Empty;

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo DE x PARA de documentos"));

                    if (File.Exists(Path.Combine(caminhoNasOrigemDePara, agendamento.NomeArquivoBaseFGV))) {
                        caminhoArquivoBaseFGV = Path.Combine(caminhoNasOrigemDePara, agendamento.NomeArquivoBaseFGV);
                    }

                    if (string.IsNullOrEmpty(caminhoArquivoBaseFGV)) {
                        string mensagem = "Arquivo de DE x PARA não encontrado";
                        Logger.LogInformation(mensagem);
                        throw new Exception(mensagem);
                    }

                    var processamentoInfo = ProcessaCargaDocumentos(caminhoArquivoBaseFGV, agendamento.Id);

                    if (!processamentoInfo.IsValid) {
                        Logger.LogInformation(string.Join(',', processamentoInfo.Mensagens));
                        throw new Exception(string.Join(',', processamentoInfo.Mensagens));
                    }

                    agendamento.AtualizarResultadoAgendamento(StatusAgendamento.EXECUTADO.Id, processamentoInfo.Dados.ArquivoRetorno, processamentoInfo.Dados.MensagemResultadoCarga);

                    DatabaseContext.SaveChanges();
                }
                catch (Exception ex) {
                    string mensagemResuladoCarga = "Erro ao realizar carga de comprovantes";
                    agendamento.AtualizarErroAgendamento(StatusAgendamento.ERRO.Id, ex.Message, ex.StackTrace, mensagemResuladoCarga);
                    Log.Logger.Error($"{mensagemResuladoCarga}:{ex.Message}");
                    DatabaseContext.SaveChanges();
                    return CommandResult.Invalid(ex.Message);
                }
            }

            return CommandResult.Valid();
        }

        public CommandResult<ProcessamentoCargaInfo> ProcessaCargaDocumentos(string arquivoDePara, int agendamentoId) {
            ProcessamentoCargaInfo resultadoInfo = new ProcessamentoCargaInfo() {
                ArquivoRetorno = string.Empty,
                MensagemResultadoCarga = string.Empty
            };

            try {
                string caminhoNasOrigemDocumentos = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.SERV_CARGA_DOCUMENTOS).Dequeue();
                string caminhoNasResultado = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOC_RESULT).Dequeue();
                bool deleteArquivosOrigem = ParametroJuridico.Obter(ParametrosJuridicos.DEL_DOC_CREDORES_ORIGEM).Dados.Conteudo == "S";

                if (!Directory.Exists(caminhoNasOrigemDocumentos)) {
                    Directory.CreateDirectory(caminhoNasOrigemDocumentos);
                }

                if (!Directory.Exists(caminhoNasResultado)) {
                    Directory.CreateDirectory(caminhoNasResultado);
                }

                Logger.LogInformation($"Usando NAS no sisjur: '{ caminhoNasOrigemDocumentos }';");

                Usuario usuario = DatabaseContext.Usuarios.FirstOrDefault(x => x.Id == "SISJUR_JOB");

                Logger.LogInformation($"Iniciando leitura do 'de_para.csv'");

                int contadorSucesso = 0;
                int contadorFalha = 0;

                var relatorio = new StringBuilder();
                relatorio.AppendLine("Codigo interno do processo;Numero do processo;Documento anexado;Status;Ocorrencia");

                using (var fs = File.OpenRead(arquivoDePara)) {
                    using (var reader = new StreamReader(fs)) {
                        // ignora o header;
                        reader.ReadLine();

                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            var cells = line.Replace("\"", "").Split(";");

                            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Informações do arquivo DExPARA."));

                            string codigoInternoProcesso = string.IsNullOrEmpty(cells[0]) ? string.Empty : cells[0];
                            string codigoTipoDocumento = string.IsNullOrEmpty(cells[1]) ? string.Empty : cells[1];
                            string nomeArquivoOrigem = string.IsNullOrEmpty(cells[2]) ? string.Empty : Path.Combine(caminhoNasOrigemDocumentos, cells[2]);

                            Processo processo = DatabaseContext.Processos.FirstOrDefault(x => x.Id == int.Parse(codigoInternoProcesso));
                            TipoDocumento tipoDocumento = DatabaseContext.TiposDocumentos.FirstOrDefault(x => x.Id == int.Parse(codigoTipoDocumento));

                            if (string.IsNullOrEmpty(codigoInternoProcesso)) {
                                Logger.LogInformation(OcorrenciasCargaDocumentos.CODIGO_INTERNO_PROCESSO_NAO_PREENCHIDO);
                                contadorFalha++;
                                relatorio.AppendLine($"\"{codigoInternoProcesso}\";=\"{codigoTipoDocumento}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaDocumentos.CODIGO_INTERNO_PROCESSO_NAO_PREENCHIDO}\"");
                                continue;
                            }

                            if (string.IsNullOrEmpty(codigoTipoDocumento)) {
                                Logger.LogInformation(OcorrenciasCargaDocumentos.CODIGO_TIPO_DOCUMENTO_NAO_PREENCHIDO);
                                contadorFalha++;
                                relatorio.AppendLine($"\"{codigoInternoProcesso}\";=\"{codigoTipoDocumento}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaDocumentos.CODIGO_TIPO_DOCUMENTO_NAO_PREENCHIDO}\"");
                                continue;
                            }

                            if (string.IsNullOrEmpty(nomeArquivoOrigem)) {
                                Logger.LogInformation(OcorrenciasCargaDocumentos.NOME_ARQUIVO_NAO_PREENCHIDO);
                                contadorFalha++;
                                relatorio.AppendLine($"\"{codigoInternoProcesso}\";=\"{codigoTipoDocumento}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaDocumentos.NOME_ARQUIVO_NAO_PREENCHIDO}\"");
                                continue;
                            }

                            if (processo is null) {
                                Logger.LogInformation(OcorrenciasCargaDocumentos.PROCESSO_NAO_ENCONTRADO);
                                contadorFalha++;
                                relatorio.AppendLine($"\"{codigoInternoProcesso}\";=\"{codigoTipoDocumento}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaDocumentos.PROCESSO_NAO_ENCONTRADO}\"");
                                continue;
                            }

                            if (tipoDocumento is null) {
                                Logger.LogInformation(OcorrenciasCargaDocumentos.TIPO_DOCUMENTO_NAO_ENCONTRADO);
                                contadorFalha++;
                                relatorio.AppendLine($"\"{codigoInternoProcesso}\";=\"{codigoTipoDocumento}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaDocumentos.TIPO_DOCUMENTO_NAO_ENCONTRADO}\"");
                                continue;
                            }

                            if (!File.Exists(nomeArquivoOrigem)) {
                                Logger.LogInformation(OcorrenciasCargaDocumentos.ARQUIVO_NAO_ENCONTRADO);
                                contadorFalha++;
                                relatorio.AppendLine($"\"{codigoInternoProcesso}\";=\"{codigoTipoDocumento}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"{OcorrenciasCargaDocumentos.ARQUIVO_NAO_ENCONTRADO}\"");
                                continue;
                            }

                            TipoProcessoEnum tipoProcesso = TipoProcessoEnum.PorId(processo.TipoProcessoId);

                            string ocorrencia = string.Empty;

                            if (tipoProcesso == TipoProcessoEnum.JEC || tipoProcesso == TipoProcessoEnum.CIVEL_CONSUMIDOR || tipoProcesso == TipoProcessoEnum.PEX) {
                                Logger.LogInformation($"Criando Documento do Processo");

                                DocumentoProcesso documento = DocumentoProcesso.Criar(processo, tipoDocumento, null, usuario);
                                DatabaseContext.Add(documento);
                                DatabaseContext.SaveChanges();

                                bool ehPex = tipoProcesso == TipoProcessoEnum.PEX;

                                if (ehPex) {
                                    var autoDocumentoGEDId = DatabaseContext.GetNext("JUR", "GED_SEQ_01", documento);
                                    documento.AtualizarAutoDocumentoGed(autoDocumentoGEDId);
                                }

                                var resultado = SalvaAnexoNAS(tipoProcesso, documento, nomeArquivoOrigem);

                                if (!resultado.IsValid) {
                                    relatorio.AppendLine($"\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"Falha ao anexar comprovante no banco de dados\"");
                                    contadorFalha++;
                                    continue;
                                }

                                string nomeArquivoBanco = $"{Path.GetFileNameWithoutExtension(nomeArquivoOrigem)}.zip";
                                if (ehPex) {
                                    nomeArquivoBanco = $"{GerarHashNomeArquivo()}#{nomeArquivoBanco}";
                                }

                                documento.AtulizarDocumentoAnexado(nomeArquivoBanco, usuario);


                            }
                            else {
                                relatorio.AppendLine($"\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{nomeArquivoOrigem}\";\"{STATUS_NAO_CARREGADO}\";\"Tipo processo diferente do esperado pela rotina\"");
                                contadorFalha++;
                                continue;
                            }

                            relatorio.AppendLine($"\"{processo.Id}\";=\"{processo.NumeroProcesso}\";\"{nomeArquivoOrigem}\";\"{STATUS_CARREGADO}\";\"{string.Empty}\"");

                            Logger.LogInformation($"Salvando informações no SISJUR;");
                            DatabaseContext.SaveChanges();
                            Logger.LogInformation($"Informações salvas para '{ nomeArquivoOrigem }';");

                            contadorSucesso++;
                        }
                        fs.Dispose();
                    }
                }
                string nomeArquivoResultadoCarga = Path.Combine(caminhoNasResultado, $"Carga_Documentos_{agendamentoId}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv");
                Logger.LogInformation($"Gerando arquivo de resultado;");
                File.WriteAllText(nomeArquivoResultadoCarga, relatorio.ToString());

                string mensagemFalha = contadorFalha > 0 ? $" {contadorFalha} documentos não carregados" : string.Empty;

                resultadoInfo.ArquivoRetorno = Path.GetFileName(nomeArquivoResultadoCarga);
                resultadoInfo.MensagemResultadoCarga = $"{contadorSucesso} documentos carregados{mensagemFalha}";

                Logger.LogInformation($"Comando concluído;");
                Log.Logger.Information(resultadoInfo.MensagemResultadoCarga);
                return CommandResult<ProcessamentoCargaInfo>.Valid(resultadoInfo);
            }
            catch (Exception ex) {
                resultadoInfo.MensagemResultadoCarga = "Erro ao processar agendamento";
                Log.Logger.Error($"{resultadoInfo.MensagemResultadoCarga}:{ex.Message}");
                Logger.LogError(ex, "Comando não concluído;");
                return CommandResult<ProcessamentoCargaInfo>.Invalid(ex.Message);
            }
        }

        public void Expurgar() {
            Log.Logger.Information("################### Início da rotina expurgar ###################");

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Parâmetro Jurídico"));
            int paramDias = Convert.ToInt32(ParametroJuridico.Obter(ParametrosJuridicos.DIAS_EXPURGO_CARGA_DOC).Dados.Conteudo);

            DateTime dataExpurgo = DateTime.Today.AddDays(-paramDias);
            Logger.LogInformation($"Verificando agendamentos com data inferior a {dataExpurgo.ToString("dd/MM/yyyy")} - {paramDias}");

            AgendamentoCargaDocumento[] AgendamentosCargaDocumento = DatabaseContext.AgendamentosCargasDocumentos
                .Where(a => a.DataAgendamento <= dataExpurgo)
                .AsNoTracking()
                .ToArray();

            Logger.LogInformation($"Existe(m) {AgendamentosCargaDocumento.Length} agendamento(s) para expurgo");
            Logger.LogInformation("Buscando o NAS do diretório do servidor para expurgar agendamentos");

            int cont = 0;
            foreach (AgendamentoCargaDocumento agendamento in AgendamentosCargaDocumento) {
                try {
                    RemoverArquivosGerados(agendamento.Id);
                    AgendamentoCargaDocumento.Remover(agendamento.Id);

                    Logger.LogInformation($"Agendamento \"{agendamento.Id})\" e seus critérios foram expurgados.\n");
                    Log.Logger.Information($"Agendamento \"{agendamento.Id})\" e seus critérios foram expurgados.\n");
                    cont++;
                }
                catch (Exception ex) {
                    Logger.LogInformation($"Ocorreu um erro ao expurgar o agendamento ({agendamento.NomeArquivoGerado}, {agendamento.Id})");
                    Log.Logger.Information($"Ocorreu um erro ao expurgar o agendamento ({agendamento.NomeArquivoGerado}, {agendamento.Id})");
                }
            }
            Logger.LogInformation($"Foram expurgados {cont} de {AgendamentosCargaDocumento.Length}.");
            Log.Logger.Information($"Foram expurgados {cont} de {AgendamentosCargaDocumento.Length}.");
            Log.Logger.Information("################### Fim da rotina expurgar ###################\n\n");
        }

        private void RemoverArquivosGerados(int agendamentoId) {
            string entity = "Agendamento Carga Documento";
            string command = $"Obter Resultado Carga {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try {
                Logger.LogInformation(Infra.Extensions.Logs.Obtendo(entity));
                var dadoArquivo = DatabaseContext.AgendamentosCargasDocumentos
                    .FirstOrDefault(a => a.Id == agendamentoId);

                if (dadoArquivo is null) {
                    Logger.LogInformation($"Não foi possivel encontrar agendamentos de {agendamentoId} para expurgo dos arquivos. ");
                    return;
                }

                string finalFilePath = string.Empty;
                var filePath = string.Empty;

                var parametroCaminhosNasCargaDoc = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOCUMENTOS);
                var parametroCaminhosNasCargaDocResult = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_CARGA_DOC_RESULT);
                var caminhoNasOrigemDocumentos = ParametroJuridico.Obter(ParametrosJuridicos.SERV_CARGA_DOCUMENTOS).Dados.Conteudo;

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Documentos"));
                foreach (var caminho in parametroCaminhosNasCargaDoc) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }

                    filePath = Path.Combine(caminho, dadoArquivo.NomeArquivoBaseFGV);

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o documentos de origem"));
                    using (var fs = File.OpenRead(filePath)) {
                        using (var reader = new StreamReader(fs)) {
                            // ignora o header;
                            reader.ReadLine();

                            string line;
                            while ((line = reader.ReadLine()) != null) {
                                var cells = line.Replace("\"", "").Split(";");

                                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Informações do arquivo DExPARA."));

                                string codigoInternoProcesso = string.IsNullOrEmpty(cells[0]) ? string.Empty : cells[0];
                                string nomeArquivoOrigem = string.IsNullOrEmpty(cells[2]) ? string.Empty : Path.Combine(caminhoNasOrigemDocumentos, cells[2]);

                                if (string.IsNullOrEmpty(nomeArquivoOrigem)) {
                                    continue;
                                }

                                FileHandler.DeleteFile(nomeArquivoOrigem);
                            }
                            fs.Dispose();
                        }
                    }

                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoBaseFGV}"));
                    FileHandler.DeleteFile(filePath);
                    Logger.LogInformation($"Arquivo {filePath} excluído.");
                }

                Logger.LogInformation(Infra.Extensions.Logs.Obtendo("Arquivo de Documentos"));
                foreach (var caminho in parametroCaminhosNasCargaDocResult) {
                    if (!Directory.Exists(caminho)) {
                        continue;
                    }

                    filePath = Path.Combine(caminho, dadoArquivo.NomeArquivoGerado);
                    Logger.LogInformation(Infra.Extensions.Logs.Obtendo($"Deletando o arquivo {dadoArquivo.NomeArquivoGerado}"));
                    FileHandler.DeleteFile(filePath);
                    Logger.LogInformation($"Arquivo {filePath} excluído.");
                }

            }
            catch (Exception ex) {
                Log.Logger.Information($"Não foi possivel encontrar agendamentos de {agendamentoId} para expurgo dos arquivos. {ex.Message}  ");
            }
        }

        private CommandResult SalvaAnexoNAS(TipoProcessoEnum tipoProcesso, DocumentoProcesso documento, string arquivo) {
            string operacao = "Salvar arquivos no NAS";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(operacao));

            try {
                Logger.LogInformation("Recuperando parâmetro jurídico");
                var parametroCaminhoNasAnexoDocumento = RecuperaParametroJuridicoPorTipoProcesso(tipoProcesso.NomeEnum).Dados.Dequeue();
                string diretorioTemporarioZip = Path.Combine(Path.GetDirectoryName(arquivo), "zip");
                var nomeArquivoParaZipar = Path.Combine(diretorioTemporarioZip, Path.GetFileName(arquivo));

                //cria pasta temporária
                if (!Directory.Exists(diretorioTemporarioZip)) {
                    Directory.CreateDirectory(diretorioTemporarioZip);
                }

                if (!Directory.Exists(parametroCaminhoNasAnexoDocumento)) {
                    Directory.CreateDirectory(parametroCaminhoNasAnexoDocumento);
                }

                //movendo arquivo para pasta temporária antes de zipar
                File.Copy(
                   arquivo,
                   nomeArquivoParaZipar, true);

                var nomeArquivoZipado = Path.Combine(Path.GetDirectoryName(arquivo), $"{GerarHashNomeArquivo()}{Path.GetFileNameWithoutExtension(arquivo)}.zip");
                Logger.LogInformation($"Compactando arquivos para '{ Path.GetFileName(nomeArquivoZipado) }';");
                ZipFile.CreateFromDirectory(diretorioTemporarioZip, nomeArquivoZipado, CompressionLevel.Fastest, false);

                //Limpa arquivo pasta temporária Zip
                Directory.Delete(diretorioTemporarioZip, true);

                RemoveAnexoAnteriorNas(documento, tipoProcesso);

                string diretorioAdicionalAnexo = Path.Combine(parametroCaminhoNasAnexoDocumento, ObterDiretorioAdicional(documento));

                //Cria diretório adicional antes de anexar o arquivo
                if (!Directory.Exists(diretorioAdicionalAnexo)) {
                    Directory.CreateDirectory(diretorioAdicionalAnexo);
                }

                Logger.LogInformation($"Anexando comprovante '{ Path.GetFileName(arquivo) }'");
                string comprovanteParaAnexar = Path.Combine(parametroCaminhoNasAnexoDocumento, MontaNomeArquivoCompleto(documento, Path.GetExtension(nomeArquivoZipado)));

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

        private CommandResult RemoveAnexoAnteriorNas(DocumentoProcesso documento, TipoProcessoEnum tipoProcesso) {
            try {
                var parametroCaminhosNasAnexoDocumentos = RecuperaParametroJuridicoPorTipoProcesso(tipoProcesso.NomeEnum).Dados;

                if (parametroCaminhosNasAnexoDocumentos.Count <= 0) {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult.Invalid("Diretório NAS não encontrado");
                }

                Logger.LogInformation("Buscando anexo anterior");
                foreach (var caminho in parametroCaminhosNasAnexoDocumentos) {
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

                Logger.LogInformation("Remoção finalizada");
                return CommandResult.Valid();
            }
            catch (Exception ex) {
                Logger.LogError(ex, "Apagando anexo anterior");
                return CommandResult.Invalid(ex.Message);
            }
        }

        private string ObterDiretorioAdicional(DocumentoProcesso documento) {
            if (documento.Processo.TipoProcessoId == TipoProcesso.PEX.Id) {
                return $"I{(documento.AutoDocumentoGedId / 10000) + 1}";
            }
            return $"ARQUIVOS_{Math.Truncate(Convert.ToDecimal(documento.Processo.Id) / 10000) + 1}\\{documento.Processo.Id}";
        }

        private string MontaNomeArquivoCompleto(DocumentoProcesso documento, string extensaoArquivo) {
            if (documento.Processo.TipoProcessoId == TipoProcesso.PEX.Id) {
                return $"I{(documento.AutoDocumentoGedId / 10000) + 1}\\{documento.AutoDocumentoGedId}{extensaoArquivo}";
            }

            return $"ARQUIVOS_{Math.Truncate(Convert.ToDecimal(documento.Processo.Id) / 10000) + 1}\\{documento.Processo.Id}\\{documento.Processo.Id}_{documento.Id}{extensaoArquivo}";
        }

        private CommandResult<Queue<string>> RecuperaParametroJuridicoPorTipoProcesso(string tipoProcesso) {
            string parametroJuridicoId;

            switch (tipoProcesso) {
                case nameof(TipoProcessoEnum.CIVEL_CONSUMIDOR):
                    parametroJuridicoId = ParametrosJuridicos.DIR_NAS_DOC_CIVCON;
                    break;

                case nameof(TipoProcessoEnum.JEC):
                    parametroJuridicoId = ParametrosJuridicos.DIR_NAS_DOC_JEC;
                    break;

                case nameof(TipoProcessoEnum.PEX):
                    parametroJuridicoId = ParametrosJuridicos.DIR_NAS_DOC_PEX;
                    break;

                default:
                    return CommandResult<Queue<string>>.Invalid($"Tipo processo diferente do esperado pela rotina: {tipoProcesso}");
            }

            return CommandResult<Queue<string>>.Valid(ParametroJuridico.TratarCaminhoDinamico(parametroJuridicoId));
        }

        private string GerarHashNomeArquivo() {
            return Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }
    }
}
