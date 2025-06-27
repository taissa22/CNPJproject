namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2501CalctribRequestDTO
    {
        public DateTime CalctribPerref { get; set; }
        public decimal? CalctribVrbccpmensal { get; set; }
        public decimal? CalctribVrbccp13 { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            string pattern = @"^\d{0,12}.\d{0,2}$";

            Regex regex = new(pattern, RegexOptions.None);

            if (CalctribPerref > DateTime.Now)
            {
                mensagensErro.Add("O campo \"Periodo de Referência\" deve ser menor que a data atual.");
            }

            if (CalctribVrbccpmensal.HasValue && CalctribVrbccpmensal.Value < 0 || !CalctribVrbccpmensal.HasValue)
            {
                mensagensErro.Add("O campo \"Base de Cálculo da Contribuição Previdenciária\" deve ser maior ou igual a zero.");
            }

            if (CalctribVrbccp13.HasValue && CalctribVrbccp13.Value < 0 || !CalctribVrbccp13.HasValue)

            {
                mensagensErro.Add("O campo \"Base Cálculo Inss 13º\" deve ser maior ou igual a zero.");
            }

            if (CalctribVrbccpmensal.HasValue && !regex.IsMatch(CalctribVrbccpmensal.Value.ToString()))

            {
                mensagensErro.Add("O campo \"Base de Cálculo da Contribuição Previdenciária\" deve estar no padrão numérico (12,2).");
            }

            if (CalctribVrbccp13.HasValue && !regex.IsMatch(CalctribVrbccp13.Value.ToString()))

            {
                mensagensErro.Add("O campo \"Base Cálculo Inss 13º\" deve estar no padrão numérico (12,2).");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}