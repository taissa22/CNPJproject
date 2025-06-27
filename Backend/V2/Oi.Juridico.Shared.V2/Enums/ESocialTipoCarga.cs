using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoCarga
    {
        [Display(Description = "Carga Padrão")]
        CargaPadrao = 1,
        [Display(Description = "Apagar Registros")]
        ApagarRegistros = 2
    }
}
