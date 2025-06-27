using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Valor,nq} - {Descricao,nq}")]
    public readonly struct StatusAgendamento : IEquatable<StatusAgendamento>
    {
        private StatusAgendamento(int id, string nome)
        {
            Id = id;
            Nome = nome;
        }

        public int Id { get; }

        public string Nome { get; }

        #region OPERATORS

        public bool Equals(StatusAgendamento other) => Id != null && Id.Equals(other.Id) && Nome.Equals(other.Nome);

        public override bool Equals(object obj) => (obj is StatusAgendamento StatusAgendamento) && Equals(StatusAgendamento);

        public override int GetHashCode() => HashCode.Combine(Id, Nome);

        public static bool operator ==(StatusAgendamento left, StatusAgendamento right) => left.Equals(right);

        public static bool operator !=(StatusAgendamento left, StatusAgendamento right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static StatusAgendamento NAO_DEFINIDO = new StatusAgendamento(-1, "Não Definido");
        public static StatusAgendamento AGENDADO = new StatusAgendamento(1, "Agendado");
        public static StatusAgendamento EM_EXECUCAO = new StatusAgendamento(2, "Em Execução");
        public static StatusAgendamento EXECUTADO = new StatusAgendamento(3, "Executado");
        public static StatusAgendamento CANCELADO = new StatusAgendamento(4, "Cancelado");
        public static StatusAgendamento ERRO = new StatusAgendamento(5, "Erro");
        
        #endregion ENUM

        private static IReadOnlyCollection<StatusAgendamento> Todos { get; } = new[] { NAO_DEFINIDO, AGENDADO, EM_EXECUCAO, EXECUTADO, CANCELADO, ERRO };

        public static StatusAgendamento PorId(int id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new StatusAgendamento(id, "Não encontrado")).Single();
    }
}
