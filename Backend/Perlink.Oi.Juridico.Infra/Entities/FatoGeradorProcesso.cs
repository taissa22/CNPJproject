using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.ComponentModel.DataAnnotations.Schema;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FatoGeradorProcesso : Notifiable, IEntity, INotifiable
    {
        private FatoGeradorProcesso()
        {
        }

        public FatoGeradorProcesso(int id, int fatoGeradorId)
        {
            Id = id;
            FatoGeradorId = fatoGeradorId;
        }      
     
        public int Id { get; private set; }

        public int FatoGeradorId { get; private set; }
    }
}