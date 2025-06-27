namespace Oi.Juridico.WebApi.V2.Areas.ESocial.v1_1.RequestDTOs
{
    public class EsF2500IdeperiodoRequestDTO
    {
        public DateTime IdeperiodoPerref { get; set; }
        public decimal? BasecalculoVrbccpmensal { get; set; }
        public decimal? BasecalculoVrbccp13 { get; set; }
        public decimal? BasecalculoVrbcfgts { get; set; }
        public decimal? BasecalculoVrbcfgts13 { get; set; }
        public byte? InfoagnocivoGrauexp { get; set; }
        public decimal? InfofgtsVrbcfgtsguia { get; set; }
        public decimal? InfofgtsVrbcfgts13guia { get; set; }
        public string? InfofgtsPagdireto { get; set; }
        public short? BasemudcategCodcateg { get; set; }
        public decimal? BasemudcategVrbccprev { get; set; }

        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new (pattern, RegexOptions.None);

            var mensagensErro = new List<string>();       

            if ((this.BasecalculoVrbccpmensal.HasValue && this.BasecalculoVrbccpmensal.Value < 0) || !this.BasecalculoVrbccpmensal.HasValue) 
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo da Contribuição Previdênciaria\" deve ser maior ou igual a 0 (zero).");
            }

            if (this.BasecalculoVrbccpmensal.HasValue && !regex.IsMatch(this.BasecalculoVrbccpmensal.Value.ToString())) 
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo da Contribuição Previdênciaria\" deve estar no padrão numérico (12,2).");
            }

            if ((this.BasecalculoVrbccp13.HasValue && this.BasecalculoVrbccp13.Value < 0) || !this.BasecalculoVrbccp13.HasValue)
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo da Contribuição Previdênciaria Referente ao 13º\" deve ser maior ou igual a 0 (zero).");
            }

            if (this.BasecalculoVrbccp13.HasValue && !regex.IsMatch(this.BasecalculoVrbccp13.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo da Contribuição Previdênciaria Referente ao 13º\" deve estar no padrão numérico (12,2).");
            }

            if ((this.BasecalculoVrbcfgts.HasValue && this.BasecalculoVrbcfgts.Value < 0) || !this.BasecalculoVrbcfgts.HasValue)
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálcul do FGTS sobre a Remuneração do Trabalhador\" deve ser maior ou igual a 0 (zero).");
            }

            if (this.BasecalculoVrbcfgts.HasValue && !regex.IsMatch(this.BasecalculoVrbcfgts.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS sobre a Remuneração do Trabalhador\" deve estar no padrão numérico (12,2).");
            }

            if ((this.BasecalculoVrbcfgts13.HasValue && this.BasecalculoVrbcfgts13.Value < 0) || !this.BasecalculoVrbcfgts13.HasValue)
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS sobre a Remuneração do Trabalhador Referente ao 13º\" deve ser maior ou igual a 0 (zero).");
            }

            if (this.BasecalculoVrbcfgts13.HasValue && !regex.IsMatch(this.BasecalculoVrbcfgts13.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS sobre a Remuneração do Trabalhador Referente ao 13º\" deve estar no padrão numérico (12,2).");
            }

            if (this.InfoagnocivoGrauexp.HasValue && (this.InfoagnocivoGrauexp.Value < 1 || this.InfoagnocivoGrauexp.Value > 4))
            {
                mensagensErro.Add("O campo do código que representa o \"Grau de Exposição a Agentes Nocivos\" inválido.");
            }

            if (this.InfofgtsVrbcfgtsguia.HasValue && this.InfofgtsVrbcfgtsguia.Value < 0)
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS para Geração de Guia\" deve ser maior ou igual a 0 (zero).");
            }

            if (this.InfofgtsVrbcfgtsguia.HasValue && !regex.IsMatch(this.InfofgtsVrbcfgtsguia.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS para Geração de Guia\" deve estar no padrão numérico (12,2).");
            }

            if (this.InfofgtsVrbcfgts13guia.HasValue && this.InfofgtsVrbcfgts13guia.Value < 0)
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS Referente o 13º para Geração de Guia\" deve ser maior ou igual a 0 (zero).");
            }

            if (this.InfofgtsVrbcfgts13guia.HasValue && !regex.IsMatch(this.InfofgtsVrbcfgts13guia.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo do FGTS Referente o 13º para Geração de Guia\" deve estar no padrão numérico (12,2).");
            }

            if (this.InfofgtsVrbcfgtsguia.HasValue && this.InfofgtsPagdireto is null)
            {
                mensagensErro.Add("O campo \"Indicativo de FGTS Pago Direto ao Trabalhador\" é obrigatório, caso informado dados para guia de FGTS.");
            }

            if (this.BasemudcategCodcateg.HasValue && this.BasemudcategCodcateg.Value < 0)
            {
                mensagensErro.Add("O campo \"Código da Categoria\" do trabalhador é obrigatório.");
            }

            if (this.BasemudcategVrbccprev.HasValue && this.BasemudcategVrbccprev.Value <= 0)
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo da Contribuição Previdênciaria da Mudança de Categoria\" deve ser maior do que 0 (zero).");
            }

            if (this.BasemudcategVrbccprev.HasValue && !regex.IsMatch(this.BasemudcategVrbccprev.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Valor da Base de Cálculo da Contribuição Previdênciaria da Mudança de Categoria\" deve estar no padrão numérico (12,2).");
            }

            if ((this.InfofgtsPagdireto != null && this.InfofgtsPagdireto != string.Empty) && (this.InfofgtsPagdireto != "S" && this.InfofgtsPagdireto != "N"))
            {
                mensagensErro.Add("O campo \"FGTS Pago Direto Trabalhador\" apresenta dados diferentes do esperado.");
            }

            if (this.InfofgtsVrbcfgtsguia.HasValue && (this.InfofgtsPagdireto == null || this.InfofgtsPagdireto == string.Empty))
            {
                mensagensErro.Add("O campo \"FGTS Pago Direto Trabalhador\" é obrigatório se o campo \"Base Cálculo FGTS GUIA\" estiver preenchido.");
            }

            if ((this.InfofgtsVrbcfgtsguia.HasValue && (!this.InfofgtsVrbcfgts13guia.HasValue || string.IsNullOrEmpty(this.InfofgtsPagdireto))) ||
                (this.InfofgtsVrbcfgts13guia.HasValue && (!this.InfofgtsVrbcfgtsguia.HasValue || string.IsNullOrEmpty(this.InfofgtsPagdireto))) ||
                (!string.IsNullOrEmpty(this.InfofgtsPagdireto) && (!this.InfofgtsVrbcfgts13guia.HasValue || !this.InfofgtsVrbcfgtsguia.HasValue)))
            {
                mensagensErro.Add("Para informar dados sobre o grupo  Informações referentes a bases de cálculo de FGTS para geração de guia, é obrigatório preencher os campos: Base Cálculo FGTS GUIA, Base Cálculo FGTS 13º GUIA e FGTS Pago Direto Trabalhador.");
            }

            if (
                (this.BasemudcategCodcateg.HasValue && !this.BasemudcategVrbccprev.HasValue ) ||
                (this.BasemudcategVrbccprev.HasValue && !this.BasemudcategCodcateg.HasValue )
               )
            {
                mensagensErro.Add("Para informar dados sobre o grupo Bases de cálculo já declaradas em GFIP, no caso de reconhecimento de mudança de código de categoria, é obrigatório preencher os campos: Categoria, Base Cálculo Inss Mensal.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
