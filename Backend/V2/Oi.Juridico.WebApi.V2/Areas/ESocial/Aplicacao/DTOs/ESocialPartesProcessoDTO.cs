namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1.DTOs
{
    public class ESocialPartesProcessoDTO
    {
        public long CodProcesso { get; set; }
        public int CodParte { get; set; }
        public string NomeParte { get; set; } = string.Empty;
        public string CpfParte { get; set; } = string.Empty;
        public byte? StatusReclamante { get; set; }
        public ESocialF2500ConsultaDTO? Formulario2500 { get; set; }
        public IEnumerable<ESocialF2501ConsultaDTO>? ListaFormularios2501 { get; set; }


    }
}
