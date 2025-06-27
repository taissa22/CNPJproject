using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums {
    [DebuggerDisplay("{Id,nq} - {Nome,nq}")]
    public readonly struct TipoDocumentoLancamento : IEquatable<TipoDocumentoLancamento> {
        private TipoDocumentoLancamento(int id, string nome) {
            Id = id;
            Nome = nome;
        }

        public int Id { get; }

        public string Nome { get; }

        #region OPERATORS

        public bool Equals(TipoDocumentoLancamento other) => Id.Equals(other.Id) && Nome.Equals(other.Nome);

        public override bool Equals(object obj) => (obj is TipoDocumentoLancamento tipoDocumentoLancamento) && Equals(tipoDocumentoLancamento);

        public override int GetHashCode() => HashCode.Combine(Id, Nome);

        public static bool operator ==(TipoDocumentoLancamento left, TipoDocumentoLancamento right) => left.Equals(right);

        public static bool operator !=(TipoDocumentoLancamento left, TipoDocumentoLancamento right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoDocumentoLancamento NAO_DEFINIDO = new TipoDocumentoLancamento(-1, "Não Definido");
        public static TipoDocumentoLancamento GUIA_JUDICIAL = new TipoDocumentoLancamento(1, "Guia Judicial");
        public static TipoDocumentoLancamento COMPROVANTE_DE_PAGAMENTO = new TipoDocumentoLancamento(2, "Comprovante de Pagamento");

        #endregion ENUM

        private static IReadOnlyCollection<TipoDocumentoLancamento> Todos { get; } = new[] { NAO_DEFINIDO, GUIA_JUDICIAL, COMPROVANTE_DE_PAGAMENTO };

        public static TipoDocumentoLancamento PorId(int id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new TipoDocumentoLancamento(id, "Não Encontrado")).Single();
    }
}
