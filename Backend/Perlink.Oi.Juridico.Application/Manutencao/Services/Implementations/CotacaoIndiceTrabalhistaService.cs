using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    internal class CotacaoIndiceTrabalhistaService : ICotacaoIndiceTrabalhistaService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<ICotacaoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CotacaoIndiceTrabalhistaService(IDatabaseContext databaseContext, ILogger<ICotacaoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }        

        public CommandResult Remover(DateTime dataCorrecao, DateTime dataBase)
        {
            string entityName = "Cotação Indice Trabalhista";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {                

                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{dataCorrecao}, {dataBase}"));
                CotacaoIndiceTrabalhista cotacao = DatabaseContext.CotacaoIndiceTrabalhistas.FirstOrDefault(x => x.DataBase.ToShortDateString() == dataBase.ToShortDateString() && x.DataCorrecao.ToShortDateString() == dataCorrecao.ToShortDateString());

                if (cotacao is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{dataCorrecao}, {dataBase}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{dataCorrecao}, {dataBase}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{dataCorrecao}, {dataBase}"));
                DatabaseContext.Remove(cotacao);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{dataCorrecao}, {dataBase}"));
                DatabaseContext.SaveChanges();

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        public CommandResult InserirTemporaria(List<CotacaoIndiceTrabalhista> lista)
        {
            DatabaseContext.ExecuteSqlInterpolated("delete from JUR.TEMP_COTACAO_INDICE_TRAB");
            DatabaseContext.SaveChanges();
            foreach (var item in lista)
            {
                var temp = TempCotacaoIndiceTrab.Criar(item.DataCorrecao, item.DataBase, item.ValorCotacao);
                DatabaseContext.Add(temp);
                DatabaseContext.SaveChanges();
            }

            return CommandResult.Valid();
        }

        public CommandResult LimparTemporaria()
        {
            return CommandResult.Valid();
        }

        public CommandResult AplicarImportacao()
        {
            var temporarios = DatabaseContext.TempCotacaoIndiceTrabalhistas;
            foreach (var item in temporarios)
            {
                var cotacao = CotacaoIndiceTrabalhista.Criar(item.DataCorrecao, item.DataBase, item.ValorCotacao);
                DatabaseContext.Add(cotacao);
                DatabaseContext.SaveChanges();
            }
            DatabaseContext.ExecuteSqlInterpolated("delete from JUR.TEMP_COTACAO_INDICE_TRAB");
            DatabaseContext.SaveChanges();
            return CommandResult.Valid();
        }
    }
}

