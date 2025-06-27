using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ApuracaoOutliers.DTO
{
    public class ApuracaoOutliersDownloadBaseFechamentoDTO
    {
        public long CodigoInterno { get; set; }
        public string NumeroProcesso { get; set; }
        public string Estado { get; set; }
        public long CodigoComarca { get; set; }
        public string NomeComarca { get; set; }
        public long CodigoVara { get; set; }
        public long CodigoTipoVara { get; set; }
        public string NomeTipoVara { get; set; }
        public long CodigoEmpresaGrupo { get; set; }
        public string NomeEmpresaGrupo { get; set; }
        public decimal TotalPagamento { get; set; }
        public DateTime DataCadastro { get; set; }
        public string PrePos { get; set; }
        public DateTime DataFinalizacaoContabil { get; set; }
        public bool ProcInfluenciaContingencia { get; set; }
        public long CodigoLancamento { get; set; }
        public decimal ValorLancamento { get; set; }
        public DateTime? DataRecebimentoFiscal { get; set; }
        public DateTime? DataPagamento { get; set; }
        public long CodigoCategoriaPagamento { get; set; }
        public string DescricaoCategoriaPagamento { get; set; }
        public bool CatPagInfluenciaContingencia { get; set; }
        public string ParametroMediaMovel { get; set; }
    }
}
