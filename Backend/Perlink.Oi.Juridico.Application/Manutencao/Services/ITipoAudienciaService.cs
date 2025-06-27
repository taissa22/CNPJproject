using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ITipoAudienciaService
    {
        CommandResult Atualizar(AtualizarTipoAudienciaCommand command);

        CommandResult Criar(CriarTipoAudienciaCommand command);

        CommandResult Remover(int TipoAudienciaId);
    }
}
