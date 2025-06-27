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
    public interface IParteRepository
    {
        CommandResult<PaginatedQueryResult<Parte>> ObterPaginado(ParteSort sort, bool ascending, int pagina, int quantidade, TipoParte tipoParte, string nome, string documento, int? carteiraDeTrabalho);
        CommandResult<IReadOnlyCollection<Parte>> Obter(ParteSort sort, bool ascending, TipoParte tipoParte, string nome, string documento, int? carteiraDeTrabalho);
    }
}
