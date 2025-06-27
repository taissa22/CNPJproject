using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Composer
{
    public class TipoAudienciaComposer : ITipoAudienciaComposer
    {
        public TipoAudiencia Create(string descricao, string sigla, TipoProcessoEnum? tipoProcesso)
        {
            return new TipoAudiencia(descricao, sigla, tipoProcesso);
        }
    }
}
