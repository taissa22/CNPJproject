using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialIndRetif
    {
        [Display(Description = "Original")]
        Original = 1,
        [Display(Description = "Retificação")]
        Retificacao = 2,
        [Display(Description = "Exclusão")]
        Exclusao = 3
    }
}
