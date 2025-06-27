using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    public interface IIndicesVigenciasService
    {
        CommandResult Criar(CriarIndiceVigenciaCommand command);
        CommandResult Remover(int id, DateTime dataVigencia);
    }
}
