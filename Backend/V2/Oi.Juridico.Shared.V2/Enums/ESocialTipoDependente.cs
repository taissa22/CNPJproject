using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoDependente
    {
        [Display(Description = "CÔNJUGE")]
        Conjuge = 1,
        [Display(Description = "COMPANHEIRO(A) COM O(A) QUAL TENHA FILHO OU VIVA HÁ MAIS DE 5 (CINCO) ANOS OU POSSUA DECLARAÇÃO DE UNIÃO ESTÁVEL")]
        CompanheiroComFilhoUniaoEstavel = 2,
        [Display(Description = "FILHO(A) OU ENTEADO(A)")]
        FilhoEnteado = 3,
        [Display(Description = "FILHO(A) OU ENTEADO(A), UNIVERSITÁRIO(A) OU CURSANDO ESCOLA TÉCNICA DE 2º GRAU")]
        FilhoEnteadoUniversitarioEscolaTecnica = 4,
        [Display(Description = "IRMÃO(Ã), NETO(A) OU BISNETO(A) SEM ARRIMO DOS PAIS, DO(A) QUAL DETENHA A GUARDA JUDICIAL")]
        IrmaoNetoBisnetoSemArrimo = 6,
        [Display(Description = "IRMÃO(Ã), NETO(A) OU BISNETO(A) SEM ARRIMO DOS PAIS, UNIVERSITÁRIO(A) OU CURSANDO ESCOLA TÉCNICA DE 2° GRAU, DO(A) QUAL DETENHA A GUARDA JUDICIAL")]
        IrmaoNetoBisnetoSemArrimoUniversitarioEscolaTecnica = 7,
        [Display(Description = "PAIS, AVÓS E BISAVÓS")]
        PaisAvosBisavos = 9,
        [Display(Description = "MENOR POBRE DO QUAL DETENHA A GUARDA JUDICIAL")]
        MenorPobreGuardaJudicial = 10,
        [Display(Description = "A PESSOA ABSOLUTAMENTE INCAPAZ, DA QUAL SEJA TUTOR OU CURADOR")]
        IncapalTutorCurador = 11,
        [Display(Description = "EX-CÔNJUGE")]
        ExConjuge = 12,
        [Display(Description = "AGREGADO/OUTROS")]
        AgregadoOutros = 99
    }
}
