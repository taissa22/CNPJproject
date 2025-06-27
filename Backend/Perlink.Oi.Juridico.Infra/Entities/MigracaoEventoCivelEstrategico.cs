using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class MigracaoEventoCivelEstrategico : Notifiable, IEntity, INotifiable
    {
        private MigracaoEventoCivelEstrategico()
        {
        }

        public MigracaoEventoCivelEstrategico(int id, int eventoCivelEstrategicoId, int eventoCivelConsumidorId, int decisaoCivelEstrategicoId, int decisaoCivelConsumidorId)
        {
            Id = id;
            EventoCivelEstrategicoId = eventoCivelEstrategicoId;
            EventoCivelConsumidorId = eventoCivelConsumidorId;
            DecisaoCivelEstrategicoId = decisaoCivelEstrategicoId;
            DecisaoCivelConsumidorId = decisaoCivelConsumidorId;

        }

        public static MigracaoEventoCivelEstrategico CriarMigracaoEventoCivelEstrategico(int eventoCivelEstrategicoId,int eventoCivelConsumidorId, int? decisaoCivelEstrategicoId, int? decisaoCivelConsumidorId )
        {
            MigracaoEventoCivelEstrategico migracaoEventoCivelEstrategico = new MigracaoEventoCivelEstrategico()

            {
                EventoCivelEstrategicoId = eventoCivelEstrategicoId,
                EventoCivelConsumidorId = eventoCivelConsumidorId,
                DecisaoCivelEstrategicoId = decisaoCivelEstrategicoId,
                DecisaoCivelConsumidorId = decisaoCivelConsumidorId,
            };
            migracaoEventoCivelEstrategico.Validate();
            return migracaoEventoCivelEstrategico;
        }

        public void AlterarMigracaoEventoCivelEstrategico(int id,int eventoCivelEstrategicoId, int eventoCivelConsumidorId, int? decisaoCivelEstrategicoId, int? decisaoCivelConsumidorId)
        {
            Id = id;
            EventoCivelEstrategicoId = eventoCivelEstrategicoId;
            EventoCivelConsumidorId = EventoCivelConsumidorId;
            DecisaoCivelEstrategicoId = decisaoCivelEstrategicoId;
            DecisaoCivelConsumidorId = decisaoCivelConsumidorId;

        }

        private void Validate()
        {
            if (EventoCivelEstrategicoId > 0)
            {
                AddNotification(nameof(EventoCivelEstrategicoId), "O campo EventoCivelEstrategicoId deve ser informado.");
            }

            if (EventoCivelConsumidorId > 0)
            {
                AddNotification(nameof(EventoCivelConsumidorId), "O campo EventoCivelConsumidorId deve ser informado.");
            }
        }


        public int Id { get; set; }
        public int EventoCivelEstrategicoId { get; set; }
        public int EventoCivelConsumidorId { get; set; }
        public int? DecisaoCivelEstrategicoId { get; set; }
        public int? DecisaoCivelConsumidorId { get; set; }
    }
}