using FluentValidation.Results;

namespace Shared.Application.Interface
{
    public interface IResultadoApplication
    {
        bool Sucesso { get; }
        string Mensagem { get; }
        string UrlRedirect { get; }
        bool ExibeNotificacao { get; }

        IResultadoApplication ExecutadoComSuccesso();
        IResultadoApplication ExecutadoComErro(string message = null);
        IResultadoApplication ExibirMensagem(string message);
        IResultadoApplication RedirecionarPara(string url);
        IResultadoApplication Resultado(ValidationResult validate);
        object Retorno();
    }

    public interface IResultadoApplication<TData> : IResultadoApplication
    {
        TData Data { get; }

        IResultadoApplication<TData> DefinirData(TData data);
    }
}
