namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2501InfocrcontribImportColunaRequestDTO
    {
        public DateTime? CalctribPerref { get; set; }
        public decimal? ContribSocialSegurado { get; set; }
        public decimal? ContribSocialEmpregador { get; set; }
        public decimal? RatSat { get; set; }
        public decimal? SalariaEducacao { get; set; }
        public decimal? INCRA { get; set; }
        public decimal? SENAI { get; set; }
        public decimal? SESI { get; set; }
        public decimal? SEBRAE { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new(pattern, RegexOptions.None);

            var mensagensErro = new List<string>();

            if (!this.CalctribPerref.HasValue)
            {
                mensagensErro.Add("O campo \"Período de Referência\" é obrigatório.");
            }

            if (this.CalctribPerref.HasValue && this.CalctribPerref.Value.Date > DateTime.Today)
            {
                mensagensErro.Add("O campo \"Período de Referência\" informado contem uma data inválida.");
            }

            if (this.ContribSocialSegurado.HasValue && !regex.IsMatch(this.ContribSocialSegurado.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.ContribSocialEmpregador.HasValue && !regex.IsMatch(this.ContribSocialEmpregador.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.RatSat.HasValue && !regex.IsMatch(this.RatSat.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.SalariaEducacao.HasValue && !regex.IsMatch(this.SalariaEducacao.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.INCRA.HasValue && !regex.IsMatch(this.INCRA.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.SENAI.HasValue && !regex.IsMatch(this.SENAI.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.SESI.HasValue && !regex.IsMatch(this.SESI.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (this.SEBRAE.HasValue && !regex.IsMatch(this.SEBRAE.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
