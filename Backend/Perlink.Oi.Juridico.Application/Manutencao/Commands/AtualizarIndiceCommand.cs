using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarIndiceCommand : Validatable, IValidatable
    {
        public int CodigoIndice { get; set; }

        public string Descricao { get; set; }

        public bool Mensal { get; set; }

        public string CodigoValorIndice { get; set; }

        public bool Acumulado { get; set; }

        public bool AcumuladoAutomatico { get; set; }

        public override void Validate()
        {
            if (CodigoIndice <= 0)
            {
                AddNotification(nameof(CodigoIndice), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(CodigoValorIndice))
            {
                AddNotification(nameof(CodigoValorIndice), "Campo Requerido");
            }
        }
    }
}