using System.Globalization;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2501InfocrirrfRequestDTO
    {       
        public int? InfocrcontribTpcr { get; set; }
        public decimal? InfocrcontribVrcr { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new (pattern, RegexOptions.None);    

            if (!this.InfocrcontribTpcr.HasValue)
            {
                mensagensErro.Add("O campo \"Código Receita (CR) IRRF\" é obrigatório");
            }

            if (this.InfocrcontribTpcr <= 0)
            {
                mensagensErro.Add("O campo \"Código Receita (CR) IRRF\" informado é inválido");
            }

            if (!this.InfocrcontribVrcr.HasValue || (this.InfocrcontribVrcr.HasValue && this.InfocrcontribVrcr.Value <= 0))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF\" deve ser maior do que 0 (zero).");
            }

            if (this.InfocrcontribVrcr.HasValue && !regex.IsMatch(this.InfocrcontribVrcr.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF\" deve estar no padrão numérico (12,2).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
