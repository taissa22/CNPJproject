using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IPedidoService {

        #region Trabalhista

        CommandResult CriarDoTrabalhista(CriarPedidoDoTrabalhistaCommand command);
        CommandResult AtualizarDoTrabalhista(AtualizarPedidoDoTrabalhistaCommand command);
        CommandResult RemoverDoTrabalhista(int id);

        #endregion


        #region Civel Estratégico

        CommandResult CriarDoCivelEstrategico(CriarPedidoDoCivelEstrategicoCommand command);

        CommandResult AtualizarDoCivelEstrategico(AtualizarPedidoDoCivelEstrategicoCommand command);

        CommandResult RemoverDoCivelEstrategico(int id);

        #endregion

        #region Civel Consumidor

        CommandResult CriarDoCivelConsumidor(CriarPedidoDoCivelConsumidorCommand command);

        CommandResult AtualizarDoCivelConsumidor(AtualizarPedidoDoCivelConsumidorCommand command);

        CommandResult RemoverDoCivelConsumidor(int id);

        #endregion
    }
}
