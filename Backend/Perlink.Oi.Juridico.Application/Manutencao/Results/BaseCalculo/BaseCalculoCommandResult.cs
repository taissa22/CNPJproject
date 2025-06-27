using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos
{
    public class BaseCalculoCommandResult : ICommandResult
    {
        public BaseCalculoCommandResult(long codigoBaseCalculo, string descricao, bool ehBaseInicial)
        {
            CodigoBaseCalculo = codigoBaseCalculo;
            Descricao = descricao;
            EhBaseInicial = ehBaseInicial;
        }

        public long CodigoBaseCalculo { get; set; }

        public string Descricao { get; set; }

        public bool EhBaseInicial { get; set; }
    }
}
