using System.ComponentModel;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Enum {
    public enum TipoProcessoEnum {
        [Description("Cível Consumidor")]
        CivelConsumidor = 1,
        [Description("Trabalhista")]
        Trabalhista = 2,
        [Description("Administrativo")]
        Administrativo = 3,
        [Description("Tributário Administrativo")]
        TributarioAdministrativo = 4,
        [Description("Tributário Judicial")]
        TributarioJudicial = 5,
        [Description("Trabalhista Administrativo")]
        TrabalhistaAdministrativo = 6,
        [Description("Juizado Especial Cível")]
        JuizadoEspecial = 7,
        [Description("Expressinho")]
        Expressinho = 8,
        [Description("Cível Estratégico")]
        CivelEstrategico = 9,
        [Description("Descumprimento Administrativo")]
        Descumprimento = 10,
        [Description("Denuncia Fiscal")]
        DenunciaFiscal = 11,
        [Description("Cível Administrativo")]
        CivelAdministrativo = 12,
        [Description("Criminal")]
        Criminal = 13,
        [Description("Criminal Administrativo")]
        CriminalAdministrativo = 14,
        [Description("Criminal Judicial")]
        CriminalJudicial = 15,
        [Description("Ofício Cível Administrativo")]
        OficioCivel = 16,
        [Description("Procon")]
        Procon = 17,
        [Description("Pex")]
        Pex = 18,
        [Description("Serviços")]
        Servicos = 30
    }
}
