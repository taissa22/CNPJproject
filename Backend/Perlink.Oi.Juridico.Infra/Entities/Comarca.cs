using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
#nullable enable
namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Comarca : Notifiable, IEntity, INotifiable
    {
        private Comarca()
        {
        }

        public static Comarca Criar(string nome, Estado estado, int? escritorioCivelId, int? escritorioTrabalhistaId,
            int? profissionalCivelEstrategicoId, int? comarcaBBId)
        {
            Comarca comarca = new Comarca();
            comarca.Nome = nome;
            comarca.Estado = estado;
            comarca.EscritorioCivelId = escritorioCivelId;
            comarca.EscritorioTrabalhistaId = escritorioTrabalhistaId;
            comarca.ProfissionalCivelEstrategicoId = profissionalCivelEstrategicoId;
            comarca.ComarcaBBId = comarcaBBId;

            comarca.Validate();
            return comarca;
        }

        public void Atualizar(string nome, Estado estado, int? escritorioCivelId, int? escritorioTrabalhistaId, 
            int? profissionalCivelEstrategicoId, int? comarcaBBId)
        {
            Nome = nome;
            Estado = estado;
            EscritorioCivelId = escritorioCivelId;
            EscritorioTrabalhistaId = escritorioTrabalhistaId;
            ProfissionalCivelEstrategicoId = profissionalCivelEstrategicoId;
            ComarcaBBId = comarcaBBId;

            Validate();
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public Estado Estado { get; private set; }
        public int? EscritorioCivelId { get; private set; } = null!;
        public int? EscritorioTrabalhistaId { get; private set; } = null!;
        public int? ProfissionalCivelEstrategicoId { get; private set; } = null!;
        public int? ComarcaBBId { get; private set; } = null!;
        public ComarcaBB? ComarcaBB { get; private set; } = null!;
        //public IEnumerable<Vara> Varas { get; private set; }
        public string EstadoId { get; private set; }
        //public EstadoEnum EstadoEnum { get; private set; }

        public void Validate()
        {
           
            if (String.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "Nome não pode ser vazio");
            }

            if (Estado == null)
            {
                AddNotification(nameof(Estado), "Estado não pode ser vazio");
            }
        }
    }
}