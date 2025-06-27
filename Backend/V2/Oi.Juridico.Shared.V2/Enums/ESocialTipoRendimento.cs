using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoRendimento
    {
        [Display(Description = "Remuneração mensal")]
        RemuMensal = 11,
        [Display(Description = "13º salário")]
        Salario13 = 12,
        [Display(Description = "RRA")]
        RRA = 18,
        [Display(Description = "Rendimento isento ou não tributável")]
        RemdIsento = 79

        //18 - RRA

        //79 - Rendimento isento ou não tributável

        //11 - Remuneração mensal
        //12 - 13º salário

    }
}
