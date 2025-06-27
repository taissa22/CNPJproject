using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoAmbienteAcordo
    {
        [Display(Description = "CCP NO ÂMBITO DE EMPRESA")]
        CPPEmpresa = 1,
        [Display(Description = "CCP NO ÂMBITO DE SINDICATO")]
        CPPSindicado = 2,
        [Display(Description = "NINTER")]
        NINTER = 3
    }
}
