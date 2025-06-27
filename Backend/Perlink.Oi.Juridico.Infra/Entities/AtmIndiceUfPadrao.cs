using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class AtmIndiceUfPadrao : Notifiable, IEntity, INotifiable
    {
        private AtmIndiceUfPadrao()
        {
        }

        public static AtmIndiceUfPadrao Criar(int codTipoProcesso, int codIndice, string codEstado)
        {
            var indice = new AtmIndiceUfPadrao();

            indice.CodTipoProcesso = codTipoProcesso;
            indice.CodIndice = codIndice;
            indice.CodEstado = codEstado;
                      
            return indice;
        }

        public int Id { get; private set; }
        public int CodTipoProcesso { get; private set; }
        public int CodIndice { get; private set; }
        public string CodEstado { get; private set; }


    }
}