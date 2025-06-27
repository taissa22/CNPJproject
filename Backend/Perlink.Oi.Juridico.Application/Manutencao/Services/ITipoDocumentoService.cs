using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ITipoDocumentoService
    {
        CommandResult Criar(CriarTipoDocumentoCommand command);

        CommandResult Atualizar(AtualizarTipoDocumentoCommand command);

        CommandResult Remover(int id);   

    }
}
