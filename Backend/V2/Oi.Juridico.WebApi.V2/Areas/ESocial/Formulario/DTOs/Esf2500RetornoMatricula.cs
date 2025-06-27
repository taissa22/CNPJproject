namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.DTOs
{
    public class Esf2500RetornoMatricula
    {
        public string? Mensagem { get; set; }
        public string? Processo { get; set; }
        public string? ParteCpf { get; set; }
        public string? ParteNome { get; set; }
        public long CodigoInterno { get; internal set; }
    }
}
