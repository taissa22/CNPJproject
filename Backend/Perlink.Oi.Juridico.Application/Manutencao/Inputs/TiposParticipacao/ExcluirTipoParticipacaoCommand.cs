using Shared.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposParticipacao
{
    public class ExcluirTipoParticipacaoCommand : ICommand
    {
        public ExcluirTipoParticipacaoCommand() { }

        public ExcluirTipoParticipacaoCommand(long id)
        {
            CodigoTipoParticipacao = id;
        }

        public long CodigoTipoParticipacao { get; set; }
    }
}
