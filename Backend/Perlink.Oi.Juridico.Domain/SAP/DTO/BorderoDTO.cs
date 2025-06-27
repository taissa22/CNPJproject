using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class BorderoDTO
    {
        public long CodigoLote { get; set; }
        public long Seq_Bordero { get; set; }
        public string NomeBeneficiario { get; set; }
        public string CpfBeneficiario { get; set; }
        public string CnpjBeneficiario { get; set; }
        public string CidadeBeneficiario { get; set; }
        public string NumeroBancoBeneficiario { get; set; }
        public string DigitoBancoBeneficiario { get; set; }
        public string NumeroAgenciaBeneficiario { get; set; }
        public string DigitoAgenciaBeneficiario { get; set; }
        public string NumeroContaCorrenteBeneficiario { get; set; }
        public string DigitoContaCorrenteBeneficiario { get; set; }
        public decimal Valor { get; set; }
        public string Comentario { get; set; }
    }
}
