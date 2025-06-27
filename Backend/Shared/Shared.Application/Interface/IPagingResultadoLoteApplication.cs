using FluentValidation.Results;

namespace Shared.Application.Interface
{
    public interface IPagingResultadoLoteApplication<TData>: IResultadoApplication<TData> {
        int Total { get; set; }
        long TotalLotes { get; set; }
        double TotalValorLotes { get; set; }
        long QuantidadesLancamentos { get; set; }
    }
}
