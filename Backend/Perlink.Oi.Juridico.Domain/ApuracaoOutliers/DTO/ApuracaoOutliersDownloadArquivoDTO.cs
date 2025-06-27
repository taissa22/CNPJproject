using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO
{
    public class ApuracaoOutliersDownloadArquivoDTO
    {
        public string NomeArquivo { get; set; }
        public byte[] Arquivo { get; set; }
    }
}
