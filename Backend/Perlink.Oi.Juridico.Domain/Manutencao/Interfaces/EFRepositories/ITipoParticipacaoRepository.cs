using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories
{
    public interface ITipoParticipacaoRepository
    {
        TipoParticipacao Get(long id);

        void Save(TipoParticipacao tipoParticipacao);

        void Update(TipoParticipacao tipoParticipacao);

        void Remove(TipoParticipacao tipoParticipacao);
    }
}
