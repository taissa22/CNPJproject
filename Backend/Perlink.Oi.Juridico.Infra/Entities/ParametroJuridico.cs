using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ParametroJuridico : Notifiable, IEntity, INotifiable
    {
        private ParametroJuridico()
        {
        }

        public string Parametro { get; private set; }
        public string TipoParametro { get; private set; }
        public string Descricao { get; private set; }
        public string Conteudo { get; private set; }
        public bool UsuarioAtualiza { get; private set; }
        public string Dono { get; private set; }
        public DateTime? DataUltimaUtilizacao { get; private set; }
    }
}