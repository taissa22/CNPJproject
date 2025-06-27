using Flunt.Notifications;
using Flunt.Validations;
using Shared.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposParticipacao
{
    public class AtualizarTipoParticipacaoCommand : Notifiable, ICommand
    {
        public AtualizarTipoParticipacaoCommand() { }

        public AtualizarTipoParticipacaoCommand(long? codigoTipoParticipacao, string descricao)
        {
            CodigoTipoParticipacao = codigoTipoParticipacao;
            Descricao = descricao;
        }

        public long? CodigoTipoParticipacao { get; set; }

        public string Descricao { get; set; }

        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .IsNotNull(CodigoTipoParticipacao, "AtualizarTipoParticipacaoCommand.CodigoTipoParticipacao", "Código Tipo Participacão é obrigatório.")
            );
        }
    }
}
