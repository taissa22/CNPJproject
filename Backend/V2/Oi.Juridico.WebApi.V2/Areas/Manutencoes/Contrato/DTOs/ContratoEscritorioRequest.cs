namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs
{
    public class ContratoEscritorioRequest
    {
        public long CodTipoContratoEscritorio { get; set; }
        public DateTime DatInicioVigencia { get; set; }
        public DateTime DatFimVigencia { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string? NumContratoJecVc { get; set; }
        public string? NumContratoProcon { get; set; }
        public string NomContrato { get; set; } = string.Empty;
        public decimal? ValUnitarioJecCc { get; set; }
        public decimal? ValUnitarioProcon { get; set; }
        public decimal? ValUnitAudCapital { get; set; }
        public decimal? ValUnitAudInterior { get; set; }
        public decimal? ValVep { get; set; }
        public string? NumSgpagJecVc { get; set; }
        public long? NumSgpagProcon { get; set; }
        public string IndPermanenciaLegado { get; set; } = string.Empty;
        public byte? NumMesesPermanencia { get; set; }
        public byte? ValDescontoPermanencia { get; set; }
        public string IndAtivo { get; set; } = string.Empty;
        public string IndConsideraCalculoVep { get; set; } = string.Empty;
        public long? CodContratoAtuacao { get; set; }
        public long? CodContratoDiretoria { get; set; }

        public List<int> Escritorios { get; set; }
        public List<string> Estados { get; set; }
    }
}
