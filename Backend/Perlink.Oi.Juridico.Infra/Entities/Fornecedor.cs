using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities {

    public sealed class Fornecedor : Notifiable, IEntity, INotifiable {

        private Fornecedor() {
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }

        public string CNPJ { get; private set; }

        public int TipoFornecedorId { get; private set; }

        public int? EscritorioId { get; private set; }
        public Escritorio Escritorio { get; private set; }

        public int? ProfissionalId { get; private set; }
        public Escritorio Profissional { get; private set; }
    }
}