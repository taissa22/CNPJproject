namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501InfoProcRetRequestDTO
    {
        public byte? InfoprocretTpprocret { get; set; }
        public string? InfoprocretNrprocret { get; set; }
        public long? InfoprocretCodsusp { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
