using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.BaseCalculos
{
    public class ExcluirBaseCalculoCommand : ICommand
    {
        public ExcluirBaseCalculoCommand() { }

        public ExcluirBaseCalculoCommand(long id)
        {
            CodigoBaseCalculo = id;
        }

        public long CodigoBaseCalculo { get; set; }
    }
}
