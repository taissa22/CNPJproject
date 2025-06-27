using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class TipoPendencia : Notifiable, IEntity, INotifiable
    {
        private TipoPendencia()
        {
        }

        /// <summary>
        /// Instancia uma Ação - <c>Tributária</c>
        /// </summary>
        /// <param name="descricao">Descrição da Ação</param>
        public static TipoPendencia Criar(string descricao)
        {
            var pendencia = new TipoPendencia()
            {
                Descricao = descricao.ToUpper()
            };
            pendencia.Validate();
            return pendencia;
        }       

        public int Id { get; private set; }

        public string Descricao { get; private set; }


        private void Validate()
        {            

            if (Descricao.Length > 50)
            {
                AddNotification(nameof(Descricao), "O Campo descrição pode conter no máximo 50 caracteres.");
            }

        }

        public void Atualizar(string descricao)
        {
            Descricao = descricao.ToUpper();           

            Validate();
        }
    }
}