using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ApuracaoProcesso : Notifiable, IEntity, INotifiable
    {
        public ApuracaoProcesso()
        {

        }
        public int ProcessoId { get; private set; }
        public int SequecialApuracao { get; private set; }
        public DateTime DataApuracao { get; private set; }
        public string UsuarioApuracaoId { get; private set; }
        public string NomeAquivoDossie { get; private set; }
        public string TextoApuracao { get; private set; }
        public int TipoDocumentoId { get; private set; }

    }
}
