using FluentValidation;
using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Validator
{
    public class ValorCorteOutlierValidator : AbstractValidator<SolicFechamentoCont>
    {
        public ValorCorteOutlierValidator()
        {
            RuleFor(s => s).Must(f => !(f.ValCorteOutliers != f.ValCorteOutliers && string.IsNullOrEmpty(f.ObsValCorteOutliers))).WithMessage("O valor de corte de outliers foi alterado. Por favor, preencha o campo observação para justificar a alteração.");
            RuleFor(s => s).Must(f => !(f.ObsValCorteOutliers.Length > 2000)).WithMessage("Campo Observação limite de caracteres 2000.");
        }
    }
}
