using Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Commands;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Services
{
    public interface IAudienciaService
    {
        CommandResult Atualizar(AtualizarAudienciaCommand command);
    }
}