namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2501CalctribRequestDTO
    {
        public DateTime CalctribPerref { get; set; }
        public decimal? CalctribVrbccpmensal { get; set; }
        public decimal? CalctribVrbccp13 { get; set; }
        public decimal? CalctribVrrendirrf { get; set; }
        public decimal? CalctribVrrendirrf13 { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            var mensagensErro = new List<string>();

            string pattern = @"^\d{0,12}.\d{0,2}$";

            Regex regex = new (pattern, RegexOptions.None);

            if (this.CalctribPerref > DateTime.Now )                
            {
                mensagensErro.Add("O campo \"Periodo de Referência\" deve ser menor que a data atual.");
            }

            if ((this.CalctribVrbccpmensal.HasValue && this.CalctribVrbccpmensal.Value < 0) || !this.CalctribVrbccpmensal.HasValue)
            {
                mensagensErro.Add("O campo \"Base de Cálculo da Contribuição Previdenciária\" deve ser maior ou igual a zero.");
            }

            if ((this.CalctribVrbccp13.HasValue && this.CalctribVrbccp13.Value < 0) || !this.CalctribVrbccp13.HasValue)

            {
                mensagensErro.Add("O campo \"Base Cálculo Inss 13º\" deve ser maior ou igual a zero.");
            }

            if ((this.CalctribVrrendirrf.HasValue && this.CalctribVrrendirrf.Value < 0) || !this.CalctribVrrendirrf.HasValue)

            {
                mensagensErro.Add("O campo \"Rendimento Tributável IR\" deve ser maior ou igual a zero.");
            }

            if ((this.CalctribVrrendirrf13.HasValue && this.CalctribVrrendirrf13.Value < 0) || !this.CalctribVrrendirrf13.HasValue)

            {
                mensagensErro.Add("O campo \"Rendimento Tributável IR 13°\" deve ser maior ou igual a zero.");
            }
             
            if (this.CalctribVrbccpmensal.HasValue && !regex.IsMatch(this.CalctribVrbccpmensal.Value.ToString()))

            {
                mensagensErro.Add("O campo \"Base de Cálculo da Contribuição Previdenciária\" deve estar no padrão numérico (12,2).");
            }

            if (this.CalctribVrbccp13.HasValue && !regex.IsMatch(this.CalctribVrbccp13.Value.ToString()))

            {
                mensagensErro.Add("O campo \"Base Cálculo Inss 13º\" deve estar no padrão numérico (12,2).");
            }

            if (this.CalctribVrrendirrf.HasValue && !regex.IsMatch(this.CalctribVrrendirrf.Value.ToString()))

            {
                mensagensErro.Add("O campo \"Rendimento Tributável IR\" deve estar no padrão numérico (12,2).");
            }

            if (this.CalctribVrrendirrf13.HasValue && !regex.IsMatch(this.CalctribVrrendirrf13.Value.ToString()))

            {
                mensagensErro.Add("O campo \"Rendimento Tributável IR 13°\" deve estar no padrão numérico (12,2).");
            } 

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}