using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands {

    public class AtualizarPedidoDoTrabalhistaCommand : Validatable, IValidatable {
        public int PedidoId { get; set; } = 0;

        public string Descricao { get; set; } = string.Empty;

        public string RiscoPerdaId { get; set; } = string.Empty;

        public bool ProvavelZero { get; set; } = false;

        public string ProprioTerceiroId { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;

        public override void Validate() {
            if (PedidoId == 0) {
                AddNotification(nameof(PedidoId), "Campo Requerido");
            }

            if (ProprioTerceiroId == string.Empty) {
                AddNotification(nameof(PedidoId), "Campo Requerido");
            }

            if (string.IsNullOrEmpty(Descricao)) {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(50)) {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }
        }
    }
}