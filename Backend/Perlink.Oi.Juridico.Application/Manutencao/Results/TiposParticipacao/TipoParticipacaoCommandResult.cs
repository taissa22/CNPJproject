using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposParticipacao
{
    public class TipoParticipacaoCommandResult : ICommandResult
    {
        public TipoParticipacaoCommandResult(long id, string descricao)
        {
            CodTipoParticipacao = id;
            Descricao = descricao;
        }

        public long CodTipoParticipacao { get; set; }

        public string Descricao { get; set; }
    }
}
