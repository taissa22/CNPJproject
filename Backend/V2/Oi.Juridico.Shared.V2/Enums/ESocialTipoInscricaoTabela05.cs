using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoInscricaoTabela05
    {
        [Display(Description="CNPJ")]
        CNPJ = 1,
        [Display(Description = "CPF")]
        CPF = 2,
        [Display(Description = "CAEPF")]
        CAEPF = 3,
        [Display(Description = "CNO")]
        CNO = 4,
        [Display(Description = "CGC")]
        CGC = 5,
        [Display(Description = "CEI")]
        CEI = 6,
    }
}
