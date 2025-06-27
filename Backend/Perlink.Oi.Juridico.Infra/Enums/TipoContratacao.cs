using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Enums
{
    public sealed class TipoContratacao
    {
        private TipoContratacao(int? id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int? Id { get; private set; }

        public string Descricao { get; private set; }

        #region enum

        public static TipoContratacao PorId(int? id) => Todos.Single(x => x.Id == id);

        public static TipoContratacao NAO_DEFINIDO = new TipoContratacao(null, "NÃO DEFINIDO");

        public static TipoContratacao FIXO_POR_CONTRATO = new TipoContratacao(1, "Fixo por contrato");

        public static TipoContratacao FIXO_POR_PROCESSO = new TipoContratacao(2, "Fixo por processo");

        public static TipoContratacao CONTRATO_POTUAL = new TipoContratacao(3, "Contrato pontual");

        // TODO: Criar semelhante no front e tornar este privado.
        public static IReadOnlyCollection<TipoContratacao> Todos { get; } = new[] { NAO_DEFINIDO, FIXO_POR_CONTRATO, FIXO_POR_PROCESSO, CONTRATO_POTUAL };

        #endregion enum

        #region converters

        public static implicit operator TipoContratacao(int? value) => PorId(value);

        public static implicit operator int?(TipoContratacao value) => value.Id;

        #endregion converters
    }
}