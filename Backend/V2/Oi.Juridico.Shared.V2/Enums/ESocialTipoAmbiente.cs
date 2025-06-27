using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoAmbiente
    {
        [Display(Description = "Produção")]
        Producao = 1,
        [Display(Description = "Produção restrita")]
        ProducaoRestrita = 2
    }
}
