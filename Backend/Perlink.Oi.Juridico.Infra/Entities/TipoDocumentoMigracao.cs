using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoDocumentoMigracao : Notifiable, IEntity, INotifiable
    {
        public TipoDocumentoMigracao()
        {

        }

        public static TipoDocumentoMigracao CriarTipoDocumentoMigracao(int codTipoDocCivelConsumidor, int codTipoDocCivelEstrategico)
        {

            var tipoDocumentoMigracao = new TipoDocumentoMigracao()
            {
                CodTipoDocCivelConsumidor = codTipoDocCivelConsumidor,
                CodTipoDocCivelEstrategico = codTipoDocCivelEstrategico
            };

            tipoDocumentoMigracao.Validate();
            return tipoDocumentoMigracao;
        }

        public void AtualizarAcaoConsumidor(int codTipoDocCivelConsumidor, int codTipoDocCivelEstrategico)
        {
            CodTipoDocCivelConsumidor = codTipoDocCivelConsumidor;
            CodTipoDocCivelEstrategico = codTipoDocCivelEstrategico;
        }


        private void Validate()
        {
            if (CodTipoDocCivelConsumidor > 0)
            {
                AddNotification(nameof(CodTipoDocCivelConsumidor), "O campo CodTipoDocCivelConsumidor deve ser informado.");
            }

            if (CodTipoDocCivelEstrategico > 0)
            {
                AddNotification(nameof(CodTipoDocCivelEstrategico), "O campo CodTipoDocCivelEstrategico deve ser informado.");
            }
        }

        public int CodTipoDocCivelConsumidor { get; set; }
        public int CodTipoDocCivelEstrategico { get; set; }
    }
}