using System.ComponentModel.DataAnnotations;

namespace Oi.Juridico.Shared.V2.Enums
{
    public enum ESocialTipoContratoTSVE
    {
        [Display(Description = "TRABALHADOR COM VÍNCULO FORMALIZADO, SEM ALTERAÇÃO NAS DATAS DE ADMISSÃO E DE DESLIGAMENTO")]
        TrabVinculoSemAlteracaoDataAdmissaoDesligamento = 1,
        [Display(Description = "TRABALHADOR COM VÍNCULO FORMALIZADO, COM ALTERAÇÃO NA DATA DE ADMISSÃO")]
        TrabVinculoComAlteracaoDataAdmissao = 2,
        [Display(Description = "TRABALHADOR COM VÍNCULO FORMALIZADO, COM INCLUSÃO OU ALTERAÇÃO DE DATA DE DESLIGAMENTO")]
        TrabVinculoComInclusaoAlteracaoDataDesligamento = 3,
        [Display(Description = "TRABALHADOR COM VÍNCULO FORMALIZADO, COM ALTERAÇÃO NAS DATAS DE ADMISSÃO E DE DESLIGAMENTO")]
        TrabVinculoComAlteracaoDataAdmissaoDesligamento = 4,
        [Display(Description = "EMPREGADO COM RECONHECIMENTO DE VÍNCULO")]
        EmpregadoReconhecimentoVinculo = 5,
        [Display(Description = "TRABALHADOR SEM VÍNCULO DE EMPREGO/ESTATUTÁRIO (TSVE), SEM RECONHECIMENTO DE VÍNCULO EMPREGATÍCIO")]
        TrabSemVinculoTSVESemReconhecimentoVinculo = 6,
        [Display(Description = "TRABALHADOR COM VÍNCULO DE EMPREGO FORMALIZADO EM PERÍODO ANTERIOR AO ESOCIAL")]
        TrabComVinculoAnteriorESocial = 7,
        [Display(Description = "RESPONSABILIDADE INDIRETA")]
        ResponsabilidadeIndireta = 8,
        [Display(Description = "TRABALHADOR CUJOS CONTRATOS FORAM UNIFICADOS (UNICIDADE CONTRATUAL)")]
        TrabComUnicidadeContratual = 9
    }
}
