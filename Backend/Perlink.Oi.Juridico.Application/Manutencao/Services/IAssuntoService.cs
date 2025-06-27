using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IAssuntoService
    {
        #region Civel Consumidor
        CommandResult CriarDoCivelConsumidor(CriarAssuntoDoCivelConsumidorCommand command);

        CommandResult AtualizarDoCivelConsumidor(AtualizarAssuntoDoCivelConsumidorCommand command);

        CommandResult RemoverDoCivelConsumidor(int assuntoId);
        #endregion

        #region Civel Estrategico

        CommandResult CriarDoCivelEstrategico(CriarAssuntoDoCivelEstrategicoCommand command);

        CommandResult AtualizarDoCivelEstrategico(AtualizarAssuntoDoCivelEstrategicoCommand command);

        CommandResult RemoverDoCivelEstrategico(int assuntoId);

        #endregion

    }
}
