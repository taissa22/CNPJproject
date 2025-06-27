using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.BaseCalculos
{
    public class RegistrarBaseCalculoCommand : ICommand
    {
        public RegistrarBaseCalculoCommand() { }

        public RegistrarBaseCalculoCommand(string descricao) 
        {
            Descricao = descricao;
        }

        public string Descricao { get; set; }
    }
}
