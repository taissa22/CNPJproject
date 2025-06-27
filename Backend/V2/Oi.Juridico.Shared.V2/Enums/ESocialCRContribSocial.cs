using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialCRContribSocial
    {
        [Display(Description = "108251 - CONTRIBUIÇÃO SOCIAL SEGURADO")]
        ContribSocialSegurado = 108251,
        [Display(Description = "113851 - CONTRIBUIÇÃO SOCIAL EMPREGADOR 20% OU 0,0%")]
        ContribSocialEmpregador = 113851,
        [Display(Description = "164651 - RAT-SAT 1, 2% OU 3%")]
        RatSat = 164651,
        [Display(Description = "117051 - SALÁRIO EDUCAÇÃO 3%")]
        SalariaEducacao = 117051,
        [Display(Description = "117651 - INCRA")]
        INCRA = 117651,
        [Display(Description = "118151 - SENAI 2,5%")]
        SENAI = 118151,
        [Display(Description = "118451 - SESI 1,5%")]
        SESI = 118451,
        [Display(Description = "120051 - SEBRAE 0,6%")]
        SEBRAE = 120051



    }
}
