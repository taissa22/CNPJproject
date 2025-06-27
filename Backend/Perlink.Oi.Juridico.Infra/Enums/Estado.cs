using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Enums {
    [DebuggerDisplay("{Id,nq} - {Nome,nq}")]
    public readonly struct EstadoEnum : IEquatable<EstadoEnum>, IComparable<EstadoEnum>
    {
        private EstadoEnum(string id, string nome) {
            Id = id;
            Nome = nome;
        }

        public string Id { get; }

        public string Nome { get; }

        #region OPERATORS

        public bool Equals(EstadoEnum other) => Id.Equals(other.Id) && Nome.Equals(other.Nome);

        public override bool Equals(object obj) => (obj is EstadoEnum Estado) && Equals(Estado);

        public override int GetHashCode() => HashCode.Combine(Id, Nome);

        public static bool operator ==(EstadoEnum left, EstadoEnum right) => left.Equals(right);

        public static bool operator !=(EstadoEnum left, EstadoEnum right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static EstadoEnum NAO_DEFINIDO = new EstadoEnum(null, "Não Definido");
        public static EstadoEnum AC = new EstadoEnum("AC", "Acre");
        public static EstadoEnum AL = new EstadoEnum("AL", "Alagoas");
        public static EstadoEnum AM = new EstadoEnum("AM", "Amazonas");
        public static EstadoEnum AP = new EstadoEnum("AP", "Amapá");
        public static EstadoEnum BA = new EstadoEnum("BA", "Bahia");
        public static EstadoEnum CE = new EstadoEnum("CE", "Ceará");
        public static EstadoEnum DF = new EstadoEnum("DF", "Distrito Federal");
        public static EstadoEnum ES = new EstadoEnum("ES", "Espírito Santo");
        public static EstadoEnum GO = new EstadoEnum("GO", "Goiás");
        public static EstadoEnum MA = new EstadoEnum("MA", "Maranhão");
        public static EstadoEnum MG = new EstadoEnum("MG", "Minas Gerais");
        public static EstadoEnum MS = new EstadoEnum("MS", "Mato Grosso do Sul");
        public static EstadoEnum MT = new EstadoEnum("MT", "Mato Grosso");
        public static EstadoEnum PA = new EstadoEnum("PA", "Pará");
        public static EstadoEnum PB = new EstadoEnum("PB", "Paraíba");
        public static EstadoEnum PE = new EstadoEnum("PE", "Pernambuco");
        public static EstadoEnum PI = new EstadoEnum("PI", "Piauí");
        public static EstadoEnum PR = new EstadoEnum("PR", "Paraná");
        public static EstadoEnum RJ = new EstadoEnum("RJ", "Rio de Janeiro");
        public static EstadoEnum RN = new EstadoEnum("RN", "Rio Grande do Norte");
        public static EstadoEnum RO = new EstadoEnum("RO", "Rondônia");
        public static EstadoEnum RR = new EstadoEnum("RR", "Roraima");
        public static EstadoEnum RS = new EstadoEnum("RS", "Rio Grande do Sul");
        public static EstadoEnum SC = new EstadoEnum("SC", "Santa Catarina");
        public static EstadoEnum SE = new EstadoEnum("SE", "Sergipe");
        public static EstadoEnum SP = new EstadoEnum("SP", "São Paulo");
        public static EstadoEnum TO = new EstadoEnum("TO", "Tocantins");

        #endregion ENUM

        private static IReadOnlyCollection<EstadoEnum> Todos { get; } = new[] { NAO_DEFINIDO, AC, AL, AM, AP, BA, CE, DF, ES, GO, MA, MG, MS, MT, PA, PB, PE, PI, PR, RJ, RN, RO, RR, RS, SC, SE, SP, TO };

        public static EstadoEnum PorId(string id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new EstadoEnum(id, id)).Single();

        public static bool IsValid(string id) => Todos.Any(x => x.Id == id);

        public int CompareTo(EstadoEnum estado)
        {
            //Ver como é essa implementação
            return estado.Id == Id ? 1 : 0;
        }             
    }
}
