using AutoMapper;
using ByteSizeLib;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Entity;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Enum;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Repository;
using Perlink.Oi.Juridico.Domain.AlteracaoBloco.Interface.Service;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Enum;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Service;
using Perlink.Oi.Juridico.Domain.External.Interface;
using Shared.Domain.Impl;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.AlteracaoBloco.Service
{
    public class AlteracaoEmBlocoService : BaseCrudService<AlteracaoEmBloco, long>, IAlteracaoEmBlocoService
    {
        public readonly IAlteracaoEmBlocoRepository repository;
        public readonly IProcessoRepository processoRepository;
        public readonly IParametroRepository parametroRepository;
        public readonly IPermissaoService permissao;
        public readonly IUsuarioService usuario;
        private readonly INasService nasService;
        public readonly IMapper mapper;
        public AlteracaoEmBlocoService(IAlteracaoEmBlocoRepository repository, IProcessoRepository processoRepository, IParametroRepository parametroRepository, IPermissaoService permissao, IUsuarioService usuario, INasService nasService, IMapper mapper) : base(repository)
        {
            this.repository = repository;
            this.processoRepository = processoRepository;
            this.parametroRepository = parametroRepository;
            this.permissao = permissao;
            this.usuario = usuario;
            this.nasService = nasService;
            this.mapper = mapper;
        }

        #region Métodos privados
        private string PegaCaminhoDoArquivo(string nomeDaPasta)
        {
            var caminho = Path.Combine(Directory.GetCurrentDirectory() + "\\Areas\\AlteracaoBloco");
            var caminhoCompleto = Path.Combine(caminho, nomeDaPasta);

            if (!Directory.Exists(caminhoCompleto))
            {
                Directory.CreateDirectory(caminhoCompleto);
            }
            return caminhoCompleto;

        }

        private string MontaCaminhoDoArquivoTemporario(IFormFile arquivo)
        {
            var caminhoTemporario = Path.Combine("Arquivos", "AlteracaoEmBloco");

            var caminhoDestino = PegaCaminhoDoArquivo(caminhoTemporario);

            var arquivoCompleto = ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"');

            var arquivoSemExtensao = Path.GetFileNameWithoutExtension(arquivoCompleto) + "_" + DateTime.Now.ToString("yyyyMMddHmmss");

            var extensao = Path.GetExtension(arquivoCompleto);

            return Path.Combine(caminhoDestino, arquivoSemExtensao + extensao);
        }

        private void SalvarArquivo(string caminhoCompleto, IFormFile arquivo)
        {
            try
            {
                using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
                {
                    arquivo.CopyTo(stream);
                }
            }
            catch (Exception)
            {
                throw new Exception(Textos.Mensagem_Erro_Gravar_Arquivo);
            }

        }

        private static ICollection<AlteracaoEmBlocoArquivoDTO> LerArquivo(AlteracaoEmBloco model, string caminho)
        {
            var caminhoCompleto = Path.Combine(caminho, model.Arquivo);
            var csvConfiguration = new CsvHelper.Configuration.Configuration()
            {
                Delimiter = ";",
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                HasHeaderRecord = true,
                CultureInfo = CultureInfo.CurrentCulture,
                IgnoreBlankLines = true,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(caminhoCompleto, Encoding.UTF8))
            using (var csv = new CsvReader(reader, csvConfiguration))
            {
                var registros = csv.GetRecords<AlteracaoEmBlocoArquivoDTO>().ToList();
                return registros;
            }
        }

        private void CriarArquivo(string caminho, AlteracaoEmBloco model, List<AlteracaoEmBlocoResultadoDTO> resultado)
        {
            var caminhoCompleto = Path.Combine(caminho, model.Arquivo);
            var csvConfiguration = new CsvHelper.Configuration.Configuration()
            {
                Delimiter = ";",
                TrimOptions = CsvHelper.Configuration.TrimOptions.Trim,
                HasHeaderRecord = true,
                CultureInfo = CultureInfo.CurrentCulture,
                IgnoreBlankLines = true,
                MissingFieldFound = null
            };

            using (var streamWriter = new StreamWriter(caminhoCompleto, false, Encoding.UTF8))
            using (var csv = new CsvWriter(streamWriter, csvConfiguration))
            {
                var classMap = csv.Configuration.AutoMap<AlteracaoEmBlocoResultadoDTO>();

                classMap.Map(row => row.CodigoInterno).Name("Código Interno");
                classMap.Map(row => row.PrePosAntes).Name("Pré ou Pós RJ _ A");
                classMap.Map(row => row.PrePosDepois).Name("Pré ou Pós RJ _ D");
                if (model.CodigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial)
                {
                    classMap.Map(row => row.InfluenciaContingenciaAntes).Name("Influencia a Contingência _ A");
                    classMap.Map(row => row.InfluenciaContingenciaDepois).Name("Influencia a Contingência _ D");
                }
                else if (model.CodigoTipoProcesso == TipoProcessoEnum.Pex)
                {
                    classMap.Map(row => row.InfluenciaContingenciaAntes).Name("Influencia a Contingência _ A");
                    classMap.Map(row => row.InfluenciaContingenciaDepois).Name("Influencia a Contingência _ D");
                    classMap.Map(row => row.FinalizacaoJuridicaAntes).Name("Finalização Jurídica _ A");
                    classMap.Map(row => row.FinalizacaoJuridicaDepois).Name("Finalização Jurídica _ D");
                    classMap.Map(row => row.FinalizacaoContabilAntes).Name("Finalização Contábil _ A");
                    classMap.Map(row => row.FinalizacaoContabilDepois).Name("Finalização Contábil _ D");
                    classMap.Map(row => row.MediacaoAntes).Name("Mediação_ A");
                    classMap.Map(row => row.MediacaoDepois).Name("Mediação_ D");
                }
                else
                {
                    classMap.Map(row => row.InfluenciaContingenciaAntes).Name("Influencia a Contingência _ A").Ignore();
                    classMap.Map(row => row.InfluenciaContingenciaDepois).Name("Influencia a Contingência _ D").Ignore();
                    classMap.Map(row => row.FinalizacaoJuridicaAntes).Name("Finalização Jurídica _ A").Ignore();
                    classMap.Map(row => row.FinalizacaoJuridicaDepois).Name("Finalização Jurídica _ D").Ignore();
                    classMap.Map(row => row.FinalizacaoContabilAntes).Name("Finalização Contábil _ A").Ignore();
                    classMap.Map(row => row.FinalizacaoContabilDepois).Name("Finalização Contábil _ D").Ignore();
                    classMap.Map(row => row.MediacaoAntes).Name("Mediação_ A").Ignore();
                    classMap.Map(row => row.MediacaoDepois).Name("Mediação_ D").Ignore();
                }
                classMap.Map(row => row.Status).Name("Status");
                classMap.Map(row => row.Ocorrencia).Name("Ocorrência");

                csv.WriteRecords(mapper.Map<IList<AlteracaoEmBlocoResultadoDTO>>(resultado));
                streamWriter.Flush();
            }
        }

        private void MoverArquivo(string nomeDoArquivo, string caminhoAtual, string caminhoDestino)
        {
            var atual = Path.Combine(caminhoAtual, nomeDoArquivo);
            var destino = Path.Combine(caminhoDestino, nomeDoArquivo);

            if (!Directory.Exists(caminhoDestino))
            {
                Directory.CreateDirectory(caminhoDestino);
            }

            if (File.Exists(destino))
            {
                File.Delete(destino);
            }

            File.Copy(atual, destino);
        }

        #endregion métodos privados

        public bool ValidarArquivoDaPastaTemporaria(IFormFile arquivo)
        {
            var extensao = Path.GetExtension(ContentDispositionHeaderValue.Parse(arquivo.ContentDisposition).FileName.Trim('"'));

            if (extensao.ToLower() != ".csv".ToLower())
                return false;
            return true;
        }

        public bool ValidarTamanhoDoArquivo(string caminhoDoArquivoTemporario, Parametro modelParametro)
        {
            var informacoesDoArquivo = new FileInfo(caminhoDoArquivoTemporario);
            var tamanhoDoArquivo = ByteSize.FromBytes(informacoesDoArquivo.Length);
            var tamanhoMaximoDoArquivo = ByteSize.FromMegaBytes(Convert.ToDouble(modelParametro.Conteudo));

            if (tamanhoDoArquivo > tamanhoMaximoDoArquivo)
            {
                File.Delete(caminhoDoArquivoTemporario);
                return false;
            }
            return true;
        }

        public bool ValidarColunasDoArquivo(string caminhoDoArquivoTemporario, long codigoTipoProcesso)
        {
            using (var reader = new StreamReader(caminhoDoArquivoTemporario, Encoding.UTF8))
            {
                var conteudo = reader.ReadLine().Split(";");
                var colunas = conteudo.Count();

                if (codigoTipoProcesso == (int)TipoProcessoEnum.Administrativo && colunas == 2)
                    return true;

                if (codigoTipoProcesso == (int)TipoProcessoEnum.Trabalhista && colunas == 2)
                    return true;

                if (codigoTipoProcesso == (int)TipoProcessoEnum.TributarioJudicial && colunas == 2)
                    return true;

                if (codigoTipoProcesso == (int)TipoProcessoEnum.CivelEstrategico && colunas == 5)
                    return true;

                if (codigoTipoProcesso == (int)TipoProcessoEnum.JuizadoEspecial && colunas == 6)
                    return true;

                if (codigoTipoProcesso == (int)TipoProcessoEnum.CivelConsumidor && colunas == 6)
                    return true;

                if (codigoTipoProcesso == (int)TipoProcessoEnum.Pex && colunas == 7)
                    return true;

                return false;
            }
        }

        public bool ValidarDadosDoArquivo(string caminhoDoArquivoTemporario, long codigoTipoProcesso)
        {
            using (var reader = new StreamReader(caminhoDoArquivoTemporario, Encoding.UTF8))
            {
                var conteudo = reader.ReadToEnd().Split(";");
                var count = conteudo.Count();
                if (count > 4 && codigoTipoProcesso == TipoProcessoEnum.CivelEstrategico.GetHashCode())
                {
                    return true;
                }

                if (count > 5 && codigoTipoProcesso == TipoProcessoEnum.CivelConsumidor.GetHashCode())
                {
                    return true;
                }

                if (count > 6 && codigoTipoProcesso == TipoProcessoEnum.Pex.GetHashCode())
                {
                    return true;
                }

                if (count > 2 && (codigoTipoProcesso == TipoProcessoEnum.TributarioJudicial.GetHashCode() || codigoTipoProcesso == TipoProcessoEnum.Trabalhista.GetHashCode()
                    || codigoTipoProcesso == TipoProcessoEnum.Administrativo.GetHashCode()))
                {
                    return true;
                }

                if (count > 4 && codigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial.GetHashCode())
                    return true;

                return false;
            }
        }

        public string GravarArquivoNaPastaTemporaria(IFormFile arquivo)
        {
            var caminhoTemporario = MontaCaminhoDoArquivoTemporario(arquivo);

            SalvarArquivo(caminhoTemporario, arquivo);

            return caminhoTemporario;
        }

        public string GravarArquivoNoNas(string caminhoDoArquivoTemporario, IFormFile arquivo, Parametro modelParametro)
        {
            var nomeDoArquivo = Path.GetFileName(caminhoDoArquivoTemporario);

            nasService.SaveFileFromTempDir(caminhoDoArquivoTemporario, modelParametro.Conteudo);

            return nomeDoArquivo;
        }

        public async Task<AlteracaoEmBlocoDTO> BaixarPlanilha(AlteracaoEmBloco model, Parametro parametroPastaDestinoArquivo)
        {

            var conteudo = await nasService.GetFileFromNAs(model.Arquivo, parametroPastaDestinoArquivo.Id);

            //var quebraNomeDoArquivo = model.Arquivo.Split('_').ToList();

            //quebraNomeDoArquivo.Remove(quebraNomeDoArquivo[quebraNomeDoArquivo.Count - 1]);

            //var nomeOriginal = "";

            //foreach (var nome in quebraNomeDoArquivo)
            //{
            //    nomeOriginal += nomeOriginal != "" ? "_" : "";

            //    nomeOriginal += nome;
            //}

            var modelDTO = new AlteracaoEmBlocoDTO()
            {
                Id = model.Id,
                NomeDoArquivo = model.Arquivo,
                Arquivo = conteudo
            };

            return modelDTO;
        }

        public void RemoverAgendamento(long id, Parametro parametro, string nomeDoArquivo)
        {
            var diretorios = nasService.TratarCaminhoDinamicoNoParametroJuridico(parametro.Conteudo);

            repository.RemoverPorId(id);

            foreach (var diretorio in diretorios)
            {
                var caminhoCompleto = Path.Combine(diretorio, nomeDoArquivo);

                if (File.Exists(caminhoCompleto))
                {
                    File.Delete(caminhoCompleto);
                }
            }
        }

        public async Task<IEnumerable<AlteracaoEmBlocoRetornoDTO>> ListarAgendamentos(int index, int count)
        {
            if (!permissao.TemPermissao(PermissaoEnum.f_VisualizaTodasAlteracoesProc))
            {
                var usuarioAtual = usuario.ObterUsuarioLogado().Result.Id;
                var resultado = await repository.ListarAgendamentosPorUsuario(index, count, usuarioAtual);
                return resultado;
            }
            else
            {
                var resultado = await repository.ListarAgendamentos(index, count);
                return resultado;
            }
        }

        public async Task<int> ObterQuantidadeTotal()
        {
            if (!permissao.TemPermissao(PermissaoEnum.f_VisualizaTodasAlteracoesProc))
            {
                var usuarioAtual = usuario.ObterUsuarioLogado().Result.Id;
                var quantidade = await repository.QuantidadeTotalPorUsuario(usuarioAtual);
                return quantidade;
            }
            else
            {
                var quantidade = await repository.QuantidadeTotal();
                return quantidade;
            }

        }

        public async Task ExpurgoAlteracaoEmBloco(ILogger logger)
        {
            logger.LogInformation("Busca parametro: EXPURGO_ALTERACAO_BLOCO = Quantidade de dias para expurgo dos agendamentos de alteração em bloco de processos Web");
            var parametro = parametroRepository.RecuperarPorNome("EXPURGO_ALTERACAO_BLOCO").Conteudo;
            logger.LogInformation(parametro);

            logger.LogInformation("Buscando caminho onde os arquivos foram salvos");
            var caminhoDoArquivo = parametroRepository.RecuperarPorNome("DIR_NAS_ALT_BLOCO_AGEND").Conteudo;
            logger.LogInformation(caminhoDoArquivo);

            logger.LogInformation(" Buscando caminho onde os arquivos de resultado foram salvos");
            var caminhoDestino = parametroRepository.RecuperarPorNome("DIR_NAS_ALT_BLOCO_RESULT").Conteudo;
            logger.LogInformation(caminhoDestino);

            logger.LogInformation("Buscando agendamentos a serem expurgados");
            var resultado = await repository.ExpurgoAlteracaoEmBloco(Convert.ToInt32(parametro));

            foreach (var item in resultado)
            {
                logger.LogInformation($"Excluindo arquivo : {item.Arquivo}");
                File.Delete(Path.Combine(caminhoDoArquivo, item.Arquivo));

                logger.LogInformation($"Excluindo arquivo de resultado : {item.Arquivo}");
                File.Delete(Path.Combine(caminhoDestino, item.Arquivo));

                logger.LogInformation($"Excluindo registro da tabela: {item.Id}");
                await repository.RemoverPorId(item.Id);
            }
        }

        private void ValidarFinalizacao(string data, StringBuilder criticas, ILogger logger, string mensagem)
        {
            bool dataInvalida = false;
            bool ehReativar = false;
            bool ehData = false;

            if (data.Contains("/"))
            {
                ehData = true;
                var dataJuridica = data.Split("/");

                if (dataJuridica[2].Count() != 4)
                {
                    dataInvalida = true;
                }
            }

            if (ehData)
            {
                logger.LogInformation(string.Concat("Verificando se a data de finalização jurídica e contábil estão válidas."));
                if (Convert.ToDateTime(data) > DateTime.Now || dataInvalida)
                {
                    logger.LogInformation(mensagem);
                    criticas.Append(mensagem);
                }
            }

            if (data == "REATIVAR")
            {
                ehReativar = true;
            }

            if (!ehReativar && !ehData)
            {
                logger.LogInformation(mensagem);
                criticas.Append(mensagem);
            }
        }

        public async Task ProcessarAgendamentos(ILogger logger)
        {
            var agendamentos = await repository.ListarAgendamentosComStatusAgendado();
            var resultado = new List<AlteracaoEmBlocoResultadoDTO>();
            var caminhoAtual = parametroRepository.RecuperarPorNome("DIR_NAS_ALT_BLOCO_AGEND").Conteudo;
            var caminhoDestino = parametroRepository.RecuperarPorNome("DIR_NAS_ALT_BLOCO_RESULT").Conteudo;
            var criticas = new StringBuilder();
            int processosAtualizados;
            int processosComErro;
            string prePosAntes;
            string influenciaContingenciaAntes;
            string finalizacaoJuridica;
            string finalizacaoContabil;
            string mediacao;

            foreach (var agendamento in agendamentos)
            {
                var model = await repository.RecuperarPorId(agendamento.Id);
                model.DataExecucao = DateTime.Now;
                model.Status = AlteracaoEmBlocoEnum.Processando;
                await repository.Atualizar(model);
                await repository.CommitAsync();
                processosAtualizados = 0;
                processosComErro = 0;

                var caminhoCompleto = Path.Combine(caminhoAtual, model.Arquivo);
                if (!File.Exists(caminhoCompleto))
                {
                    logger.LogInformation("Arquivo não encontrado");
                    model.Status = AlteracaoEmBlocoEnum.Erro;
                    await repository.Atualizar(model);
                    await repository.CommitAsync();
                    continue;
                }

                logger.LogInformation("Movendo arquivo para a pasta de resultado");
                MoverArquivo(model.Arquivo, caminhoAtual, caminhoDestino);
                logger.LogInformation($"Arquivo {model.Arquivo} movido");

                logger.LogInformation($"Lendo arquivo {model.Arquivo}");
                var registros = LerArquivo(model, caminhoDestino);

                logger.LogInformation("Realizando validações");
                foreach (var registro in registros)
                {
                    var regex = Regex.IsMatch(registro.CodigoInterno, @"[a-zA-Z á-úÁ-Ú }{,.^?~=+\-_\/*\-+.\|@!#$%&*();]");
                    var processo = regex == true ? null : registro.CodigoInterno != String.Empty ? await processoRepository.RecuperarPorId(Convert.ToInt64(registro.CodigoInterno)) : null;
                    prePosAntes = processo == null ? "" : processo.PrePos == 1 ? "PRE" : processo.PrePos == 2 ? "POS" : processo.PrePos == 3 ? "AMBOS" : "A DEFINIR";
                    influenciaContingenciaAntes = processo == null ? "" : processo.IndicadorConsiderarProvisao == true ? "SIM" : "NAO";
                    finalizacaoJuridica = processo == null ? null : processo.DataFinalizacao.ToString();
                    finalizacaoContabil = processo == null ? null : processo.DataFinalizacaoContabil.ToString();
                    mediacao = processo == null ? "" : processo.IndicadorMediacao == true ? "SIM" : "NAO";

                    var valido = true;
                    var validoRegexTamanho = true;
                    var validoPrePos = true;
                    var validocontingencia = true;
                    logger.LogInformation(string.Concat("Verificando codigo interno vazio. Codigo Interno: ", registro.CodigoInterno));
                    if (String.IsNullOrEmpty(registro.CodigoInterno) || String.IsNullOrWhiteSpace(registro.CodigoInterno))
                    {
                        logger.LogInformation("Código do processo é de preenchimento obrigatório;");
                        criticas.Append("Código do processo é de preenchimento obrigatório;");
                        valido = false;
                    }

                    logger.LogInformation(string.Concat("Verificando Código inválido. Codigo Interno: ", registro.CodigoInterno));
                    if (valido && regex)
                    {
                        logger.LogInformation("Código interno do processo inválido;");
                        criticas.Append("Código interno do processo inválido;");
                        validoRegexTamanho = false;
                    }

                    logger.LogInformation(string.Concat("Verificando codigo interno com mais de 8 caracteres. Codigo Interno: ", registro.CodigoInterno));
                    if (valido && registro.CodigoInterno.Length > 8)
                    {
                        logger.LogInformation("O código interno do processo deve conter até 8 algarismos;");
                        criticas.Append("O código interno do processo deve conter até 8 algarismos;");
                        validoRegexTamanho = false;
                    }

                    logger.LogInformation(string.Concat("Procurando processo no banco de dados. Codigo Interno: ", registro.CodigoInterno));
                    if (valido && validoRegexTamanho && (String.IsNullOrEmpty(Convert.ToString(processo)) || String.IsNullOrWhiteSpace(Convert.ToString(processo))))
                    {
                        logger.LogInformation("Processo não encontrado;");
                        criticas.Append("Processo não encontrado;");
                        valido = false;
                    }

                    #region Validações JEC e PEX

                    if (model.CodigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial || model.CodigoTipoProcesso == TipoProcessoEnum.Pex)
                    {
                        logger.LogInformation(string.Concat("O Tipo de processo do processo é Juizado Especial ou Pex. Codigo Interno: ", registro.CodigoInterno));
                        logger.LogInformation(string.Concat("Verificando se campo Pre/pos e influencia a contigencia foram preenchidos. Codigo Interno: ", registro.CodigoInterno));
                        if (String.IsNullOrEmpty(registro.PrePos) && String.IsNullOrEmpty(registro.InfluenciaContingencia))
                        {
                            logger.LogInformation("Pelo menos um dos campos, Pré ou Pós RJ ou influencia a contingência deve ser preenchido para alteração;");
                            criticas.Append("Pelo menos um dos campos, Pré ou Pós RJ ou influencia a contingência deve ser preenchido para alteração;");
                            validoPrePos = false;
                            validocontingencia = false;
                        }

                        if (!String.IsNullOrEmpty(registro.PrePos) && validoPrePos && registro.PrePos != "PRE" && registro.PrePos != "POS" && registro.PrePos != "A DEFINIR" && registro.PrePos != "AMBOS")
                        {
                            logger.LogInformation(string.Concat("Verificando se o campo Pre/pos contem um dos valores PRE, POS, AMBOS ou A DEFINIR. Codigo Interno: ", registro.CodigoInterno));
                            logger.LogInformation("Pré ou Pós RJ tem que ser PRE, POS, AMBOS ou A DEFINIR;");
                            criticas.Append("Pré ou Pós RJ tem que ser PRE, POS, AMBOS ou A DEFINIR;");
                        }

                        logger.LogInformation(string.Concat("Verificando se o campo Influencia a contingencia contem um dos valores SIM ou NAO. Codigo Interno: ", registro.CodigoInterno));
                        if (!String.IsNullOrEmpty(registro.InfluenciaContingencia) && validocontingencia && (registro.InfluenciaContingencia != "SIM" && registro.InfluenciaContingencia != "NAO"))
                        {
                            logger.LogInformation("Influencia a Contingência tem que ser SIM ou NAO;");
                            criticas.Append("Influencia a Contingência tem que ser SIM ou NAO;");
                        }

                        if (model.CodigoTipoProcesso == TipoProcessoEnum.Pex)
                        {
                            ValidarFinalizacao(registro.FinalizacaoJuridica, criticas, logger, "Conteúdo do campo Data Finalização Jurídica inválido: informar uma data no formato dd/mm/yyyy que seja menor ou igual a hoje para finalizar juridicamente o processo ou escrever REATIVAR para a reativação jurídica;");
                            ValidarFinalizacao(registro.FinalizacaoContabil, criticas, logger, "Conteúdo do campo Data Finalização Contábil inválido: informar uma data no formato dd/mm/yyyy que seja menor ou igual a hoje para finalizar contabilmente o processo ou escrever REATIVAR para a reativação contábil;");
                        }

                        logger.LogInformation(string.Concat("Verificando se o campo mediação contem um dos valores SIM ou NAO ou VAZIO. Codigo Interno: ", registro.CodigoInterno));
                        if (registro.Mediacao != "SIM" && registro.Mediacao != "NAO" && registro.Mediacao != "")
                        {
                            logger.LogInformation("Mediação tem que ser SIM, NAO ou VAZIO;");
                            criticas.Append("Conteúdo do campo Mediação inválido: informar SIM, NAO ou deixar vazio para esse campo não ser atualizado para o processo;");
                        }
                    }

                    if ((model.CodigoTipoProcesso != TipoProcessoEnum.JuizadoEspecial || model.CodigoTipoProcesso != TipoProcessoEnum.Pex) && String.IsNullOrEmpty(registro.PrePos))
                    {
                        logger.LogInformation(string.Concat("Verificando se o campo Pre/pos esta vazio. Codigo Interno: ", registro.CodigoInterno));
                        criticas.Append("O campo Pré ou Pós RJ é de preenchimento obrigatório;");
                        valido = false;
                    }

                    if (!String.IsNullOrEmpty(registro.PrePos) && (model.CodigoTipoProcesso != TipoProcessoEnum.JuizadoEspecial && model.CodigoTipoProcesso != TipoProcessoEnum.Pex) && (registro.PrePos != "PRE" && registro.PrePos != "POS" && registro.PrePos != "A DEFINIR" && registro.PrePos != "AMBOS"))
                    {
                        logger.LogInformation(string.Concat("Verificando se o campo Pre/pos contem um dos valores PRE, POS, AMBOS ou A DEFINIR. Codigo Interno: ", registro.CodigoInterno));
                        criticas.Append("Pré ou Pós RJ tem que ser PRE, POS, AMBOS ou A DEFINIR;");
                    }

                    #endregion Validações JEC

                    #region Validações CC


                    if (model.CodigoTipoProcesso == TipoProcessoEnum.CivelConsumidor)
                    {
                        logger.LogInformation(string.Concat("O Tipo de processo do processo é Cível Consumidor. Codigo Interno: ", registro.CodigoInterno));
                        logger.LogInformation(string.Concat("Verificando se campo Pre/pos e influencia a contigencia foram preenchidos. Codigo Interno: ", registro.CodigoInterno));
                        if (String.IsNullOrEmpty(registro.PrePos) && String.IsNullOrEmpty(registro.InfluenciaContingencia))
                        {
                            logger.LogInformation("Pelo menos um dos campos, Pré ou Pós RJ ou influencia a contingência deve ser preenchido para alteração;");
                            criticas.Append("Pelo menos um dos campos, Pré ou Pós RJ ou influencia a contingência deve ser preenchido para alteração;");
                            validoPrePos = false;
                            validocontingencia = false;
                        }

                        if (!String.IsNullOrEmpty(registro.PrePos) && validoPrePos && registro.PrePos != "PRE" && registro.PrePos != "POS" && registro.PrePos != "A DEFINIR" && registro.PrePos != "AMBOS")
                        {
                            logger.LogInformation(string.Concat("Verificando se o campo Pre/pos contem um dos valores PRE, POS ou A DEFINIR. Codigo Interno: ", registro.CodigoInterno));
                            logger.LogInformation("Pré ou Pós RJ tem que ser PRE, POS ou A DEFINIR;");
                            criticas.Append("Pré ou Pós RJ tem que ser PRE, POS ou A DEFINIR;");
                        }

                        logger.LogInformation(string.Concat("Verificando se o campo Influencia a contingencia contem um dos valores SIM ou NAO. Codigo Interno: ", registro.CodigoInterno));
                        if (!String.IsNullOrEmpty(registro.InfluenciaContingencia) && validocontingencia && (registro.InfluenciaContingencia != "SIM" && registro.InfluenciaContingencia != "NAO"))
                        {
                            logger.LogInformation("Influencia a Contingência tem que ser SIM ou NAO;");
                            criticas.Append("Influencia a Contingência tem que ser SIM ou NAO;");
                        }
                    }

                    if (model.CodigoTipoProcesso != TipoProcessoEnum.CivelConsumidor && String.IsNullOrEmpty(registro.PrePos))
                    {
                        logger.LogInformation(string.Concat("Verificando se o campo Pre/pos esta vazio. Codigo Interno: ", registro.CodigoInterno));
                        criticas.Append("O campo Pré ou Pós RJ é de preenchimento obrigatório;");
                        valido = false;
                    }

                    if (!String.IsNullOrEmpty(registro.PrePos) && model.CodigoTipoProcesso != TipoProcessoEnum.CivelConsumidor && (registro.PrePos != "PRE" && registro.PrePos != "POS" && registro.PrePos != "A DEFINIR" && registro.PrePos != "AMBOS"))
                    {
                        logger.LogInformation(string.Concat("Verificando se o campo Pre/pos contem um dos valores PRE, POS ou A DEFINIR. Codigo Interno: ", registro.CodigoInterno));
                        criticas.Append("Pré ou Pós RJ tem que ser PRE, POS ou A DEFINIR;");
                    }

                    #endregion Validações CC

                    if (processo != null)
                    {
                        logger.LogInformation(string.Concat("Verificando se o tipo de processo do agendamento é o mesmo cadastrado no processo. Codigo Interno: ", registro.CodigoInterno));
                        if (processo.CodigoTipoProcesso != model.CodigoTipoProcesso.GetHashCode())
                        {
                            criticas.Append("Tipo de Processo diferente do informado no agendamento;");
                        }
                    }

                    if (criticas.Length == 0)
                    {
                        logger.LogInformation($"Atualizando status do agendamento {model.Id} para finalizado");
                        model.Status = AlteracaoEmBlocoEnum.Finalizado;
                        processosAtualizados++;
                        model.ProcessosAtualizados = processosAtualizados;
                        await repository.Atualizar(model);
                        logger.LogInformation("Status atualizado");

                        processo.PrePos = registro.PrePos == "PRE" ? 1 : registro.PrePos == "POS" ? 2 : registro.PrePos == "A DEFINIR" ? 0 : registro.PrePos == "AMBOS" ? 3 : processo.PrePos;

                        if (processo.CodigoTipoProcesso == TipoProcessoEnum.JuizadoEspecial.GetHashCode())
                        {
                            processo.IndicadorConsiderarProvisao = registro.InfluenciaContingencia == "SIM" ? true : registro.InfluenciaContingencia == "NAO" ? false : processo.IndicadorConsiderarProvisao;
                        }
                        if (processo.CodigoTipoProcesso == TipoProcessoEnum.Pex.GetHashCode() || processo.CodigoTipoProcesso == TipoProcessoEnum.CivelConsumidor.GetHashCode())
                        {
                            processo.IndicadorConsiderarProvisao = registro.InfluenciaContingencia == "SIM" ? true : registro.InfluenciaContingencia == "NAO" ? false : processo.IndicadorConsiderarProvisao;
                            processo.DataFinalizacao = Convert.ToDateTime(registro.FinalizacaoJuridica);
                            processo.DataFinalizacaoContabil = Convert.ToDateTime(registro.FinalizacaoContabil);
                            processo.IndicaProcessoAtivo = false;
                        }

                        if (processo.CodigoTipoProcesso == TipoProcessoEnum.Pex.GetHashCode())
                        {
                            processo.IndicadorMediacao = registro.Mediacao == "SIM" ? true : registro.Mediacao == "NAO" ? false : processo.IndicadorMediacao;
                        }

                        logger.LogInformation($"Atualizando processo {processo.Id}");
                        await processoRepository.Atualizar(processo);
                        logger.LogInformation("processo atualizado");
                    }
                    else
                    {
                        logger.LogInformation($"Atualizando status do agendamento {model.Id} para finalizado");
                        model.Status = AlteracaoEmBlocoEnum.Finalizado;
                        processosComErro++;
                        model.ProcessosComErro = processosComErro;
                        await repository.Atualizar(model);
                        logger.LogInformation("Status atualizado");
                    }

                    if (criticas.Length > 0)
                    {
                        prePosAntes = null;
                        influenciaContingenciaAntes = null;
                        finalizacaoJuridica = null;
                        finalizacaoContabil = null;
                    }

                    resultado.Add(
                            new AlteracaoEmBlocoResultadoDTO()
                            {
                                CodigoInterno = registro.CodigoInterno,
                                PrePosAntes = prePosAntes,
                                PrePosDepois = prePosAntes == null ? "" : processo == null ? "" : processo.PrePos == 1 ? "PRE" : processo.PrePos == 2 ? "POS" : processo.PrePos == 0 ? "A DEFINIR" : processo.PrePos == 3 ? "AMBOS" : "",
                                InfluenciaContingenciaAntes = influenciaContingenciaAntes,
                                InfluenciaContingenciaDepois = influenciaContingenciaAntes == null ? "" : processo == null ? "" : processo.IndicadorConsiderarProvisao == true ? "SIM" : processo.IndicadorConsiderarProvisao == false ? "NAO" : "",
                                FinalizacaoJuridicaAntes = finalizacaoJuridica,
                                FinalizacaoJuridicaDepois = finalizacaoJuridica == null ? null : processo == null ? null : processo.DataFinalizacao.ToString(),
                                FinalizacaoContabilAntes = finalizacaoContabil,
                                FinalizacaoContabilDepois = finalizacaoContabil == null ? null : processo == null ? null : processo.DataFinalizacaoContabil.ToString(),
                                MediacaoAntes = mediacao,
                                MediacaoDepois = mediacao == null ? "" : processo == null ? "" : processo.IndicadorMediacao == true ? "SIM" : processo.IndicadorMediacao == false ? "NAO" : "",
                                Status = criticas.Length == 0 ? "Alterado" : "Não alterado",
                                Ocorrencia = criticas.ToString(),
                            });
                    criticas.Clear();
                }

                await repository.CommitAsync();

                logger.LogInformation($"Criando arquivo {model.Arquivo} de resultado");
                CriarArquivo(caminhoDestino, model, resultado);
                logger.LogInformation("Arquivo criado");

                resultado.Clear();
            }
        }
    }
}
