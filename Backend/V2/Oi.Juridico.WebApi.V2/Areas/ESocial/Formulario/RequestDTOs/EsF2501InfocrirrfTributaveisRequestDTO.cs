using System.Globalization;

namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501InfocrirrfTributaveisRequestDTO
    {
        public decimal? InfoirVrrendtrib { get; set; }
        public decimal? InfoirVrrendtrib13 { get; set; }
        public decimal? InfoirVrrendmolegrave { get; set; }
        public decimal? InfoirVrrendisen65 { get; set; }
        public decimal? InfoirVrjurosmora { get; set; }
        public decimal? InfoirVrrendisenntrib { get; set; }
        public string? InfoirDescisenntrib { get; set; }
        public decimal? InfoirVrprevoficial { get; set; }

        public decimal? InfoirVrrendmolegrave13 { get; set; }
        public decimal? InfoirRrendisen65dec { get; set; }
        public decimal? InfoirVrjurosmora13 { get; set; }
        public decimal? InfoirVrprevoficial13 { get; set; }

        public decimal? InfoirVlrDiarias { get; set; }
        public decimal? InfoirVlrAjudaCusto { get; set; }
        public decimal? InfoirVlrIndResContrato { get; set; }
        public decimal? InfoirVlrAbonoPec { get; set; }
        public decimal? InfoirVlrAuxMoradia { get; set; }


        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new(pattern, RegexOptions.None);

            //if (!this.InfocrcontribTpcr.HasValue)
            //{
            //    mensagensErro.Add("O campo \"Código Receita (CR) IRRF\" é obrigatório");
            //}

            //if (this.InfocrcontribTpcr <= 0)
            //{
            //    mensagensErro.Add("O campo \"Código Receita (CR) IRRF\" informado é inválido");
            //}

            //if (!this.InfocrcontribVrcr.HasValue || (this.InfocrcontribVrcr.HasValue && this.InfocrcontribVrcr.Value <= 0))
            //{
            //    mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF\" deve ser maior do que 0 (zero).");
            //}

            //if (this.InfocrcontribVrcr.HasValue && !regex.IsMatch(this.InfocrcontribVrcr.Value.ToString()))
            //{
            //    mensagensErro.Add("O campo \"Valor Correspondente (CR) IRRF\" deve estar no padrão numérico (12,2).");
            //}

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
