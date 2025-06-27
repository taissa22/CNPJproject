using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums {
    [DebuggerDisplay("{Id,nq} - {Nome,nq}")]
    public readonly struct TipoProcesso : IEquatable<TipoProcesso> {
        private TipoProcesso(int id, string nome, string enumName) {
            Id = id;
            Nome = nome;
            NomeEnum = enumName;
        }

        public int Id { get; }

        public string Nome { get; }

        public string NomeEnum { get; }

        #region OPERATORS

        public bool Equals(TipoProcesso other) => Id.Equals(other.Id) && Nome.Equals(other.Nome);

        public override bool Equals(object obj) => (obj is TipoProcesso tipoProcesso) && Equals(tipoProcesso);

        public override int GetHashCode() => HashCode.Combine(Id, Nome);

        public static bool operator ==(TipoProcesso left, TipoProcesso right) => left.Equals(right);

        public static bool operator !=(TipoProcesso left, TipoProcesso right) => !(left == right);

        #endregion OPERATORS

        #region ENUM

        public static TipoProcesso NAO_DEFINIDO = new TipoProcesso(-1, "Não Definido", "NAO_DEFINIDO");
        
        public static TipoProcesso CIVEL_CONSUMIDOR = new TipoProcesso(1, "Cível Consumidor", "CIVEL_CONSUMIDOR");
        public static TipoProcesso TRABALHISTA = new TipoProcesso(2, "Trabalhista", "TRABALHISTA");
        public static TipoProcesso ADMINISTRATIVO = new TipoProcesso(3, "Administrativo", "ADMINISTRATIVO");
        public static TipoProcesso TRIBUTARIO_ADMINISTRATIVO = new TipoProcesso(4, "Tributário Administrativo", "TRIBUTARIO_ADMINISTRATIVO");
        public static TipoProcesso TRIBUTARIO_JUDICIAL = new TipoProcesso(5, "Tributário Judicial", "TRIBUTARIO_JUDICIAL");
        public static TipoProcesso TRABALHISTA_ADMINISTRATIVO = new TipoProcesso(6, "Trabalhista Administrativo", "TRABALHISTA_ADMINISTRATIVO");
        public static TipoProcesso JEC = new TipoProcesso(7, "Juizado Especial Cível", "JEC");
        public static TipoProcesso EXPRESSINHO = new TipoProcesso(8, "Expressinho", "EXPRESSINHO");
        public static TipoProcesso CIVEL_ESTRATEGICO = new TipoProcesso(9, "Cível Estratégico", "CIVEL_ESTRATEGICO");
        public static TipoProcesso DESCUMPRIMENTO = new TipoProcesso(10, "Descumprimento", "DESCUMPRIMENTO"); 
        public static TipoProcesso DENUNCIA_FISCAL = new TipoProcesso(11, "Denúncia Fiscal", "DENUNCIA_FISCAL"); 
        public static TipoProcesso CIVEL_ADMINISTRATIVO = new TipoProcesso(12, "Cível Administrativo", "CIVEL_ADMINISTRATIVO"); 
        public static TipoProcesso CRIMINAL = new TipoProcesso(13, "Criminal", "CRIMINAL"); 
        public static TipoProcesso CRIMINAL_ADMINISTRATIVO = new TipoProcesso(14, "Criminal Administrativo", "CRIMINAL_ADMINISTRATIVO"); 
        public static TipoProcesso CRIMINAL_JUDICIAL = new TipoProcesso(15, "Criminal Judicial", "CRIMINAL_JUDICIAL"); 
        public static TipoProcesso PROCON = new TipoProcesso(17, "Procon", "PROCON"); 
        public static TipoProcesso PEX = new TipoProcesso(18, "Pex", "PEX");

        #endregion ENUM

        private static IReadOnlyCollection<TipoProcesso> Todos { get; } = new[] { NAO_DEFINIDO, CIVEL_CONSUMIDOR, TRABALHISTA, ADMINISTRATIVO, TRIBUTARIO_ADMINISTRATIVO, TRIBUTARIO_JUDICIAL, TRABALHISTA_ADMINISTRATIVO, JEC, EXPRESSINHO, CIVEL_ESTRATEGICO, DESCUMPRIMENTO, DENUNCIA_FISCAL, CIVEL_ADMINISTRATIVO, CRIMINAL, CRIMINAL_ADMINISTRATIVO, CRIMINAL_JUDICIAL, PROCON, PEX };

        public static TipoProcesso PorId(int id) => Todos.Where(x => x.Id == id).DefaultIfEmpty(new TipoProcesso(id, "Não Encontrado", "NAO_ENCONTRADO")).Single();
    }
}
