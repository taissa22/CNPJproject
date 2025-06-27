namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501DedDepenRequestDTO
    {
        public byte? DeddepenTprend { get; set; }
        public string? DeddepenCpfdep { get; set; }
        public decimal? DeddepenVlrdeducao { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
