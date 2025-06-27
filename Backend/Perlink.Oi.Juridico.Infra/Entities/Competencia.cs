using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;

#nullable enable

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class Competencia : Notifiable, IEntity, INotifiable
    {
#pragma warning disable CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        private Competencia()
        {
        }

#pragma warning restore CS8618 // O campo não anulável não foi inicializado. Considere declará-lo como anulável.

        public static Competencia Criar(DataString nome)
        {
            var competencia = new Competencia()
            {
                Nome = nome.ToString()
            };
            competencia.Validate();
            return competencia;
        }

        internal int OrgaoId { get; private set; }
        public Orgao Orgao { get; private set; }

        public int Sequencial { get; private set; }
        public string Nome { get; private set; }

        public void Validate()
        {
            ClearNotifications();
            if (Orgao is null)
            {
                AddNotification(nameof(Orgao), "A competência deve ser adicionado a um Órgão.");
            }

            if (Sequencial == 0)
            {
                AddNotification(nameof(Sequencial), "A competência deve ser adicionado a um Órgão.");
            }

            if (Nome.Length > 40)
            {
                AddNotification(nameof(Nome), "O Nome de uma competência permite no máximo 40 caracteres.");
            }
        }

        internal void AdicionarOrgao(Orgao orgao, int sequencial)
        {
            if (Orgao != null)
            {
                throw new InvalidOperationException("Não se pode mudar o Órgão de uma competência");
            }

            Orgao = orgao;
            OrgaoId = orgao.Id;
            Sequencial = sequencial;
            Validate();
        }

        public void Atualizar(DataString nome)
        {
            Nome = nome.ToString();
            Validate();
        }
    }
}