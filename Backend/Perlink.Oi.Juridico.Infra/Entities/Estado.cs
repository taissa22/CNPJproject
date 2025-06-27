using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class Estado : Notifiable, IEntity, INotifiable
    {
        public void Atualizar(string Nome, decimal ValorJuros)
        {
            this.Nome = Nome;
            this.ValorJuros = ValorJuros;
            Validate();
        }

        public string Id { get; private set; }
        public string Nome { get; private set; }
        public int IndiceId { get; private set; }
        public decimal ValorJuros { get; private set; }
        public IEnumerable<Municipio> Municipios { get; private set; }

        private void Validate()
        {
            if (String.IsNullOrEmpty(Id))
            {
                AddNotification(nameof(Id), "Campo Requerido");
            }

            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }

        }
    }
}
