using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoPlanilha
    {
        [Display(Description = "Em Linhas")]
        Emlinhas = 1,
        [Display(Description = "Em Colunas")]
        EmColunas = 2
    }
}
