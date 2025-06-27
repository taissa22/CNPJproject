using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Commands
{ 
    public class CriarComarcaCommand : Validatable, IValidatable
    {
        public int Id { get; set; }
        public string Nome { get; set; } 
        public string EstadoId { get; set; } 
        public int? EscritorioCivelId { get; set; } 
        public int? EscritorioTrabalhistaId { get; set; }
        public int? ProfissionalCivelEstrategicoId { get; set; } 
        public int? ComarcaBBId { get; set; }

        public override void Validate()
        {
            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Nome não pode ser vazio");
            }

            if (String.IsNullOrEmpty(EstadoId))
            {
                AddNotification(nameof(EstadoId), "Estado não pode ser vazio");
            }

        }

    }
}
