using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs
{
    public class ContratoEscritorioResponse
    {
        public long CodContratoEscritorio { get; set; }
        public string TipoContratoEscritorio { get; set; } = string.Empty;
        public string Escritorios { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public DateTime DatInicioVigencia { get; set; }
        public DateTime DatFimVigencia { get; set; }
        public string Cnpj { get; set; } = string.Empty;
        public string NumContratoJecVc { get; set; } = string.Empty;
        public string NumContratoProcon { get; set; } = string.Empty;
        public string NomContrato { get; set; } = string.Empty;
        public decimal? ValUnitarioJecCc { get; set; }
        public decimal? ValUnitarioProcon { get; set; }
        public decimal? ValUnitAudCapital { get; set; }
        public decimal? ValUnitAudInterior { get; set; }
        public decimal? ValVep { get; set; }
        public string NumSgpagJecVc { get; set; } = string.Empty;
        public long? NumSgpagProcon { get; set; }
        public string IndPermanenciaLegado { get; set; } = string.Empty;
        public byte? NumMesesPermanencia { get; set; }
        public byte? ValDescontoPermanencia { get; set; }
        public string IndAtivo { get; set; } = string.Empty;
        public string IndConsideraCalculoVep { get; set; } = string.Empty;
        public string? ContratoAtuacao { get; set; }
        public string? ContratoDiretoria { get; set; }
    }
}
