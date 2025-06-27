using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialProcessoEmissao
    {
        [Display(Description = "Aplicativo do empregador")]
        AplicativoEmpregador = 1,
        [Display(Description = "Aplicativo governamental - Simplificado Pessoa Física")]
        AplicativoGovSimplificadoPF = 2,
        [Display(Description = "Aplicativo governamental - Web Geral")]
        AplicativoGovWebGeral = 3,
        [Display(Description = "Aplicativo governamental - Simplificado Pessoa Jurídica")]
        AplicativoGovSimplificadoPJ = 4,
        [Display(Description = "Aplicativo governamental para dispositivos móveis - Empregador Doméstico")]
        AplicativoGovMobileEmpregadorDomestico = 22
       
    }
}
