using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialRepercussaoProcesso
    {
        [Display(Description = "DECISÃO COM PAGAMENTO DE VERBAS DE NATUREZA REMUNERATÓRIA")]
        DecisaoComPagamento = 1,
        [Display(Description = "DECISÃO SEM PAGAMENTO DE VERBAS DE NATUREZA REMUNERATÓRIA")]
        DecisaoSemPagamento = 2
    }
}
