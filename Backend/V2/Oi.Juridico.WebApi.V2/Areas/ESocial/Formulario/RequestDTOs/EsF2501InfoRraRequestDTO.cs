namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501InfoRraRequestDTO
    {
        public string? InforraDescrra { get; set; }
        public decimal? InforraQtdmesesrra { get; set; }
        public decimal? DespprocjudVlrdespcustas { get; set; }
        public decimal? DespprocjudVlrdespadvogados { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
