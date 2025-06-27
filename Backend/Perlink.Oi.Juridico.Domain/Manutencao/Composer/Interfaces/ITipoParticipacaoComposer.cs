using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces
{
    public interface ITipoParticipacaoComposer
    {
        TipoParticipacao Create(string descricao);
    }
}
