using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;
#nullable enable
namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ComarcaBB : Notifiable, IEntity, INotifiable
    {
        private ComarcaBB() {
            
        }
        public static ComarcaBB Criar(string codEstadoBB, int codigo, string nome)
        {
            ComarcaBB bbcomarca = new ComarcaBB();
            bbcomarca.EstadoId = codEstadoBB;
            bbcomarca.Codigo = codigo;
            bbcomarca.Nome = nome;

            bbcomarca.Validate();
            return bbcomarca;
        }
        public void Atualizar(string codEstadoBB, int codigo, string nome)
        {
           EstadoId = codEstadoBB;
           Codigo = codigo;
           Nome = nome;

           Validate();
        }
        public int Id { get; private set; }
        public string EstadoId { get; private set; } = null!;
        public int Codigo { get; private set; }
        public string Nome { get; private set; } = null!;

        public void Validate()
        {
            if (Id <= 0)
            {
                AddNotification(nameof(Id), "Código não pode ser menor do que zero.");
            }
            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Nome não pode ser vazio");
            }
            if (String.IsNullOrEmpty(EstadoId))
            {
                AddNotification(nameof(EstadoId), "Código estado não pode ser vazio");
            }
            if (Codigo <= 0)
            {
                AddNotification(nameof(Codigo), "Código não pode ser menor do que zero.");
            }
        }

    }
}
