using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarAcaoDoCivelConsumidorCommand : Validatable, IValidatable
    {
        public int id { get; set; } = 0;

        public int? IdEstrategico { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int? NaturezaAcaoBBId { get; set; } = null;
        public bool EnviarAppPreposto { get; set; }

        public override void Validate()
        {
            if (id == 0)
            {
                AddNotification(nameof(id), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao))
            {
                AddNotification(nameof(Descricao), "Campo Requerido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(30))
            {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}