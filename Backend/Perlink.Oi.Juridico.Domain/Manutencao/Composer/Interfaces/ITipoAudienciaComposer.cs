using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Manutencao.Entities;

namespace Perlink.Oi.Juridico.Domain.Manutencao.Composer.Interfaces
{
    public interface ITipoAudienciaComposer
    {
        TipoAudiencia Create(string descricao, string sigla, TipoProcessoEnum? tipoProcesso);
    }
}
