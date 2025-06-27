namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501InfoDepRequestDTO
    {
        public string? InfodepCpfdep { get; set; }
        public DateTime? InfodepDtnascto { get; set; }
        public string? InfodepNome { get; set; }
        public string? InfodepDepirrf { get; set; }
        public string? InfodepTpdep { get; set; }
        public string? InfodepDescrdep { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
