namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500AbonoRequestDTO
    {
        public string? AbonoAnobase { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();
            //TODO: Implementar validações de chegada
            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
