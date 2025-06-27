namespace Oi.Juridico.WebApi.V2.Areas.Manutencoes.Contrato.DTOs
{
    public class LogContratoEscritorioResponse
    {
        public long? CodContratoEscritorio { get; set; }
        public string OperacaoContrato { get; set; }
        public string? DatLogContrato { get; set; }
        public string CodUsuario { get; set; }
        public long? CodTipoContratoEscritorioA { get; set; }
        public long? CodTipoContratoEscritorioD { get; set; }
        public string? DatInicioVigenciaA { get; set; }
        public string? DatInicioVigenciaD { get; set; }
        public string? DatFimVigenciaA { get; set; }
        public string? DatFimVigenciaD { get; set; }
        public string CnpjA { get; set; }
        public string CnpjD { get; set; }
        public string NumContratoJecVcA { get; set; }
        public string NumContratoJecVcD { get; set; }
        public string NomContratoA { get; set; }
        public string NomContratoD { get; set; }
        public decimal? ValUnitarioJecCcA { get; set; }
        public decimal? ValUnitarioJecCcD { get; set; }
        public decimal? ValUnitarioProconA { get; set; }
        public decimal? ValUnitarioProconD { get; set; }
        public decimal? ValUnitAudCapitalA { get; set; }
        public decimal? ValUnitAudCapitalD { get; set; }
        public decimal? ValUnitAudInteriorA { get; set; }
        public decimal? ValUnitAudInteriorD { get; set; }
        public decimal? ValVepA { get; set; }
        public decimal? ValVepD { get; set; }
        public string NumSgpagJecVcA { get; set; }
        public string NumSgpagJecVcD { get; set; }
        public string IndPermanenciaLegadoA { get; set; }
        public string IndPermanenciaLegadoD { get; set; }
        public byte? NumMesesPermanenciaA { get; set; }
        public byte? NumMesesPermanenciaD { get; set; }
        public byte? ValDescontoPermanenciaA { get; set; }
        public byte? ValDescontoPermanenciaD { get; set; }
        public string IndAtivoA { get; set; }
        public string IndAtivoD { get; set; }
        public string IndConsideraCalculoVepA { get; set; }
        public string IndConsideraCalculoVepD { get; set; }
        public string OperacaoEscritorio { get; set; }
        public string? DatLogEscritorio { get; set; }
        public int? CodProfissionalA { get; set; }
        public int? CodProfissionalD { get; set; }
        public string? CodEstadoA { get; set; }
        public string? CodEstadoD { get; set; }
        public string NumContratoProconA { get; internal set; }
        public string NumContratoProconD { get; internal set; }
        public long? NumSgpagProconA { get; internal set; }
        public long? NumSgpagProconD { get; internal set; }
        public long? CodContratoAtuacaoA { get; internal set; }
        public long? CodContratoAtuacaoD { get; internal set; }
        public long? CodContratoDiretoriaA { get; internal set; }
        public long? CodContratoDiretoriaD { get; internal set; }
    }
}
