using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos
{
    public class PesquisarBaseCalculoCommandResult : ICommandResult
    {
        public PesquisarBaseCalculoCommandResult() { }

        public PesquisarBaseCalculoCommandResult(IEnumerable<FiltroBaseCalculoCommandResult> baseCalculos, int totalElementos)
        {
            Lista = baseCalculos;
            Total = totalElementos;
        }

        public IEnumerable<FiltroBaseCalculoCommandResult> Lista { get; private set; }

        public int Total { get; private set; }
    }
}