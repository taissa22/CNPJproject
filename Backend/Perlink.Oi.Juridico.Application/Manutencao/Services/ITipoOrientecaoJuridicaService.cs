using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ITipoOrientecaoJuridicaService
    {
        CommandResult Criar(CriarTipoOrientecaoJuridicaCommand command);

        CommandResult Atualizar(AtualizarTipoOrientecaoJuridicaCommand command);

        CommandResult Remover(int CodigoTipoOrientacaoJuridica);

    }
}
