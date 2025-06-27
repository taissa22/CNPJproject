using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum TiposProcessos
    {
        [Display(Name = "Civel Consumidor", Description = "Cível Consumidor")]
        CivelConsumidor = 1,
        [Display(Name = "Trabalhista", Description = "Trabalhista")]
        Trabalhista = 2,
        [Display(Name = "Administrativo", Description = "Administrativo")]
        Administrativo = 3,
        [Display(Name = "Tributario Administrativo", Description = "Tributário Administrativo")]
        TributarioAdministrativo = 4,
        [Display(Name = "Tributario Judicial", Description = "Tributário Judicial")]
        TributarioJudicial = 5,
        [Display(Name = "Trabalhista Administrativo", Description = "Trabalhista Administrativo")]
        TrabalhistaAdministrativo = 6,
        [Display(Name = "JuizadoEspecial", Description = "Juizado Especial")]
        JuizadoEspecial = 7,
        [Display(Name = "Expressinho", Description = "Expressinho")]
        Expressinho = 8,
        [Display(Name = "Civel Estrategico", Description = "Cível Estratégico")]
        CivelEstrategico = 9,
        [Display(Name = "Descumprimento", Description = "Descumprimento Administrativo")]
        Descumprimento = 10,
        [Display(Name = "Denuncia Fiscal", Description = "Denuncia Fiscal")]
        DenunciaFiscal = 11,
        [Display(Name = "Civel Administrativo", Description = "Cível Administrativo")]
        CivelAdministrativo = 12,
        [Display(Name = "Criminal", Description = "Criminal")]
        Criminal = 13,
        [Display(Name = "Criminal Administrativo", Description = "Criminal Administrativo")]
        CriminalAdministrativo = 14,
        [Display(Name = "Criminal Judicial", Description = "Criminal Judicial")]
        CriminalJudicial = 15,
        [Display(Name = "Ofício Cível Administrativo", Description = "Ofício Cível Administrativo")]
        OficioCivel = 16,
        [Display(Name = "Procon", Description = "Procon")]
        Procon = 17,
        [Display(Name = "PEX", Description = "PEX")]
        PEX = 18,
        [Display(Name = "Serviços", Description = "Serviços")]
        Servicos = 30


    }
}
