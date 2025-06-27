using FluentValidation.Results;
using Shared.Application.Interface;
using Shared.Tools;
using System;
using System.Linq;

namespace Shared.Application.Impl
{
    public class ResultadoApplication : IResultadoApplication
    {
        public bool Sucesso { get; private set; }
        public string Mensagem { get; private set; }
        public string UrlRedirect { get; private set; }

        public bool ExibeNotificacao { get; set;}

        public IResultadoApplication ExecutadoComSuccesso()
        {
            Sucesso = true;

            return this;
        }

        public IResultadoApplication ExecutadoComErro(Exception excecao)
        {
            Sucesso = false;

            Mensagem = excecao.TratarExcecao();

            return this;
        }

        public IResultadoApplication ExecutadoComErro(string message = null)
        {
            Sucesso = false;

            if (!string.IsNullOrEmpty(message))
                Mensagem = message;

            return this;
        }

        public IResultadoApplication ExibirMensagem(string message)
        {
            Mensagem = message;

            return this;
        }

        public IResultadoApplication Resultado(ValidationResult validate)
        {
            Sucesso = validate.IsValid;
            Mensagem = string.Join("\n", validate.Errors.Select(x => x.ErrorMessage));

            return this;
        }

        public IResultadoApplication RedirecionarPara(string url)
        {
            UrlRedirect = url;

            return this;
        }

        public virtual object Retorno()
        {
            return new
            {
                success = Sucesso,
                message = Mensagem,
                urlRedirect = UrlRedirect,
                exibeNotificacao = ExibeNotificacao
            };
        }

    }

    public class ResultadoApplication<TData> : ResultadoApplication, IResultadoApplication<TData>
    {
        public TData Data { get; private set; }

        public IResultadoApplication<TData> DefinirData(TData data)
        {
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
                urlRedirect = UrlRedirect
            };
        }
    }
}
