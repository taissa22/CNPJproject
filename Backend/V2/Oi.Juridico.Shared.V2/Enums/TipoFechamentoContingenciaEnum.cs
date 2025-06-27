using System.ComponentModel;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum TipoFechamentoContingenciaEnum
    {        
        [Description("Cível Consumidor")]
        Civel_Consumidor = 1,
        [Description("Cível Estratégico")]
        Civel_Estrategico = 6,
        [Description("Trabalhista por Média")]
        Trabalhista = 7,
        [Description("Juizado Especial")]
        Juizado_Especial = 49,
        [Description("Cível Consumidor por Média")]
        Civel_Consumidor_por_Media = 50,
        [Description("PEX por Média")]
        PEX_por_Media = 51,
    }
}
