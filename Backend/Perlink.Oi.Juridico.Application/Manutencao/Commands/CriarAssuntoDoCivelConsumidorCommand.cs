using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarAssuntoDoCivelConsumidorCommand : Validatable, IValidatable {

        public int? IdEstrategico { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public string Proposta { get; set; } = string.Empty;

        public string Negociacao { get; set; } = string.Empty;

        public bool Ativo { get; set; } = false;
        public string CodigoTipoContingencia { get; set; }

        public override void Validate() {
            if (string.IsNullOrEmpty(Descricao)) {
                AddNotification(nameof(Descricao), "O campo deve ser preenchido");
            }

            if (!string.IsNullOrEmpty(Descricao) && !Descricao.HasMaxLength(40)) {
                AddNotification(nameof(Descricao), "Limite de caracteres excedido");
            }

            if (!string.IsNullOrEmpty(Proposta) && !Proposta.HasMaxLength(2000)) {
                AddNotification(nameof(Proposta), "Limite de caracteres excedido");
            }

            if (!string.IsNullOrEmpty(Negociacao) && !Negociacao.HasMaxLength(4000)) {
                AddNotification(nameof(Negociacao), "Limite de caracteres excedido");
            }
        }
    }
}
