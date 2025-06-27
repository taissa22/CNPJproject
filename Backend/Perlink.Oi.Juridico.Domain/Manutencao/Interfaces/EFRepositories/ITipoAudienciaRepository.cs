using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories
{
    public interface ITipoAudienciaRepository
    {
        TipoAudiencia Get(long id);

        void Save(TipoAudiencia tipoAudiencia);

        void Update(TipoAudiencia tipoAudiencia);

        void Remove(TipoAudiencia tipoAudiencia);
    }
}
