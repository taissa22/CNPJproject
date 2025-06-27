using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialRegimeTrabalhista
    {

         [Display(Description = "CLT - Consolidação das Leis de Trabalho e legislações trabalhistas específicas")]
         CLT = 1,
         [Display(Description = "Estatutário/legislações específicas (servidor temporário, militar, agentepolítico, etc.)")]
         Estatutario = 2   
    }
}
