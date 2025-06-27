using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Entity;
using Perlink.Oi.Juridico.Domain.ControleDeAcesso.Interface.Repository;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class ProcessoService : BaseCrudService<Processo, long>, IProcessoService
    {
        private readonly IProcessoRepository ProcessoRepository;
        private readonly IParametroRepository parametroRepository;
        private readonly IExportacaoPrePosRJRepository exportacaoPrePosRJRepository;
        public ProcessoService(IProcessoRepository ProcessoRepository, IParametroRepository parametroRepository, IExportacaoPrePosRJRepository exportacaoPrePosRJRepository) : base(ProcessoRepository) {
            this.ProcessoRepository = ProcessoRepository;
            this.parametroRepository = parametroRepository;
            this.exportacaoPrePosRJRepository = exportacaoPrePosRJRepository;
        }

        public async Task<ICollection<Processo>> RecuperarPorIdentificador(string numeroProcesso, long codigoProcesso, long codigoTipoProcesso) {
            return await ProcessoRepository.RecuperarPorIdentificador(numeroProcesso, codigoProcesso, codigoTipoProcesso);
        }

        public async Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoInterno(long codigoProcesso, long codigoTipoProcesso, bool ehEscritorio, IList<long> codigosEscritorio = null)
        {
            if (ehEscritorio)
                return await ProcessoRepository.RecuperarProcessoPeloCodigoInterno(codigoProcesso, codigoTipoProcesso, codigosEscritorio);
            else
                return await ProcessoRepository.RecuperarProcessoPeloCodigoInterno(codigoProcesso, codigoTipoProcesso);
        }

		public async Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, bool ehEscritorio, IList<long> codigosEscritorio = null)
        {
            if (ehEscritorio)
                return await ProcessoRepository.RecuperarProcessoPeloCodigoTipoProcesso(numeroProcesso, codigoTipoProcesso, codigosEscritorio);
            else
                return await ProcessoRepository.RecuperarProcessoPeloCodigoTipoProcesso(numeroProcesso, codigoTipoProcesso);
		}
		
        public async Task<string> ExportacaoPrePosRj(TipoProcessoEnum tipoProcesso, ILogger logger)
        {
            var nomeArquivo = string.Empty;
            try
            {
                logger.LogInformation("Busca o parâmetro com a quantidade de meses para exportação");
                var parametroQtdeMesesParaExportacao = parametroRepository.RecuperarPorNome("EXPORTACAOPREPOSQTDMES");

                logger.LogInformation($"Inicio da busca do processos {tipoProcesso}");
                var retorno = await ProcessoRepository.ExportacaoPrePosRj(tipoProcesso, Convert.ToInt32(parametroQtdeMesesParaExportacao.Conteudo));
                logger.LogInformation($"Fim da busca do processos {tipoProcesso}");

                Parametro dirEnvioSAP = RetornaCaminhoArquivo(logger);

                nomeArquivo = GerarNomeArquivo(tipoProcesso, logger);

                using (FileStream fs = File.Create(Path.Combine(dirEnvioSAP.Conteudo, nomeArquivo)))
                {
                    using (var writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(writer))
                        {
                            var ignorarConsiderarProvisao = tipoProcesso != TipoProcessoEnum.JuizadoEspecial && tipoProcesso != TipoProcessoEnum.Trabalhista;
                            var classMap = new DefaultClassMap<ProcessoExportacaoPrePosRJDTO>();
                            classMap.AutoMap();
                            classMap.Map(m => m.ConsiderarProvisao).Ignore(ignorarConsiderarProvisao);
                            csv.Configuration.RegisterClassMap(classMap);

                            csv.Configuration.Delimiter = ";";
                            var options = new TypeConverterOptions { Formats = new[] { "dd/MM/yyyy" } };
                            csv.Configuration.TypeConverterOptionsCache.AddOptions<DateTime>(options);
                            csv.Configuration.TypeConverterOptionsCache.AddOptions<DateTime?>(options);
                            csv.WriteRecords(retorno);
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                logger.LogInformation(e.Message.ToString());
            }

            return nomeArquivo;
        }

        private Parametro RetornaCaminhoArquivo(ILogger logger)
        {
            var dirEnvioSAP = parametroRepository.RecuperarPorNome("EXPORTACAOBASEPREPOSNAS");
            logger.LogInformation($"Diretório: {dirEnvioSAP.Conteudo}");

            logger.LogInformation("Verificando se o caminho de gravação do arquivo existe.");
            if (!Directory.Exists(dirEnvioSAP.Conteudo))
            {
                Directory.CreateDirectory(dirEnvioSAP.Conteudo);
            }

            return dirEnvioSAP;
        }

        private static string GerarNomeArquivo(TipoProcessoEnum tipoProcesso, ILogger logger)
        {
            logger.LogInformation("Inicio da criaçao do arquivo");

            string nomeArquivo = $"Base_Pre_Pos_{tipoProcesso}_{DateTime.Now:ddMMyyyy}.csv";

            logger.LogInformation(nomeArquivo);
            return nomeArquivo;
        }

        public async Task ExpurgoPrePosRj(ILogger logger)
        {
            logger.LogInformation("Busca parametro: EXPURGO_BASES_PRE_POS = Quantidade de dias para expurgo das bases pré/pós RJ geradas");
            var parametro = parametroRepository.RecuperarPorNome("EXPURGO_BASES_PRE_POS").Conteudo;
            logger.LogInformation(parametro);

            logger.LogInformation("Busca caminho onde os arquivos foram salvos");
            var parametroCaminhoArquivos = parametroRepository.RecuperarPorNome("EXPORTACAOBASEPREPOSNAS").Conteudo;
            logger.LogInformation(parametroCaminhoArquivos);

            logger.LogInformation("Busca das exportações a serem expurgadas.");
            var resultado = await ProcessoRepository.ExpurgoPrePosRj(Convert.ToInt32(parametro));

            foreach (var item in resultado)
            {
                if (!string.IsNullOrEmpty(item.ArquivoJec))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoJec));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoJec));
                }

                if (!string.IsNullOrEmpty(item.ArquivoTrabalhista))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoTrabalhista));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoTrabalhista));
                }

                if (!string.IsNullOrEmpty(item.ArquivoCivelConsumidor))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoCivelConsumidor));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoCivelConsumidor));
                }

                if (!string.IsNullOrEmpty(item.ArquivoCivelEstrategico))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoCivelEstrategico));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoCivelEstrategico));
                }

                if (!string.IsNullOrEmpty(item.ArquivoPex))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoPex));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoPex));
                }

                if (!string.IsNullOrEmpty(item.ArquivoTributarioJudicial))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoTributarioJudicial));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoTributarioJudicial));
                }

                if (!string.IsNullOrEmpty(item.ArquivoAdministrativo))
                {
                    logger.LogInformation(string.Concat("Excluindo arquivo :", item.ArquivoAdministrativo));
                    File.Delete(string.Concat(parametroCaminhoArquivos, item.ArquivoAdministrativo));
                }

                logger.LogInformation("Excluindo registro da tabela.");
                logger.LogInformation(item.Id.ToString());
                await exportacaoPrePosRJRepository.RemoverPorId(item.Id);
            }
        }
    }
}
