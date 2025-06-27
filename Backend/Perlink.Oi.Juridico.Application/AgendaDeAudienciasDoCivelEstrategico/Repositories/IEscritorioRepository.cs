using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories {

    public interface IEscritorioRepository {

        CommandResult<IReadOnlyCollection<Escritorio>> Obter();

        CommandResult<IReadOnlyCollection<Escritorio>> ObterAutorizadosAoUsuarioLogado();
    }
}