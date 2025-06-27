using CsvHelper.Configuration.Attributes;

namespace Perlink.Oi.Juridico.Domain.AlteracaoBloco.DTO
{
    public class AlteracaoEmBlocoDTO
    {
        public long Id { get; set; }
        public byte[] Arquivo { get; set; }
        public string NomeDoArquivo { get; set; }
    }

    public class AlteracaoEmBlocoArquivoDTO
    {
        [Index(0)]
        public string CodigoInterno { get; set; }

        [Index(1)]
        public string PrePos { get; set; }

        [Index(2)]
        public string InfluenciaContingencia { get; set; }

        [Index(3)]
        public string FinalizacaoJuridica { get; set; }

        [Index(4)]
        public string FinalizacaoContabil { get; set; }

        [Index(5)]
        public string Mediacao { get; set; }
    }

    public class AlteracaoEmBlocoResultadoDTO
    {
        public string CodigoInterno { get; set; }
        public string PrePosAntes { get; set; }
        public string PrePosDepois { get; set; }
        public string InfluenciaContingenciaAntes { get; set; }
        public string InfluenciaContingenciaDepois { get; set; }
        public string FinalizacaoJuridicaAntes { get; set; }
        public string FinalizacaoJuridicaDepois { get; set; }
        public string FinalizacaoContabilAntes { get; set; }
        public string FinalizacaoContabilDepois { get; set; }
        public string MediacaoAntes { get; set; }
        public string MediacaoDepois { get; set; }
        public string Status { get; set; }
        public string Ocorrencia { get; set; }
    }
}
