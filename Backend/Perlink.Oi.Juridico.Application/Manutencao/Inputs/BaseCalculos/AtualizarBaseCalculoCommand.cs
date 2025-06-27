using Flunt.Notifications;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.BaseCalculos
{
    public class AtualizarBaseCalculoCommand : ICommand
    {
        public AtualizarBaseCalculoCommand() { }

        public AtualizarBaseCalculoCommand(long? codigoBaseCalculo, string descricao, bool ehbaseInicial)
        {
            CodigoBaseCalculo = codigoBaseCalculo;
            Descricao = descricao;
            ehBaseInicial = ehbaseInicial;
        }

        public long? CodigoBaseCalculo { get; set; }

        public string Descricao { get; set; }

        public bool ehBaseInicial { get; set; }
    }
}
