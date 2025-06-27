using Perlink.Oi.Juridico.Infra.Entities.Internal;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ResponsavelInterno : Notifiable, IEntity, INotifiable
    {
        private ResponsavelInterno()
        {
        }
        public ResponsavelInterno(string id, string nome, string situacao) {
            Id = id;
            Nome = nome;
            Situacao = situacao;
        }


        public string Id { get; private set; }

        public string Nome { get; private set; }

        public bool Ativo { get; private set; }
        public string Situacao { get; private set; }
        internal UsuarioBase UsuarioBase { get; private set; }

        public bool EhCivelEstrategico { get; private set; }
    }
}