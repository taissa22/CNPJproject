using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services
{
    public interface ICotacaoIndiceTrabalhistaService
    {       
        CommandResult Remover(DateTime dataCorrecao, DateTime dataBase);

        CommandResult InserirTemporaria(List<CotacaoIndiceTrabalhista> lista);

        CommandResult LimparTemporaria();

        CommandResult AplicarImportacao();
    }
}