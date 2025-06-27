using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoContrato
    {
        [Display(Description = "Prazo indeterminado")]
        PrazoIndeterminado = 1,
        [Display(Description = "Prazo determinado, definido em dias")]
        PrazoDeterminadoDias = 2,
        [Display(Description = "Prazo determinado, vinculado à ocorrência de um fato")]
        PrazoDeterminadoFato = 3


    }
}

