using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class PercentualATM : Notifiable, IEntity, INotifiable
    {
        private PercentualATM()
        {
        }

        public int Id { get; private set; }
        public string EstadoId { get; set; }
        public string NomeEstado => EstadoEnum.PorId(EstadoId).Nome;
        public EstadoEnum Estado => EstadoEnum.PorId(EstadoId);
        public Decimal Percentual { get; private set; }
        public DateTime DataVigencia { get; private set; }
        public int CodTipoProcesso { get; private set; }

        public static PercentualATM CriarPercentualAtm(EstadoEnum estado, string percentual, DateTime dataVigencia, int codTipoProcesso)
        {
            var percentualATM = new PercentualATM()
            {
                EstadoId = estado.Id,
                Percentual = string.IsNullOrEmpty(percentual) ? 0 : Convert.ToDecimal(percentual.Replace(".", ",").Trim()),
                DataVigencia = dataVigencia,
                CodTipoProcesso = codTipoProcesso
            };
            percentualATM.Validate();
            return percentualATM;
        }

        public void AtualizarPercentualAtm(decimal percentual, int codTipoProcesso)
        {
            Percentual = percentual;
            CodTipoProcesso = codTipoProcesso;
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(EstadoId))
            {
                AddNotification(nameof(Estado), "O Campo Sigla de Estado é obrigatório.");
            }

            if (Percentual < 1)
            {
                AddNotification(nameof(Percentual), "Não podem haver percentuais vazios ou zero.");
            }

            if (DataVigencia == null)
            {
                AddNotification(nameof(DataVigencia), "O Campo Data de Vigência é obrigatório.");
            }
        }
    }
}