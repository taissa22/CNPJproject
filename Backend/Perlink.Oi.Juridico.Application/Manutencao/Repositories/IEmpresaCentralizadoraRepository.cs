using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IEmpresaCentralizadoraRepository
    {
        CommandResult<IReadOnlyCollection<EmpresaCentralizadora>> Obter(EmpresaCentralizadoraSort sort, bool ascending, DataString? nome, int? ordem, int? codigo);

        CommandResult<PaginatedQueryResult<EmpresaCentralizadora>> ObterPaginado(EmpresaCentralizadoraSort sort, bool ascending, int pagina, int quantidade, DataString? nome, int? ordem, int? codigo);

        CommandResult<bool> Existe(DataString nome, int? codigo);
        CommandResult<IReadOnlyCollection<EmpresaCentralizadora>> Obter();
    }
}