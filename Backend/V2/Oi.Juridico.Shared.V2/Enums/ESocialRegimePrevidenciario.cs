using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialRegimePrevidenciario
    {
        [Display(Description = "Regime Geral de Previdência Social – RGPS")]
        RegimeGeral = 1,
        [Display(Description = "Regime Próprio de Previdência Social - RPPS, Regime dos Parlamentares e Sistema de Proteção dos Militares dos Estados/DF")]
        RegimeProprio = 2,
        [Display(Description = "Regime de Previdência Social no exterior")]
        RegimeExterior = 3
    }
}
