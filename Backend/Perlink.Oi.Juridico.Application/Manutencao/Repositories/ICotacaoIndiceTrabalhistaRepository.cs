using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface ICotacaoIndiceTrabalhistaRepository
    {
        CommandResult<IReadOnlyCollection<CotacaoIndiceTrabalhista>> Obter(DateTime dataCorrecao, DateTime dataBaseDe, DateTime dataBaseAte, CotacaoIndiceTrabalhistaSort sort, bool ascending);
        CommandResult<PaginatedQueryResult<CotacaoIndiceTrabalhista>> ObterPaginado(DateTime dataCorrecao, DateTime dataBaseDe, DateTime dataBaseAte, CotacaoIndiceTrabalhistaSort sort, bool ascending, int pagina, int quantidade);

        CommandResult<PaginatedQueryResult<TempCotacaoIndiceTrab>> ObterPaginadoTemp(CotacaoIndiceTrabalhistaSort sort, bool ascending, int pagina, int quantidade);

        CommandResult<PaginatedQueryResult<CotacaoIndiceTrabalhista>> CorrigirArquivo(List<CotacaoIndiceTrabalhista> lista);
        CommandResult<string> Validar(List<CotacaoIndiceTrabalhista> lista, DataTable tab);

        CommandResult<string> Validar(IFormFile arquivo, DateTime dataCorrecao, ref List<CotacaoIndiceTrabalhista> listaCotacao);


    }
}
