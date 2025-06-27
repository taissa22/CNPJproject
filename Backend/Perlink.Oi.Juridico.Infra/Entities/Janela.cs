using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class Janela : Notifiable, IEntity, INotifiable
    {
        public string CodAplicacao { get; set; }
        public string CodJanela { get; set; }
        public string CodMenu { get; set; }
        public Janela()
        {

        }
    }
}
