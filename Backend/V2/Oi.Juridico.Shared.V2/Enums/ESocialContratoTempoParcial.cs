using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialContratoTempoParcial
    {
        [Display(Description = "Não é contrato em tempo parcial")]
        NaoParcial = 0,
        [Display(Description = "Limitado a 25 horas semanais")]
        Limitado25Horas = 1,
        [Display(Description = "Limitado a 30 horas semanais")]
        Limitado30Horas = 2,
        [Display(Description = "Limitado a 26 horas semanais")]
        Limitado26Horas = 3

   
    }
}
