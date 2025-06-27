using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
#nullable enable

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class CriarAdvogadoDoEscritorioCommand : Validatable, IValidatable
    {
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
            if (String.IsNullOrEmpty(Estado))
            {
                AddNotification(nameof(Estado), "Campo Requerido");
            }

            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Campo Requerido");
            }

        }
    }
}