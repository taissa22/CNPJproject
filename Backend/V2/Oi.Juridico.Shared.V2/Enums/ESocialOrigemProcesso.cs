using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialOrigemProcesso
    {
        [Display(Description = "Processo judicial")]
        ProcessoJudicial = 1, 
        [Display(Description = "Demanda submetida à CCP ou ao NINTER")]
        DemandaCCPNINTER = 2 
    }
}
