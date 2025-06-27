using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface IVaraService
    {
        CommandResult Criar(CriarVaraCommand command);
        CommandResult Atualizar(AtualizarVaraCommand dados);
        CommandResult Remover(int VaraId, int ComarcaId, int TipoVaraId);
    }
}
