using FluentValidation;
using Shared.Domain.Impl.Entity;
using Shared.Domain.Impl.Validator;
using System;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class AreaEnvolvida : EntityCrud<AreaEnvolvida, long>
    {
        public override AbstractValidator<AreaEnvolvida> Validator => throw new NotImplementedException();
        public string Nome { get; set; }
        public bool IndAtivo { get; set; }
        public bool IndCivelEstrategico { get; set; }
        public bool IndCivelConsumidor { get; set; }
        public bool IndJec { get; set; }
        public bool IndTrabalhista { get; set; }
        public bool IndTribJudicial { get; set; }
        public bool IndTribAdm { get; set; }
        public bool IndCivelAdm { get; set; }
        public bool IndTrabAdm { get; set; }
        public bool IndAdm { get; set; }
        public bool IndProcon { get; set; }
        public bool IndCrimJudicial { get; set; }
        public bool IndCrimAdm { get; set; }

        public override void PreencherDados(AreaEnvolvida data)
        {
            Nome = data.Nome;
            IndAtivo = data.IndAtivo;
            IndCivelEstrategico = data.IndCivelEstrategico;
            IndCivelConsumidor = data.IndCivelConsumidor;
            IndJec = data.IndJec;
            IndTrabalhista = data.IndTrabalhista;
            IndTribJudicial = data.IndTribJudicial;
            IndTribAdm = data.IndTribAdm;
            IndCivelAdm = data.IndCivelAdm;
            IndTrabAdm = data.IndTrabAdm;
            IndAdm = data.IndAdm;
            IndProcon = data.IndProcon;
            IndCrimJudicial = data.IndCrimJudicial;
            IndCrimAdm = data.IndCrimAdm;
        }

        public override ResultadoValidacao Validar()
        {
            return ExecutarValidacaoPadrao(this);
        }
    }

    internal class AreaEnvolvidaValidator : AbstractValidator<AreaEnvolvida>
    {
        public AreaEnvolvidaValidator()
        {

        }
    }
}
