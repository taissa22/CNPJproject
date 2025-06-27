using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoPrazoSlaVencendo
    {
        [Display(Description = "Sem prazo Vencendo")]
        SemPrazoVencendo = 0,
        [Display(Description = "Prazo de Segurança")]
        PrazoSeguranca = 1,
        [Display(Description = "Prazo Final")]
        PrazoFinal = 2
    }
}
