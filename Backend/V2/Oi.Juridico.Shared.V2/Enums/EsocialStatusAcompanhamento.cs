using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum EsocialStatusAcompanhamento
    { 
        
        [Display(Description = "Não iniciado")]
        NaoIniciado = 0,
        [Display(Description = "Enviado")]
        Enviado = 1,
        [Display(Description = "Erro no envio")]
        ErroEnvio = 2,
        [Display(Description = "Retorno OK")]
        RetornoOK = 3
    }
    
}
