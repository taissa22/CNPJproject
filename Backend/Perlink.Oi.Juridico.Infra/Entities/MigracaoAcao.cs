using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
   public sealed class MigracaoAcao : Notifiable, IEntity, INotifiable
    {
        public MigracaoAcao() {

        }     

        public static MigracaoAcao CriarMigracaoAcao(int codAcaoEstrategico, int codAcaoConsumidor) {

            var migracaoAcao = new MigracaoAcao()
            {
                CodAcaoEstrategico = codAcaoEstrategico,
                CodAcaoConsumidor = codAcaoConsumidor
            };

            migracaoAcao.Validate();
            return migracaoAcao;
        }

        public void AtualizarMigracaoAcao(int codAcaoEstrategico, int codAcaoConsumidor)
        {
            CodAcaoEstrategico = codAcaoEstrategico;
            CodAcaoConsumidor = codAcaoConsumidor;
        }


        private void Validate()
        {
            if (CodAcaoEstrategico > 0 )
            {
                AddNotification(nameof(CodAcaoEstrategico), "O campo CodAcaoEstrategico deve ser informado.");
            }

            if (CodAcaoConsumidor > 0)
            {
                AddNotification(nameof(CodAcaoEstrategico), "O campo CodAcaoConsumidor deve ser informado.");
            }
        }    

        public int CodAcaoEstrategico  { get; set; }
        public int CodAcaoConsumidor { get; set; }
    }
}
