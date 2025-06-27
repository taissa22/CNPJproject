namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs
{
    public class ObterEscritorioResponse
    {
        public long CodContratoEscritorio { get; set; }
        public long CodTipoContratoEscritorio { get; set; }
        public List<int> Escritorios { get; set; }
        public List<string> UF { get; set; }
        public DateTime DatInicioVigencia { get; set; }
        public DateTime DatFimVigencia { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string NumContratoJecVc { get; set; } = string.Empty;
        public string NomContrato { get; set; } = string.Empty;
        public decimal? ValUnitarioJecCc { get; set; }
        public decimal? ValUnitarioProcon { get; set; }
        public decimal? ValUnitAudCapital { get; set; }
        public decimal? ValUnitAudInterior { get; set; }
        public decimal? ValVep { get; set; }
        public string NumSgpagJecVc { get; set; } = string.Empty;
        public byte? NumMesesPermanencia { get; set; }
        public byte? ValDescontoPermanencia { get; set; }
        public bool IndPermanenciaLegado { get; set; }
        public bool IndAtivo { get; set; }
        public bool IndConsideraCalculoVep { get; set; }
        public string? NumContratoProcon { get; internal set; }
        public long? NumSgpagProcon { get; internal set; }
        public long? CodContratoAtuacao { get; internal set; }
        public long? CodContratoDiretoria { get; internal set; }
    }
}
