
namespace Perlink.Oi.Juridico.Domain.Compartilhado.Entities
{
    public class Sequencial
    {
        private Sequencial() { }

        public Sequencial(string tabela)
        {
            Id = tabela;
            ValorDaSequence = 0;
        }

        public Sequencial(string tabela, long valor)
        {
            Id = tabela;
            ValorDaSequence = valor;
        }

        public string Id { get; private set; }

        public long ValorDaSequence { get; private set; }

        public void IncrementaValor()
        {
            ValorDaSequence++;
        }
    }
}
