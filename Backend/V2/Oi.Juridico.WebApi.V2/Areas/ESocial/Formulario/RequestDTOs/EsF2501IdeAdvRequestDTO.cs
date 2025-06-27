namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501IdeAdvRequestDTO
    {
        public byte? IdeadvTpinsc { get; set; }
        public string? IdeadvNrinsc { get; set; }
        public decimal? IdeadvVlradv { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
