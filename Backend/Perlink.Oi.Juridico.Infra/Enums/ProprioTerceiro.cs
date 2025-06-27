using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{

    [DebuggerDisplay("{Valor,nq}")]
    public sealed class ProprioTerceiro {
        private ProprioTerceiro(string id, string valor) {
            Id = id;
            Valor = valor;
        }

        public string Id { get; private set; }

        public string Valor { get; private set; }

        #region enum

        public static ProprioTerceiro PorId(string id) => Todos.Single(x => x.Id == id);

        public static ProprioTerceiro NAO_DEFINIDO = new ProprioTerceiro(null, "NÃO DEFINIDO");

        public static ProprioTerceiro PROPRIO = new ProprioTerceiro("P", "PRÓPRIO");

        public static ProprioTerceiro TERCEIRO = new ProprioTerceiro("T", "TERCEIRO");

        private static IReadOnlyCollection<ProprioTerceiro> Todos { get; } = new[] { NAO_DEFINIDO, PROPRIO, TERCEIRO };

        #endregion enum

        #region converters

        public static implicit operator ProprioTerceiro(string value) => PorId(value);

        public static implicit operator string(ProprioTerceiro value) => value.Id;

        #endregion converters
    }
}
