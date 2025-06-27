using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class MigracaoEventoCivelConsumidor : Notifiable, IEntity, INotifiable
    {
        private MigracaoEventoCivelConsumidor()
        {
        }

        public MigracaoEventoCivelConsumidor(int id, int eventoCivelConsumidorId, int eventoCivelEstrategicoId, int decisaoEventoCivelConsumidorId, int decisaoEventoCivelEstrategicoId)
        {
            Id = id;
            EventoCivelConsumidorId = eventoCivelConsumidorId;
            EventoCivelEstrategicoId = eventoCivelEstrategicoId;
            DecisaoEventoCivelConsumidorId = decisaoEventoCivelConsumidorId;
            DecisaoEventoCivelEstrategicoId = decisaoEventoCivelEstrategicoId;

        }

        public static MigracaoEventoCivelConsumidor CriarMigracaoEventoCivelConsumidor( int eventoCivelConsumidorId, int eventoCivelEstrategicoId, int? decisaoEventoCivelConsumidorId, int? decisaoEventoCivelEstrategicoId)
        {
            var migracaoEventoCivelConsumidor = new MigracaoEventoCivelConsumidor()
            {
                EventoCivelConsumidorId = eventoCivelConsumidorId,
                EventoCivelEstrategicoId = eventoCivelEstrategicoId,
                DecisaoEventoCivelConsumidorId = decisaoEventoCivelConsumidorId,
                DecisaoEventoCivelEstrategicoId = decisaoEventoCivelEstrategicoId
            };

            migracaoEventoCivelConsumidor.Validate();
            return migracaoEventoCivelConsumidor;
        }

        public void AtualizarMigracaoEventoCivelConsumidor(int id, int eventoCivelConsumidorId, int eventoCivelEstrategicoId, int? decisaoEventoCivelConsumidorId, int? decisaoEventoCivelEstrategicoId) 
        
        {
            Id = id;
            EventoCivelConsumidorId = eventoCivelConsumidorId;
            EventoCivelEstrategicoId = eventoCivelEstrategicoId;
            DecisaoEventoCivelConsumidorId = decisaoEventoCivelConsumidorId;
            DecisaoEventoCivelEstrategicoId = DecisaoEventoCivelEstrategicoId;
        }


        private void Validate()
        {
            if (EventoCivelConsumidorId > 0)
            {
                AddNotification(nameof(EventoCivelConsumidorId), "O campo EventoCivelConsumidorId deve ser informado.");
            }

            if (EventoCivelEstrategicoId > 0)
            {
                AddNotification(nameof(EventoCivelEstrategicoId), "O campo EventoCivelEstrategicoId deve ser informado.");
            }
        }


        public int Id { get; set; }
        public int EventoCivelConsumidorId { get; set; }
        public int EventoCivelEstrategicoId { get; set; }
        public int? DecisaoEventoCivelConsumidorId { get; set; }
        public int? DecisaoEventoCivelEstrategicoId { get; set; }    

    }


}