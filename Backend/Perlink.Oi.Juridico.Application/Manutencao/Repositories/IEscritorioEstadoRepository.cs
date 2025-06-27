using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEscritorioEstadoRepository
    {        
        CommandResult<IReadOnlyCollection<EscritorioEstadoDTO>> Obter(int escritorioId, int tipoProcessoid);
    }
}
