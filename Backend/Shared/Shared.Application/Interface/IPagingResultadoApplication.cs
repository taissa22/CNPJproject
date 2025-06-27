using FluentValidation.Results;

namespace Shared.Application.Interface
{
    public interface IPagingResultadoApplication<TData>: IResultadoApplication<TData> {
        int Total { get; set; }
    }
}
