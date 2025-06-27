using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class AgendamentoDeFechamentoAtmUfPexRepository : IAgendamentoDeFechamentoAtmUfPexRepository
    {
        private ILogger<IAgendamentoDeFechamentoAtmUfPexRepository> Logger { get; }
        private IParametroJuridicoProvider ParametroJuridico { get; }

        public AgendamentoDeFechamentoAtmUfPexRepository(ILogger<IAgendamentoDeFechamentoAtmUfPexRepository> logger, IParametroJuridicoProvider parametroJuridico)
        {
            Logger = logger;
            ParametroJuridico = parametroJuridico;
        }

        public CommandResult<Arquivo> ObterArquivoBase(int agendamentoId)
        {
            string entity = "Agendamento Carga Comprovante";
            string command = $"Obter Arquivos Base {entity}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(command));

            try
            {
                Logger.LogInformation("Verificando Id do Agendamento");
                if (agendamentoId <= 0)
                {
                    return CommandResult<Arquivo>.Invalid("Id do agendamento não informado");
                }

                var parametroDirNasRelatorioAtmPex = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_ATM_PEX);

                if (!Directory.Exists(parametroDirNasRelatorioAtmPex.First()))
                {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<Arquivo>.Invalid("Diretório NAS não encontrado");
                }

                var arquivos = Directory.GetFiles(parametroDirNasRelatorioAtmPex.First(), $"ATM_Pex_Base_{agendamentoId}_*");

                if (arquivos.Any() == false)
                {
                    Logger.LogInformation("Arquivo não encontrado.");
                    return CommandResult<Arquivo>.Invalid("Arquivo não encontrado.");
                }

                Arquivo arquivoCompleto = new Arquivo();
                arquivoCompleto.DadosArquivo = File.ReadAllBytes(arquivos.First());
                arquivoCompleto.NomeArquivo = Path.GetFileName(arquivos.First());


                return CommandResult<Arquivo>.Valid(arquivoCompleto);

            }
            catch (Exception ex)
            {
                Logger.LogError(Infra.Extensions.Logs.OperacaoComErro(command));
                return CommandResult<Arquivo>.Invalid(ex.Message);
            }
        }

        public CommandResult<Arquivo> ObterArquivoRelatorio(int agendamentoId)
        {
            try
            {
                Logger.LogInformation("Verificando Id do Agendamento");
                if (agendamentoId <= 0)
                {
                    return CommandResult<Arquivo>.Invalid("Id do agendamento não informado");
                }

                var parametroDirNasRelatorioAtmPex = ParametroJuridico.TratarCaminhoDinamico(ParametrosJuridicos.DIR_NAS_RELATORIO_ATM_PEX);

                if (!Directory.Exists(parametroDirNasRelatorioAtmPex.First()))
                {
                    Logger.LogInformation("Diretório Nas não encontrado");
                    return CommandResult<Arquivo>.Invalid("Diretório NAS não encontrado");
                }

                var arquivos = Directory.GetFiles(parametroDirNasRelatorioAtmPex.First(), $"ATM_Pex_Rel_ATM_{agendamentoId}_*");

                if (arquivos.Any() == false)
                {
                    Logger.LogInformation("Arquivo não encontrado.");
                    return CommandResult<Arquivo>.Invalid("Arquivo não encontrado.");
                }

                Arquivo arquivoCompleto = new Arquivo();
                arquivoCompleto.DadosArquivo = File.ReadAllBytes(arquivos.First());
                arquivoCompleto.NomeArquivo = Path.GetFileName(arquivos.First());

                return CommandResult<Arquivo>.Valid(arquivoCompleto);

            }
            catch (Exception ex)
            {
                return CommandResult<Arquivo>.Invalid(ex.Message);
            }
        }
    }

    public class Arquivo
    {
        public string NomeArquivo { get; set; }
        public byte[] DadosArquivo { get; set; }
    }
}
