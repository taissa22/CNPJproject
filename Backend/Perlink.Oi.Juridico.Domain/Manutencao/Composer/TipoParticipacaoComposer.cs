using Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Composer
{
    public class TipoParticipacaoComposer : ITipoParticipacaoComposer
    {
        public TipoParticipacao Create(string descricao)
        {
            return new TipoParticipacao(descricao);
        }
    }
}
