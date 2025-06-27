using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ITipoProcedimentoService
    {

        CommandResult Criar(CriarTipoProcedimentoCommand command);

        CommandResult Atualizar(AtualizarTipoProcedimentoCommand command);

        CommandResult Remover(int codigo);

    }
}
