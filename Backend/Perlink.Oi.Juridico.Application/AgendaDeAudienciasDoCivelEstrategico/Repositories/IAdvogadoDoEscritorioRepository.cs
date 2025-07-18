﻿using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.AgendaDeAudienciasDoCivelEstrategico.Repositories {

    public interface IAdvogadoDoEscritorioRepository {

        CommandResult<IReadOnlyCollection<AdvogadoDoEscritorio>> ObterPorEscritorio(int escritorioId);
    }
}