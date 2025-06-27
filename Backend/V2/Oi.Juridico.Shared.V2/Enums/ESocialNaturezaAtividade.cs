using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialNaturezaAtividade
    {
        [Display(Description = "Trabalho urbano")]
        TrabalhoUrbano = 1,
        [Display(Description = "Trabalho rural")]
        TrabalhoRural = 2
    }
}
