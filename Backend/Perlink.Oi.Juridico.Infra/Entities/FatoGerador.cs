using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class FatoGerador : Notifiable, IEntity, INotifiable
    {
        private FatoGerador()
        {
        }

        public FatoGerador(int id, string nome) {
            Id = id;
            Nome = nome;
        }

        public static FatoGerador Criar(string Nome,bool Ativo)
        {
            FatoGerador fato = new FatoGerador();
            fato.Nome = Nome;
            fato.Ativo = Ativo;
            fato.Validate();
            return fato;
        }
        public void Atualizar(int Id,string Nome, bool Ativo)
        {
            this.Id = Id;
            this.Nome = Nome;
            this.Ativo = Ativo;
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Nome não pode estar vazia");
            }
        }

        public int Id { get; private set; }

        public string Nome { get; private set; }

        public bool Ativo { get; private set; }

        internal int? TipoProcessoId { get; private set; }

        public TipoProcessoManutencao? TipoProcesso => TipoProcessoManutencao.PorId(TipoProcessoId.GetValueOrDefault());
    }
}