using Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Composer
{
    public class BaseCalculoComposer : IBaseCalculoComposer
    {
        public BaseCalculo Create(string descricao)
        {
            return new BaseCalculo(descricao);
        }
    }
}
