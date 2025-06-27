using Perlink.Oi.Juridico.Application.DocumentoCredor.Enums;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.IO.Compression;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories
{
    public interface IAgendamentoDeFechamentoAtmUfPexRepository
    {
        CommandResult<Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations.Arquivo> ObterArquivoBase(int agendamentoId);
        CommandResult<Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations.Arquivo> ObterArquivoRelatorio(int agendamentoId);
    }

    
}
