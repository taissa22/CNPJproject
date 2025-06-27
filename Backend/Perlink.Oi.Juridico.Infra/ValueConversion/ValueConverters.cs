using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Perlink.Oi.Juridico.Infra.Enums;

namespace Perlink.Oi.Juridico.Infra.ValueConversion
{
    internal static class ValueConverters
    {
        public static readonly ValueConverter<bool, string> BoolToString =
            new ValueConverter<bool, string>(v => v ? "S" : "N", v => string.IsNullOrEmpty(v) || v.Equals("N") ? false : true);

        public static readonly ValueConverter<TipoParte, string> TipoParteToString =
            new ValueConverter<TipoParte, string>(v => v.Valor, v => TipoParte.PorValor(v));

        public static readonly ValueConverter<TipoPessoa, string> TipoPessoaToString =
            new ValueConverter<TipoPessoa, string>(v => v.Valor, v => TipoPessoa.PorValor(v));

        public static readonly ValueConverter<EstadoEnum, string> EstadoToString =
            new ValueConverter<EstadoEnum, string>(v => v.Id, v => EstadoEnum.PorId(v));

        public static readonly ValueConverter<ClassificacaoProcesso, string> ClassificacaoProcessoToString =
            new ValueConverter<ClassificacaoProcesso, string>(v => v.Valor, v => ClassificacaoProcesso.PorValor(v));

        public static readonly ValueConverter<TipoProcesso, int?> TipoProcessoToNullableInt =
            new ValueConverter<TipoProcesso, int?>(v => v.Id == -1 ? default(int?) : v.Id, v => TipoProcesso.PorId(v ?? -1));

        public static readonly ValueConverter<TipoProcesso, int> TipoProcessoToInt =
            new ValueConverter<TipoProcesso, int>(v => v.Id, v => TipoProcesso.PorId(v));

        public static readonly ValueConverter<CredoraDevedora, string> CredoraDevedoraToString =
            new ValueConverter<CredoraDevedora, string>(v => v.Id, v => CredoraDevedora.PorId(v));

        public static readonly ValueConverter<string, string> UpperCaseString =
            new ValueConverter<string, string>(v => v.ToUpper(), v => v.ToUpper());

        public static readonly ValueConverter<RiscoPerda, string> RiscoPerdaToString =
             new ValueConverter<RiscoPerda, string>(v => v.Id, v => RiscoPerda.PorId(v));

        //public static readonly ValueConverter<StatusAgendamentoCagarPagamento, string> StatusAgendamentoCagarPagamento =
        //     new ValueConverter<StatusAgendamentoCagarPagamento, string>(v => v.Id, v => StatusAgendamentoCagarPagamento.PorId(v));

    }
}