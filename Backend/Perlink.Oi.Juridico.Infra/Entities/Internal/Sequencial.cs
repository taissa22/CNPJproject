using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Infra.Entities.Internal
{
    internal sealed class Sequencial : IEntity
    {
        private Sequencial()
        {
        }

        internal Sequencial(string tabela)
        {
            Tabela = tabela;
            Valor = 0;
        }

        public string Tabela { get; private set; }

        public int Valor { get; private set; }

        internal void IncrementaValor()
        {
            Valor++;
        }
    }
}