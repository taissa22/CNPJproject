using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Seedwork
{
    public class CommandResult
    {
        private CommandResult()
        {
                
        }
        public static CommandResult Valid() => new CommandResult(ResultType.Valid);

        public static CommandResult Valid(IEnumerable<string> mensagens) => new CommandResult(ResultType.Valid, mensagens);

        public static CommandResult Forbidden() => new CommandResult(ResultType.Forbidden);

        public static CommandResult Invalid(IEnumerable<string> mensagens) => new CommandResult(ResultType.Invalid, mensagens);

        public static CommandResult Invalid(string mensagem) => new CommandResult(ResultType.Invalid, mensagem);

        protected CommandResult(ResultType tipo)
        {
            Tipo = tipo;
        }

        protected CommandResult(ResultType tipo, IEnumerable<string> mensagens) : this(tipo)
        {
            Mensagens = mensagens;
        }

        protected CommandResult(ResultType tipo, string mensagem) : this(tipo)
        {
            Mensagens = new[] { mensagem };
        }

        public ResultType Tipo { get; }

        public IEnumerable<string> Mensagens { get; }

        public bool IsValid => Tipo == ResultType.Valid;
    }
}
