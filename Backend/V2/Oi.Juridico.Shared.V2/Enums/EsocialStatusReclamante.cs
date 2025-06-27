using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum EsocialStatusReclamante
    {
        [Display(Description = "Pendente de análise")]
        PendenteAnalise = 0,
        [Display(Description = "Não elegível para eSocial")]
        NaoElegivelESocial = 1,
        [Display(Description = "Elegível para eSocial")]
        ElegivelESocial = 2        
    }
}
