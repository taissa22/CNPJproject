using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarAdvogadoDoEscritorioCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Telefone { get; set; }
        public string TelefoneDDD { get; set; }
        public bool EhContato { get; set; }
        public string Email { get; set; }
        public string NumeroOAB { get; set; }
        public int EscritorioId { get; set; }

        public override void Validate()
        {
            if (Id <= 0)
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
