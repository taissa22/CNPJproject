using System.Globalization;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501InfocrirrfRequestDTO
    {
        public int? InfocrcontribTpcr { get; set; }
        public decimal? InfocrcontribVrcr { get; set; }
        public decimal? InfocrcontribVrcr13 { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new(pattern, RegexOptions.None);

            if (!InfocrcontribTpcr.HasValue)
            {
                mensagensErro.Add("O campo \"Código Receita (CR) IRRF\" é obrigatório");
            }

            if (InfocrcontribTpcr <= 0)
            {
                mensagensErro.Add("O campo \"Código Receita (CR) IRRF\" informado é inválido");
            }

            if (!InfocrcontribVrcr.HasValue || InfocrcontribVrcr.HasValue && InfocrcontribVrcr.Value <= 0)
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF - Rendimento Mensal\" deve ser maior ou igual 0 (zero).");
            }

            if (InfocrcontribVrcr13.HasValue && InfocrcontribVrcr13.Value <= 0)
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF - 13º\" deve ser maior ou igual 0 (zero).");
            }

            if (InfocrcontribVrcr.HasValue && !regex.IsMatch(InfocrcontribVrcr.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF\" deve estar no padrão numérico (12,2).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
