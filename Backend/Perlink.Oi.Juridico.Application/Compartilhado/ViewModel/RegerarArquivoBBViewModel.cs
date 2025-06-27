using System;

namespace Perlink.Oi.Juridico.Application.Compartilhado.ViewModel
{
    public class RegerarArquivoBBViewModel
    { 
        public long CodigoLote { get; set; }
        public long NumeroLoteBB { get; set; }
        public DateTime DataGuia { get; set; }
        public bool EnviaServidor { get; set; }
    }
}
