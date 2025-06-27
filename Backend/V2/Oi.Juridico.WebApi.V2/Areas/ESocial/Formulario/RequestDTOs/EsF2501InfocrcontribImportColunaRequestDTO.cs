namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
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

            if (!CalctribPerref.HasValue)
            {
                mensagensErro.Add("O campo \"Período de Referência\" é obrigatório.");
            }

            if (CalctribPerref.HasValue && CalctribPerref.Value.Date > DateTime.Today)
            {
                mensagensErro.Add("O campo \"Período de Referência\" informado contem uma data inválida.");
            }

            if (ContribSocialSegurado.HasValue && !regex.IsMatch(ContribSocialSegurado.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (ContribSocialEmpregador.HasValue && !regex.IsMatch(ContribSocialEmpregador.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (RatSat.HasValue && !regex.IsMatch(RatSat.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (SalariaEducacao.HasValue && !regex.IsMatch(SalariaEducacao.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (INCRA.HasValue && !regex.IsMatch(INCRA.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (SENAI.HasValue && !regex.IsMatch(SENAI.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (SESI.HasValue && !regex.IsMatch(SESI.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            if (SEBRAE.HasValue && !regex.IsMatch(SEBRAE.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor Correspondente (CR) Contrib. Sociais\" deve estar no padrão numérico (12,2).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
