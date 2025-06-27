using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialGrauExposicao
    {
        [Display(Description = "NÃO ENSEJADOR DE APOSENTADORIA ESPECIAL")]
        NaoEnsejador = 1,
        [Display(Description = "ENSEJADOR DE APOSENTADORIA ESPECIAL - FAE15_12% (15 ANOS DE CONTRIBUIÇÃO E ALÍQUOTA DE 12%)")]
        EnsejadorFAE15A12 = 2,
        [Display(Description = "ENSEJADOR DE APOSENTADORIA ESPECIAL - FAE20_09% (20 ANOS DE CONTRIBUIÇÃO E ALÍQUOTA DE 9%)")]
        EnsejadorFAE20A9 = 3,
        [Display(Description = "ENSEJADOR DE APOSENTADORIA ESPECIAL - FAE25_06% (25 ANOS DE CONTRIBUIÇÃO E ALÍQUOTA DE 6%")]
        EnsejadorFAE25A6 = 4
    }
}
