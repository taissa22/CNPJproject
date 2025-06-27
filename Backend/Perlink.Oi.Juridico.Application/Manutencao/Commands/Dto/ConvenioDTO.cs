using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using CNPJVO = Perlink.Oi.Juridico.Infra.ValueObjects.CNPJ;
using Perlink.Oi.Juridico.Infra.Enums;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto
{
    public class ConvenioDTO : Validatable, IValidatable
    {
        public string EstadoId { get; set; } = string.Empty;

        public int Codigo { get; set; }

        public string CNPJ { get; set; } = string.Empty;

        public int BancoDebito { get; set; }

        public int AgenciaDebito { get; set; }

        public string DigitoAgenciaDebito { get; set; } = string.Empty;

        public string ContaDebito { get; set; } = string.Empty;

        public int MCI { get; set; }

        public int AgenciaDepositaria { get; set; }

        public string DigitoAgenciaDepositaria { get; set; } = string.Empty;

        public override void Validate()
        {
            if (!EstadoEnum.IsValid(EstadoId))
            {
                AddNotification(nameof(EstadoId), "Estado inválido.");
            }

            if (!Codigo.HasMaxLength(4))
            {
                AddNotification(nameof(Codigo), "Limite de caracteres exedido");
            }

            if (!CNPJVO.IsValidForSisjur(CNPJ))
            {
                AddNotification(nameof(CNPJ), "CNPJ inválido.");
            }

            if (!BancoDebito.HasMaxLength(9))
            {
                AddNotification(nameof(BancoDebito), "Limite de caracteres exedido");
            }

            if (!AgenciaDebito.HasMaxLength(9))
            {
                AddNotification(nameof(AgenciaDebito), "Limite de caracteres exedido");
            }

            if (!DigitoAgenciaDebito.HasMaxLength(1))
            {
                AddNotification(nameof(DigitoAgenciaDebito), "Limite de caracteres exedido");
            }

            if (!ContaDebito.HasMaxLength(11))
            {
                AddNotification(nameof(ContaDebito), "Limite de caracteres exedido");
            }

            if (!MCI.HasMaxLength(9))
            {
                AddNotification(nameof(MCI), "Limite de caracteres exedido");
            }

            if (!AgenciaDepositaria.HasMaxLength(4))
            {
                AddNotification(nameof(AgenciaDepositaria), "Limite de caracteres exedido");
            }

            if (!DigitoAgenciaDepositaria.HasMaxLength(1))
            {
                AddNotification(nameof(DigitoAgenciaDepositaria), "Limite de caracteres exedido");
            }
        }
    }
}