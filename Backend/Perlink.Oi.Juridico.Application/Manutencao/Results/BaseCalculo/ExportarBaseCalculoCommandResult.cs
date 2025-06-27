using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos
{
    public class ExportarBaseCalculoCommandResult : ICommandResult
    {
        public ExportarBaseCalculoCommandResult(IEnumerable<FiltroBaseCalculoCommandResult> basesCalculo)
        {
            BasesCalculo = basesCalculo;
        }

        public IEnumerable<FiltroBaseCalculoCommandResult> BasesCalculo { get; private set; }
    }
}
