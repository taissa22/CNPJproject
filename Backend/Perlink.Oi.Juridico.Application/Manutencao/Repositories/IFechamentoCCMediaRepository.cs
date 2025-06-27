using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IFechamentoCCMediaRepository
    {
        CommandResult<PaginatedQueryResult<FechamentoCCMedia>> ObterPaginado(DateTime? dataInicial, DateTime? dataFinal, int pagina);
        CommandResult<byte[]> Baixar(long id, ref string nomeArquivo);
    }
}
