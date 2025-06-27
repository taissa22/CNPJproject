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
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    public class IndicesVigenciasService : IIndicesVigenciasService
    {
        public IndicesVigenciasService(IDatabaseContext databaseContext, ILogger<IIndicesVigenciasService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IIndicesVigenciasService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public CommandResult Criar(CriarIndiceVigenciaCommand command)
        {
            string entityName = "Indice";
            string commandName = $"Criar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();

                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));


                if (Convert.ToDateTime(command.DataVigencia) > DateTime.Now)
                {
                    string mensagem = "Não é permitido incluir vigências com a data maior que hoje.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.IndiceVigencias.Any(v => v.DataVigencia.Date == Convert.ToDateTime(command.DataVigencia).Date && v.ProcessoId == command.Tipoprocessoid))
                    //Where(v => v.DataVigencia.Date == Convert.ToDateTime(command.DataVigencia).Date && v.IndiceId == command.IndiceId && v.ProcessoId == command.Tipoprocessoid).Count() > 0)
                {
                    string mensagem = "Não é permitido incluir mais de uma vigência para uma mesma data.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.IndiceVigencias.Any(v => v.DataVigencia.Date > Convert.ToDateTime(command.DataVigencia).Date && v.ProcessoId == command.Tipoprocessoid))
                {
                    string mensagem = "Não é permitido incluir vigências com data menor que a data maior data já cadastrada.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                IndiceCorrecaoProcesso indice = new IndiceCorrecaoProcesso()
                {
                    DataVigencia = Convert.ToDateTime(command.DataVigencia),
                    IndiceId = command.IndiceId,
                    ProcessoId = command.Tipoprocessoid
                };

                if (indice.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(indice.Notifications.ToNotificationsString());
                }

                DatabaseContext.Add(indice);
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

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

        public CommandResult Remover(int codigoIndice, DateTime dataVigencia)
        {
            string entityName = "Indice";
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_INDICE))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(permissao: Permissoes.ACESSAR_INDICE, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

             
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, $"{codigoIndice}"));
                IndiceCorrecaoProcesso indice = DatabaseContext.IndiceVigencias.FirstOrDefault(x => x.ProcessoId == codigoIndice && x.DataVigencia == dataVigencia);

                if (indice is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoIndice}"));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, $"{codigoIndice}"));
                }

                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, $"{codigoIndice}"));
                DatabaseContext.Remove(indice);

                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade($"{codigoIndice}"));
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
    }
}
