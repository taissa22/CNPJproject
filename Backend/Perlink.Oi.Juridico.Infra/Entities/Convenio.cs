using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.Validators.PrimitiveValidators;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using CNPJVO = Perlink.Oi.Juridico.Infra.ValueObjects.CNPJ;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Convenio : Notifiable, IEntity, INotifiable
    {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        private Convenio()
        {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        public static Convenio Criar(EstadoEnum estado, int codigo,
            CNPJ cnpj, int bancoDebito, int agenciaDebito, DataString digitoAgenciaDebito, DataString contaDebito,
            int mci, int agenciaDepositaria, DataString digitoAgenciaDepositaria)
        {
            var convenio = new Convenio()
            {
                EstadoId = estado.Id,
                Codigo = codigo,
                CNPJ = cnpj.ToString(),
                BancoDebito = bancoDebito,
                AgenciaDebito = agenciaDebito,
                DigitoAgenciaDebito = digitoAgenciaDebito,
                ContaDebito = contaDebito,
                MCI = mci,
                AgenciaDepositaria = agenciaDepositaria,
                DigitoAgenciaDepositaria = digitoAgenciaDepositaria
            };
            convenio.Validate();
            return convenio;
        }

        internal int CodigoEmpresaCentralizadora { get; private set; }
        internal EmpresaCentralizadora EmpresaCentralizadora { get; private set; }

        internal string EstadoId { get; private set; }
        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);

        public int Codigo { get; private set; }
        public string CNPJ { get; private set; }
        public int BancoDebito { get; private set; }
        public int AgenciaDebito { get; private set; }
        public string DigitoAgenciaDebito { get; private set; }
        public string ContaDebito { get; private set; }

        public int MCI { get; private set; }
        public int AgenciaDepositaria { get; private set; }
        public string DigitoAgenciaDepositaria { get; private set; }

        public void Validate()
        {
            ClearNotifications();
            if (EmpresaCentralizadora is null)
            {
                AddNotification(nameof(EmpresaCentralizadora), "O Convênio deve ser adicionado a uma Empresa Centralizadora.");
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

        internal void AdicionarEmpresaCentralizadora(EmpresaCentralizadora empresaCentralizadora)
        {
            if (EmpresaCentralizadora != null)
            {
                throw new InvalidOperationException("Não se pode mudar a Empresa Centralizadora de um Convênio");
            }
            EmpresaCentralizadora = empresaCentralizadora;
            CodigoEmpresaCentralizadora = empresaCentralizadora.Codigo;
            Validate();
        }
    }
}