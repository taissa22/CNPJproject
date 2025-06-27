using FluentValidation;
using Oi.Juridico.Contextos.V2.FechamentoContingenciaContext.Entities;
using Oi.Juridico.Shared.V2.Enums;

namespace Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Validator
{
    public class SolicFechamentoContValidator : AbstractValidator<SolicFechamentoCont>
    {
        public SolicFechamentoContValidator()
        {
            /*
             WHEN == CONDICIONAL
             RULEFOR == SELECIONAR CAMPO A SER TRATADO
             MUST == VALIDAÇÃO A SER FEITA
             WITHMESSAGE == MENSAGEM EXIBIDA SE HOUVER ERRO NA VALIDAÇÃO DO CAMPO
            */

            When(solic => solic.CodTipoFechamento == 49 || solic.CodTipoFechamento == 51 || (solic.CodTipoFechamento == 7 && solic.CodTipoFechamentoTrab.Contains("TM")), () =>
            {
                RuleFor(s => s.NumeroDeMeses).Must(f => f.HasValue).WithMessage("Nº de meses não informado.");

                When(fech => fech.CodTipoFechamento == 51, () =>
                   {
                       RuleFor(s => s.PercentualHaircut).Must(f => f.HasValue).WithMessage("% de haircut não informado.");
                       RuleFor(s => s.MultDesvioPadrao).Must(f => f.HasValue).WithMessage("Multiplicador desvio padrão não informado.");
                   });
            });

            // VALIDAÇÃO PARA EXECUÇÃO IMEDIATA
            When(fech => fech.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.ExecucaoImediata, () =>
            {
                RuleFor(s => s.MesContabil).Must(f => f.HasValue).WithMessage("Mês contábil não informado.");
                RuleFor(s => s.AnoContabil).Must(f => f.HasValue).WithMessage("Ano contábil não informado.");
                RuleFor(s => s.DataPrevia).Must(f => f.HasValue).WithMessage("Data da Prévia não informado.");

                When(fech => fech.DataPrevia.HasValue, () =>
                {
                    RuleFor(s => s.DataPrevia).Must(f => f!.Value <= DateTime.Now).WithMessage("Data da Prévia não pode ser maior ou igual a data atual.");
                });

                When(fech => fech.MesContabil.HasValue && fech.AnoContabil.HasValue, () =>
                {
                    // LEGADO USA A DATA DA PREVIA COMO RULEFOR
                    RuleFor(s => s).Must(f => f.AnoContabil!.Value != 0 && f.MesContabil!.Value != 0).WithMessage("O mês/ano contábil deve ser preenchido corretamente."); // REVER LOGICA
                    RuleFor(s => s.MesContabil).Must(f => f!.Value > 0 && f.Value < 13).WithMessage("O mês contábil deve ser preenchido corretamente.");
                    RuleFor(s => s.AnoContabil).Must(f => f!.Value > 1900).WithMessage("O ano contábil deve ser preenchido corretamente.");

                    When(fech => fech.AnoContabil!.Value > 0 && fech.MesContabil > 0, () =>
                    {
                        // LEGADO USA A DATA DA PREVIA COMO RULEFOR
                        RuleFor(s => s).Must(f => new DateTime(f.AnoContabil!.Value, f.MesContabil!.Value, 1) <= DateTime.Now).WithMessage("O mês/ano contábil deve ser menor ou igual ao mês/ano corrente.");
                    });
                });
            });

            // VALIDAÇÃO PARA DATA ESPECIFICA
            When(fech => fech.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.DataEspecifica, () =>
            {
                RuleFor(s => s.DataEspecifica).Must(f => f.HasValue).WithMessage("Campo Executar na Data não informado.");

                When(fech => fech.DataEspecifica.HasValue, () =>
                {
                    RuleFor(s => s.DataEspecifica).Must(f => f.Value > DateTime.Now).WithMessage("O campo Executar na data deve ser uma data maior que a data atual.");
                });

                RuleFor(s => s.MesContabil).Must(f => f.HasValue).WithMessage("Mês contábil não informado.");
                RuleFor(s => s.AnoContabil).Must(f => f.HasValue).WithMessage("Ano contábil não informado.");
                RuleFor(s => s.DataPrevia).Must(f => f.HasValue).WithMessage("Data da Prévia não informado.");

                When(fech => fech.DataPrevia.HasValue && fech.DataEspecifica.HasValue, () =>
                {
                    // LEGADO USA A DATA DA PREVIA COMO RULEFOR
                    RuleFor(s => s).Must(f => !(f.DataPrevia!.Value > f.DataEspecifica!.Value)).WithMessage("Data da Prévia não pode ser maior que o campo Executar na Data.");

                    When(fech => fech.MesContabil.HasValue && fech.AnoContabil.HasValue, () =>
                    {
                        // LEGADO USA A DATA DA PREVIA COMO RULEFOR
                        RuleFor(s => s).Must(f => f.AnoContabil!.Value != 0 && f.MesContabil!.Value != 0).WithMessage("O mês/ano contábil deve ser preenchido corretamente.");
                        RuleFor(s => s).Must(f => f.MesContabil!.Value > 0 && f.MesContabil.Value < 13).WithMessage("O mês contábil deve ser preenchido corretamente.");
                        RuleFor(s => s).Must(f => f.AnoContabil!.Value > 1900).WithMessage("O ano contábil deve ser preenchido corretamente.");

                        When(fech => fech.AnoContabil!.Value > 0 && fech.MesContabil!.Value > 0, () =>
                        {
                            // LEGADO USA A DATA DA PREVIA COMO RULEFOR
                            RuleFor(s => s).Must(f => new DateTime(f.AnoContabil!.Value, f.MesContabil!.Value, 1) <= f.DataEspecifica!.Value).WithMessage("O mês/ano contábil deve ser menor ou igual a data prevista para execução.");
                        });
                    });
                });
            });

            // VALIDAÇÃO PARA DIÁRIA
            When(fech => fech.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Diaria, () =>
            {
                // RULE FOR NO LEGADO USA DATA DA PREVIA
                RuleFor(s => s.DataDiariaIni).Must(f => f.HasValue).WithMessage("Data Diária inicial é obrigatória para a periodicidade de execução.");
                RuleFor(s => s.DataDiariaFim).Must(f => f.HasValue).WithMessage("Data Diária final é obrigatória para a periodicidade de execução.");

                When(fech => fech.DataDiariaIni.HasValue, () =>
                {
                    RuleFor(s => s.DataDiariaIni).Must(f => f!.Value.Date >= DateTime.Today).WithMessage("A data inicial não poderá ser menor que a data atual.");
                });

                When(fech => fech.DataDiariaIni.HasValue && fech.DataDiariaFim.HasValue, () =>
                {
                    // RULE FOR NO LEGADO USA DATA DA PREVIA
                    RuleFor(s => s).Must(f => f.DataDiariaIni!.Value <= f.DataDiariaFim!.Value).WithMessage("Data Diária inicial não pode ser maior que a Data Diária final.");
                });
            });

            // VALIDAÇÃO PARA SEMANAL
            When(fech => fech.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Semanal, () =>
            {
                RuleFor(s => s.DiaDaSemana).Must(f => f.HasValue).WithMessage("Dia da semana não informado.");
            });

            // VALIDAÇÃO PARA MENSAL
            When(fech => fech.PeriodicidadeExecucao == (int)PeriodicidadeExecucaoEnum.Mensal, () =>
            {
                When(fech => fech.IndUltimoDiaDoMes == "N", () =>
                {
                    RuleFor(s => s.DiaDoMes).Must(f => f.HasValue).WithMessage("Dia do mês não informado.");
                });
            });
        }
    }
}
