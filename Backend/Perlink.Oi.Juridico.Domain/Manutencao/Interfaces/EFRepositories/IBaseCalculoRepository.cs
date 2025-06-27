using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Interfaces.EFRepositories
{
    public interface IBaseCalculoRepository
    {
        BaseCalculo Get(long id);

        void Save(BaseCalculo baseCalculo);

        void Update(BaseCalculo baseCalculo);

        void Remove(BaseCalculo baseCalculo);

        BaseCalculo ObterPorBaseInicial();
    }
}
