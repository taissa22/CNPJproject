using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class Municipio : Notifiable, IEntity, INotifiable
    {
        public static Municipio Criar(string EstadoId, string Nome)
        {
            var municipio = new Municipio()
            {
                Nome = Nome,
                EstadoId = EstadoId
            };
            municipio.Validate();
            return municipio;
        }
        public void Atualizar(string EstadoId ,string Nome)
        {
            this.EstadoId = EstadoId;
            this.Nome = Nome;
            Validate();
        }

        public int Id { get; private set; }
        public string EstadoId { get; private set; }
        public Estado Estado { get; private set; }
        public string Nome { get; private set; }

        public  void Validate()
        {
            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }
        }
    }
}
