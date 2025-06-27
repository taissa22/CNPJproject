using Perlink.Oi.Juridico.Application.Manutencao.Commands.Dto;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;


namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{
    public class AtualizarEmpresaCentralizadoraCommand : Validatable, IValidatable
    {
        public int Codigo { get; set; }

        public string Nome { get; set; } = string.Empty;

        public IEnumerable<ConvenioDTO> Convenios { get; set; } = Array.Empty<ConvenioDTO>();

        public override void Validate()
        {
            if (Codigo == 0)
            {
                AddNotification(nameof(Codigo), "O Código é obrigatório");
            }

            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Nome é obrigatório");
            }
            if (!string.IsNullOrEmpty(Nome) && Nome.Length > 400)
            {
                AddNotification(nameof(Nome), "O Nome permite no máximo 400 caracteres");
            }

            foreach (var convenio in Convenios)
            {
                convenio.Validate();
                AddNotifications(convenio);
            }
        }
    }
}