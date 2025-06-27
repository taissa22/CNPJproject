using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Seedwork
{
    public class CommandResult<T> : CommandResult
    {
        public static CommandResult<T> Valid(T dados) => new CommandResult<T>(ResultType.Valid, dados);

        public static CommandResult<T> Valid(T dados, IEnumerable<string> mensagens) => new CommandResult<T>(ResultType.Valid, mensagens, dados);

        public static new CommandResult<T> Forbidden() => new CommandResult<T>(ResultType.Forbidden);

        public static new CommandResult<T> Invalid(string mensagem) => new CommandResult<T>(ResultType.Invalid, mensagem);

        public static new CommandResult<T> Invalid(IEnumerable<string> mensagens) => new CommandResult<T>(ResultType.Invalid, mensagens);

        protected CommandResult(ResultType tipo) : base(tipo) {
        }

        protected CommandResult(ResultType tipo, T dados) : base(tipo) {
            Dados = dados;
        }

        protected CommandResult(ResultType tipo, string mensagem) : base(tipo, mensagem) {
        }

        protected CommandResult(ResultType tipo, IEnumerable<string> mensagens) : base(tipo, mensagens) {
        }

        protected CommandResult(ResultType tipo, IEnumerable<string> mensagens, T dados) : base(tipo, mensagens) {
            Dados = dados;
        }

        public T Dados { get; }
    }
}
