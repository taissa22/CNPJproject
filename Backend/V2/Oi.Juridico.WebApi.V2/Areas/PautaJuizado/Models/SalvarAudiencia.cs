namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.Models
{
    public class SalvarAudiencia
    {
        public int CodProcesso { get; set; }

        public string Terceirizado { get; set; }

        public int SeqAudiencia { get; set; }

        public int? AlocacaoTipo { get; set; }
    }
}
