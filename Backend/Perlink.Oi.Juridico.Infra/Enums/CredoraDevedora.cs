using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Id,nq} - {Descricao,nq}")]
    public readonly struct CredoraDevedora : IEquatable<CredoraDevedora>
    {
        private CredoraDevedora(string id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public string Id { get; }

        public string Descricao { get; }

        public bool Equals(CredoraDevedora other) => Id.Equals(other.Id) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is CredoraDevedora CredoraDevedora) && Equals(CredoraDevedora);

        public override int GetHashCode() => HashCode.Combine(Id, Descricao);

        public static bool operator ==(CredoraDevedora left, CredoraDevedora right) => left.Equals(right);

        public static bool operator !=(CredoraDevedora left, CredoraDevedora right) => !(left == right);

        #region enum

        public static CredoraDevedora NAO_DEFINIDO = new CredoraDevedora(null, "NÃO DEFINIDO");

        public static CredoraDevedora CREDORA = new CredoraDevedora("C", "CREDORA");

        public static CredoraDevedora DEVEDORA = new CredoraDevedora("D", "DEVEDORA");

        #endregion enum

        #region converters

        private static IReadOnlyCollection<CredoraDevedora> Todos { get; } = new[] { NAO_DEFINIDO, CREDORA, DEVEDORA };

        public static CredoraDevedora PorId(string id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new CredoraDevedora(id, id)).Single();

        #endregion converters
    }

    [DebuggerDisplay("{Id,nq} - {Descricao,nq}")]
    public readonly struct RiscoPerda : IEquatable<RiscoPerda>
    {
        private RiscoPerda(string id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public string Id { get; }

        public string Descricao { get; }

        public bool Equals(RiscoPerda other) => Id.Equals(other.Id) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is RiscoPerda RiscoPerda) && Equals(RiscoPerda);

        public override int GetHashCode() => HashCode.Combine(Id, Descricao);

        public static bool operator ==(RiscoPerda left, RiscoPerda right) => left.Equals(right);

        public static bool operator !=(RiscoPerda left, RiscoPerda right) => !(left == right);

        #region enum

        public static RiscoPerda NAO_DEFINIDO = new RiscoPerda(null, "NÃO DEFINIDO");

        public static RiscoPerda POSSIVEL = new RiscoPerda("PO", "POSSÍVEL");

        public static RiscoPerda PROVAVEL = new RiscoPerda("PR", "PROVÁVEL");

        public static RiscoPerda REMOTO = new RiscoPerda("RE", "REMOTO");

        #endregion enum

        #region converters

        private static IReadOnlyCollection<RiscoPerda> Todos { get; } = new[] { NAO_DEFINIDO, POSSIVEL, PROVAVEL, REMOTO };

        public static RiscoPerda PorId(string id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new RiscoPerda(id, id)).Single();

        #endregion converters
    }

   
}