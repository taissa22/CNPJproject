using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoDocumentoMigracaoEstrategico : Notifiable, IEntity, INotifiable
    {
        public TipoDocumentoMigracaoEstrategico()
        {

        }

        public static TipoDocumentoMigracaoEstrategico CriarTipoDocumentoMigracaoEstrategico(int codTipoDocCivelConsumidor, int codTipoDocCivelEstrategico)
        {

            var tipoDocumentoMigracaoEstrategico = new TipoDocumentoMigracaoEstrategico()
            {
                CodTipoDocCivelEstrategico = codTipoDocCivelEstrategico,
                CodTipoDocCivelConsumidor = codTipoDocCivelConsumidor
            };

            tipoDocumentoMigracaoEstrategico.Validate();
            return tipoDocumentoMigracaoEstrategico;
        }

        public void AtualizarAcaoConsumidorEstrategico(int codTipoDocCivelConsumidor, int codTipoDocCivelEstrategico)
        {
            CodTipoDocCivelEstrategico = codTipoDocCivelEstrategico;
            CodTipoDocCivelConsumidor = codTipoDocCivelConsumidor;
        }


        private void Validate()
        {
            if (CodTipoDocCivelEstrategico > 0)
            {
                AddNotification(nameof(CodTipoDocCivelEstrategico), "O campo CodTipoDocCivelEstrategico deve ser informado.");
            }

            if (CodTipoDocCivelConsumidor > 0)
            {
                AddNotification(nameof(CodTipoDocCivelConsumidor), "O campo CodTipoDocCivelConsumidor deve ser informado.");
            }

        }
        public int CodTipoDocCivelEstrategico { get; set; }
        public int CodTipoDocCivelConsumidor { get; set; }

    }
}