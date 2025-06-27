using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class AreaEnvolvida : Notifiable, IEntity, INotifiable
    {
        private AreaEnvolvida()
        {
        }
        public AreaEnvolvida(int id, string nome) {
            Id = id;
            Nome = nome;
        }


        public int Id { get; private set; }

        public string Nome { get; private set; }

        public bool Ativo { get; private set; }
      
        public bool EhCivelEstrategico { get; private set; }
    }
}