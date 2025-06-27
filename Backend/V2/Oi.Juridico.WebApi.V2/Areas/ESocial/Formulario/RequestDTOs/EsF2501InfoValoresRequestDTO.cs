namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501InfoValoresRequestDTO
    {
        public int? InfovaloresIndapuracao { get; set; }
        public decimal? InfovaloresVlrnretido { get; set; }
        public decimal? InfovaloresVlrdepjud { get; set; }
        public decimal? InfovaloresVlrcmpanocal { get; set; }
        public decimal? InfovaloresVlrcmpanoant { get; set; }
        public decimal? InfovaloresVlrrendsusp { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
