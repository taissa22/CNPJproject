using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{        
    public enum EsocialTipoPensaoAlimenticia
    {
        [Display(Description = "Não existe pensão alimentícia")]
        NaoExistePensaoAlimenticia = 0,
        [Display(Description = "Percentual de pensão alimentícia")]
        PercentualPensaoAlimenticia = 1,
        [Display(Description = "Valor de pensão alimentícia")]
        ValorPensaoAlimenticia = 2,
        [Display(Description = "Percentual e valor de pensão alimentícia")]
        PercentualValorPensaoAlimenticia = 3
    }
   
}
