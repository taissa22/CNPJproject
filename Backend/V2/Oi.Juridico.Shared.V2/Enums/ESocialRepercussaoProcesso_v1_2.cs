using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialRepercussaoProcesso_v1_2
    {
        [Display(Description = "DECISÃO COM REPERCUSSÃO TRIBUTÁRIA E/OU FGTS COM RENDIMENTOS INFORMADOS EM S-2501")]
        DecisaoComPagamento = 1,
        [Display(Description = "DECISÃO SEM REPERCUSSÃO TRIBUTÁRIA OU FGTS")]
        DecisaoSemPagamento = 2,
        [Display(Description = "DECISÃO COM REPERCUSSÃO EXCLUSIVA PARA DECLARAÇÃO DE RENDIMENTOS PARA FINS DE IMPOSTO DE RENDA COM RENDIMENTOS INFORMADOS EM S-2501")]
        Decisaoexclusiva = 3,
        [Display(Description = "DECISÃO COM REPERCUSSÃO EXCLUSIVA PARA DECLARAÇÃO DE RENDIMENTOS PARA FINS DE IMPOSTO DE RENDA COM PAGAMENTO ATRAVÉS DE DEPÓSITO JUDICIAL")]
        DecisaoJudicial = 4,
        [Display(Description = "DECISÃO COM REPERCUSSÃO TRIBUTÁRIA E/OU FGTS COM PAGAMENTO ATRAVÉS DE DEPÓSITO JUDICIAL")]
        DecisaoTributaria = 5
    }
}
