using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Esfera : Notifiable, IEntity, INotifiable
    {
        private Esfera()
        {
        }

        public static Esfera Criar(string nome, bool corrigePrincipal, bool corrigeMultas, bool corrigeJuros)
        {
            Esfera esfera = new Esfera();

            esfera.Nome = nome.ToUpper();
            esfera.CorrigePrincipal = corrigePrincipal;
            esfera.CorrigeMultas = corrigeMultas;
            esfera.CorrigeJuros = corrigeJuros;
            esfera.Validate();
            return esfera;
        }

        public void Atualizar(string nome, bool corrigePrincipal, bool corrigeMultas, bool corrigeJuros)
        {
            Nome = nome.ToUpper();
            CorrigePrincipal = corrigePrincipal;
            CorrigeMultas = corrigeMultas;
            CorrigeJuros = corrigeJuros;
            Validate();
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public bool CorrigePrincipal { get; private set; }
        public bool CorrigeMultas { get; private set; }
        public bool CorrigeJuros { get; private set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo requerido");
            }
        }
    }
}
