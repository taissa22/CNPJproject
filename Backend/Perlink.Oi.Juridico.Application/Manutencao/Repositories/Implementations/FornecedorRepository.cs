using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ICentroDeCustoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public FornecedorRepository(IDatabaseContext databaseContext, ILogger<ICentroDeCustoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        public CommandResult<IReadOnlyCollection<Fornecedor>> Obter()
        {
            string logName = "Fornecedores";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));
            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_EMPRESA_DO_GRUPO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_EMPRESA_DO_GRUPO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<Fornecedor>>.Forbidden();
            }

            var parametro = DatabaseContext.ParametrosJuridicos
                                           .FirstOrDefault(x => x.Parametro.Equals("TP_FORNECEDOR_SAP"));

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var result = DatabaseContext.Fornecedores
                                  .Where(x => x.TipoFornecedorId != Int32.Parse(parametro.Conteudo))
                                  .AsNoTracking()
                                  .ToArray();

            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<Fornecedor>>.Valid(result);
        }
    }
}