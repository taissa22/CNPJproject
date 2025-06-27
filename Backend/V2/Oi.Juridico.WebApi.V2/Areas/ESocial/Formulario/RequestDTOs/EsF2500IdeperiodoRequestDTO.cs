namespace Oi.Juridico.WebApi.V2.Areas.ESocial.Formulario.RequestDTOs
{
    public class EsF2500IdeperiodoRequestDTO
    {
        public DateTime IdeperiodoPerref { get; set; }
        public decimal? BasecalculoVrbccpmensal { get; set; }
        public decimal? BasecalculoVrbccp13 { get; set; }
        public byte? InfoagnocivoGrauexp { get; set; }
        public short? BasemudcategCodcateg { get; set; }
        public decimal? BasemudcategVrbccprev { get; set; }
        public decimal? InfofgtsVrbcfgtsproctrab { get; set; }
        public decimal? InfofgtsVrbcfgtssefip { get; set; }
        public decimal? InfofgtsVrbcfgtsdecant { get; set; }


        public (bool Invalido, IEnumerable<string> ListaErros) Validar()
        {
            string pattern = @"^\d{0,12}.\d{0,2}$";
            Regex regex = new(pattern, RegexOptions.None);

            var mensagensErro = new List<string>();

            if (BasecalculoVrbccpmensal.HasValue && BasecalculoVrbccpmensal.Value < 0 || !BasecalculoVrbccpmensal.HasValue)
            {
                mensagensErro.Add("O campo \"Base Cálculo INSS Mensal\" deve ser maior ou igual a 0 (zero).");
            }

            if (BasecalculoVrbccpmensal.HasValue && !regex.IsMatch(BasecalculoVrbccpmensal.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Base Cálculo INSS Mensal\" deve estar no padrão numérico (12,2).");
            }

            if (BasecalculoVrbccp13.HasValue && BasecalculoVrbccp13.Value < 0 || !BasecalculoVrbccp13.HasValue)
            {
                mensagensErro.Add("O campo \"Base Cálculo INSS 13º\" deve ser maior ou igual a 0 (zero).");
            }

            if (BasecalculoVrbccp13.HasValue && !regex.IsMatch(BasecalculoVrbccp13.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Base Cálculo INSS 13º\" deve estar no padrão numérico (12,2).");
            }

            if (InfoagnocivoGrauexp.HasValue && (InfoagnocivoGrauexp.Value < 1 || InfoagnocivoGrauexp.Value > 4))
            {
                mensagensErro.Add("O campo do código que representa o \"Grau de Exposição\" inválido.");
            }

            if (BasemudcategCodcateg.HasValue && BasemudcategCodcateg.Value < 0)
            {
                mensagensErro.Add("O campo \"Código da Categoria\" do trabalhador é obrigatório.");
            }

            if (BasemudcategVrbccprev.HasValue && BasemudcategVrbccprev.Value <= 0)
            {
                mensagensErro.Add("O campo \"Base de Cálculo INSS Mudança de Categoria\" deve ser maior do que 0 (zero).");
            }

            if (BasemudcategVrbccprev.HasValue && !regex.IsMatch(BasemudcategVrbccprev.Value.ToString()))
            {
                mensagensErro.Add("O campo \"Base de Cálculo INSS Mudança de Categoria\" deve estar no padrão numérico (12,2).");
            }

            if (
                BasemudcategCodcateg.HasValue && !BasemudcategVrbccprev.HasValue ||
                BasemudcategVrbccprev.HasValue && !BasemudcategCodcateg.HasValue
               )
            {
                mensagensErro.Add("Para informar dados sobre o grupo Bases de cálculo já declaradas em GFIP, no caso de reconhecimento de mudança de código de categoria, é obrigatório preencher os campos: Categoria, Base Cálculo Inss Mensal.");
            }

            return (mensagensErro.Count > 0, mensagensErro);
        }
    }
}
