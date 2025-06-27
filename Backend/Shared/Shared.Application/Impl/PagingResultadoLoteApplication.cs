using FluentValidation.Results;
using Shared.Application.Interface;
using Shared.Tools;
using System;
using System.Linq;

namespace Shared.Application.Impl
{
    public class PagingResultadoLoteApplication<TData> : ResultadoApplication, IPagingResultadoLoteApplication<TData> {
        public TData Data { get; private set; }

        public int Total { get; set; }
        public long TotalLotes { get; set; }
        public double TotalValorLotes { get; set; }
        public long QuantidadesLancamentos { get; set; }

        public IResultadoApplication<TData> DefinirData(TData data) {
            Data = data;

            return this;
        }

        public override object Retorno()
        {
            return new
            {
                success = Sucesso,
                message = Mensagem,
                data = Data,
                total = Total,
                urlRedirect = UrlRedirect
            };
        }
    }
}
