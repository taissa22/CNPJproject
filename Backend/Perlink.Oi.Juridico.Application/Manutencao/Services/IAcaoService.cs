using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services {

    public interface IAcaoService {

        #region Cível Estratégico

        CommandResult CriarDoCivelEstrategico(CriarAcaoDoCivelEstrategicoCommand command);

        CommandResult AtualizarDoCivelEstrategico(AtualizarAcaoDoCivelEstrategicoCommand command);

        CommandResult RemoverDoCivelEstrategico(int acaoId);

        #endregion Cível Estratégico

        #region Civel Consumidor

        CommandResult CriarDoCivelConsumidor(CriarAcaoDoCivelConsumidorCommand command);

        CommandResult AtualizarDoCivelConsumidor(AtualizarAcaoDoCivelConsumidorCommand command);

        CommandResult RemoverDoCivelConsumidor(int acaoId);

        #endregion Civel Consumidor

        #region Trabalhista

        CommandResult CriarDoTrabalhista(CriarAcaoDoTrabalhistaCommand command);

        CommandResult AtualizarDoTrabalhista(AtualizarAcaoDoTrabalhistaCommand command);

        CommandResult RemoverDoTrabalhista(int acaoId);

        #endregion Trabalhista

        #region Tributario Judicial

        CommandResult CriarDoTributarioJudicial(CriarAcaoDoTributarioJudicialCommand command);

        CommandResult AtualizarDoTributarioJudicial(AtualizarAcaoDoTributarioJudicialCommand command);

        CommandResult RemoverDoTributarioJudicial(int acaoId);

        #endregion Tributario Judicial
    }
}