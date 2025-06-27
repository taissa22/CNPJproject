namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2501InfocrcontribRequestDTO
    {
        public int? InfocrcontribTpcr { get; set; }
        public decimal? InfocrcontribVrcr { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new (pattern, RegexOptions.None);

            var mensagensErro = new List<string>();

            if (!this.InfocrcontribTpcr.HasValue)
            {
                mensagensErro.Add("O campo \"Código Receita (CR) Contrib. Sociais\" é obrigatório");
            }

            if (this.InfocrcontribTpcr <= 0)
            {
                mensagensErro.Add("O campo \"Código Receita (CR) Contrib. Sociais\" informado é inválido");
            }

            if (!this.InfocrcontribVrcr.HasValue || (this.InfocrcontribVrcr.HasValue && this.InfocrcontribVrcr.Value <= 0))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve ser maior do que 0 (zero).");
            }

            if (this.InfocrcontribVrcr.HasValue && !regex.IsMatch(this.InfocrcontribVrcr.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
