using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum EsocialStatusEscritorioContador
    {        
        [Display(Description = "Não iniciado")]
        NaoIniciado = 1,
        [Display(Description = "Pendente")]
        Pendente = 2,
        [Display(Description = "Finalizado")]
        Finalizado = 3

    }

}
