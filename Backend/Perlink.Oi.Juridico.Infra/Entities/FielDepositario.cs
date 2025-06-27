using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public class FielDepositario : Notifiable, IEntity, INotifiable
    {
        private FielDepositario()
        {
        }

        public static FielDepositario Criar(CPF cpf, string nome)
        {
            var fielDepositario = new FielDepositario()
            {
                Cpf = cpf,
                Nome = nome.Padronizar()
            };
            fielDepositario.Validate();
            return fielDepositario;
        }

        public void AtualizarFielDepositario(string nome, CPF cpf)
        {
            Cpf = cpf;
            Nome = nome.Padronizar();

            Validate();
        }

        public int Id { get; private set; }

        public string Cpf { get; private set; }

        public string Nome { get; private set; }

        private void Validate()
        {
            if (string.IsNullOrEmpty(Nome))
            {
                AddNotification(nameof(Nome), "O Campo descrição é obrigatório.");
            }
        }
    }
}