using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Id,nq} - {Nome,nq}")]
    public readonly struct ClassificacaoClosing : IEquatable<ClassificacaoClosing>
    {
        private ClassificacaoClosing(int id, string nome, string enumName)
        {
            Id = id;
            Nome = nome;
            NomeEnum = enumName;
        }

        public int Id { get; }

        public string Nome { get; }

        public string NomeEnum { get; }

        #region OPERATORS

        public bool Equals(ClassificacaoClosing other) => Id.Equals(other.Id) && Nome.Equals(other.Nome);

        public override bool Equals(object obj) => (obj is ClassificacaoClosing classificacaoClosing) && Equals(classificacaoClosing);

        public override int GetHashCode() => HashCode.Combine(Id, Nome);

        public static bool operator ==(ClassificacaoClosing left, ClassificacaoClosing right) => left.Equals(right);

        public static bool operator !=(ClassificacaoClosing left, ClassificacaoClosing right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static ClassificacaoClosing NAO_DEFINIDO = new ClassificacaoClosing(-1, "Não Definido", "NAO_DEFINIDO");

        public static ClassificacaoClosing A_DEFINIR = new ClassificacaoClosing(0, "A Definir", "A_DEFINIR");
        public static ClassificacaoClosing PRE = new ClassificacaoClosing(1, "Pré", "PRE");
        public static ClassificacaoClosing POS = new ClassificacaoClosing(2, "Pós", "POS");
        public static ClassificacaoClosing HIBRIDO = new ClassificacaoClosing(3, "Híbrido", "HIBRIDO");
        

        #endregion ENUM

        private static IReadOnlyCollection<ClassificacaoClosing> Todos { get; } = new[] { NAO_DEFINIDO, A_DEFINIR, PRE, POS, HIBRIDO};

        public static ClassificacaoClosing PorId(int id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new ClassificacaoClosing(id, "Não Encontrado", "NAO_ENCONTRADO")).Single();

    }
}
