using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class AudienciaProcessoBuscaResultadoVM
    {
        public AudienciaProcessoBuscaResultadoVM(IEnumerable<AudienciaProcessoResultadoVM> dado, int totalElementos)
        {
            Dado = dado;
            TotalElementos = totalElementos;
        }

        public int TotalElementos { get; private set; }

        public IEnumerable<AudienciaProcessoResultadoVM> Dado { get; private set; }
    }
}
