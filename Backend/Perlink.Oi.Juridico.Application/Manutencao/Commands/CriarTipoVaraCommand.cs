using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarTipoVaraCommand : Validatable, IValidatable
    {
        public string Nome { get; set; }

        public bool IndCivelConsumidor { get; set; }

        public bool IndCivelEstrategico { get; set; }

        public bool IndTrabalhista { get; set; }

        public bool IndTributaria { get; set; }

        public bool IndJuizado { get; set; }

        public bool IndCriminalJudicial { get; set; }

        public bool IndProcon { get; set; }

        public override void Validate()
        {
            if (string.IsNullOrEmpty(Nome) || string.IsNullOrWhiteSpace(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }
        }
    }
}