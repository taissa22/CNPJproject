using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO
{
    public class ApuracaoOutliersDownloadResultadoDTO
    {
        //public ICollection<ListaProcessosBaseFechamentoJecDTO> ListaProcessos { get; set; }
        public long CodigoProcesso { get; set; }
        public decimal TotalPagamentos { get; set; }
        public string ColunaVazia { get; set; }
        public decimal MediaPagamentos { get; set; }
        public decimal FatorDesvioPadrao{ get; set; }
        public decimal DesvioPadrao { get; set; }
        public decimal ValorTotalPagamentos { get; set; }
        public long QtdProcessosPagamentos { get; set; }
        public decimal ValorCorteOutliers { get; set; }
    }
    public class ListaProcessosBaseFechamentoJecDTO
    {
        public long CodigoProcesso { get; set; }
        public decimal TotalPagamentos { get; set; }
    }
}
