using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IProfissionalRepository
    {
        CommandResult<IReadOnlyCollection<Profissional>> Obter(ProfissionalSort sort, bool ascending, TipoPessoa tipoPessoa, string nome, string documento, bool? advogadoAutor = null);

        CommandResult<PaginatedQueryResult<Profissional>> ObterPaginado(ProfissionalSort sort, bool ascending, int pagina, int quantidade, TipoPessoa tipoPessoa, string nome, string documento, bool? advogadoAutor = null);

        CommandResult<bool> Existe(string nome, int? id);
    }
}
