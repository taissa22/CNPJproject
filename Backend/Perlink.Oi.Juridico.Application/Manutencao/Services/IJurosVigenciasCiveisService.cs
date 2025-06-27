using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IJurosVigenciasCiveisService
    {
        CommandResult Criar(CriarJurosVigenciasCiveisCommand command);

        CommandResult Atualizar(AtualizarJurosVigenciasCiveisCommand command);

        CommandResult Remover(int codigoTipoProcesso, DateTime dataVigencia);
    }
}
