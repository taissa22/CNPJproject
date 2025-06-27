using System;
using System.Collections.Generic;
using System.Text;

namespace Oi.Juridico.Contextos.AtmPexContext.Entities
{
    public class EmpresasCentralizadoras
    {
        public EmpresasCentralizadoras()
        {
            FechamentoPexMedia = new HashSet<FechamentoPexMedia>();
        }

        public byte Codigo { get; set; }
        public string Nome { get; set; }
        public byte NumOrdemClassifProcesso { get; set; }
        public byte? NumAgenciaDepositaria { get; set; }
        public bool? NumDigitoAgenciaDepositaria { get; set; }
        public byte? CodConvenioBb { get; set; }

        public virtual ICollection<FechamentoPexMedia> FechamentoPexMedia { get; set; }
    }
}
