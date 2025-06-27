using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    [DebuggerDisplay("{Valor,nq} - {Descricao,nq}")]
    public readonly struct ClassificacaoProcesso : IEquatable<ClassificacaoProcesso>
    {
        private ClassificacaoProcesso(string valor, string descricao)
        {
            Valor = valor;
            Descricao = descricao;
        }

        public string Valor { get; }

        public string Descricao { get; }

        #region OPERATORS

        public bool Equals(ClassificacaoProcesso other) => Valor.Equals(other.Valor) && Descricao.Equals(other.Descricao);

        public override bool Equals(object obj) => (obj is ClassificacaoProcesso ClassificacaoProcesso) && Equals(ClassificacaoProcesso);

        public override int GetHashCode() => HashCode.Combine(Valor, Descricao);

        public static bool operator ==(ClassificacaoProcesso left, ClassificacaoProcesso right) => left.Equals(right);

        public static bool operator !=(ClassificacaoProcesso left, ClassificacaoProcesso right) => !(left == right);

        #endregion OPERATORS


        #region ENUM

        public static ClassificacaoProcesso NAO_DEFINIDO = new ClassificacaoProcesso(null, "Não Definido");
        public static ClassificacaoProcesso UNICO = new ClassificacaoProcesso("U", "Único");
        public static ClassificacaoProcesso PRIMARIO = new ClassificacaoProcesso("P", "Primário");
        public static ClassificacaoProcesso SECUNDARIO = new ClassificacaoProcesso("S", "Secundário");

        #endregion ENUM

        private static IReadOnlyCollection<ClassificacaoProcesso> Todos { get; } = new[] { NAO_DEFINIDO, UNICO, PRIMARIO, SECUNDARIO };

        public static ClassificacaoProcesso PorValor(string valor) => Todos.Where(x => x.Valor == valor).DefaultIfEmpty(new ClassificacaoProcesso(valor, valor)).Single();
    }
}