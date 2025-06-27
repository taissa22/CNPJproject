using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposParticipacao
{
    public class RegistrarTipoParticipacaoCommand : ICommand
    {
        public RegistrarTipoParticipacaoCommand() { }

        public RegistrarTipoParticipacaoCommand(string descricao)
        {
            Descricao = descricao;
        }

        public string Descricao { get; set; }
    }
}
