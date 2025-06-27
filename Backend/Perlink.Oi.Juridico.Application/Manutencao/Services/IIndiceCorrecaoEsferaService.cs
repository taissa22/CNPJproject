using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IIndiceCorrecaoEsferaService
    {
        CommandResult<PaginatedQueryResult<ProcessoInconsistente>> Criar(CriarIndiceCorrecaoEsferaCommand command);

        CommandResult<PaginatedQueryResult<ProcessoInconsistente>> Remover(int codigoEsfera, DateTime dataVigencia, int codigoIndice);
    }
}