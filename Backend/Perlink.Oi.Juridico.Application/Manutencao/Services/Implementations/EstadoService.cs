using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Commands;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    public class EstadoService : IEstadoService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEstadoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        public EstadoService(IDatabaseContext databaseContext, ILogger<IEstadoService> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }


        public CommandResult Atualizar(AtualizarEstadoCommand command)
        {
            string entityName = nameof(Estado);
            string commandName = $"Atualizar {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {
                command.Validate();
                command.Nome = command.Nome.Trim();
                if (command.Invalid)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.ComandoInvalido(commandName));
                    return CommandResult.Invalid(command.Notifications.ToNotificationsString());
                }

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }


                Estado estado = DatabaseContext.Estados.FirstOrDefault(x => x.Id == command.Id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, command.Id));

                if (estado is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, command.Id));
                }

                if (DatabaseContext.Estados.Any(x => x.Nome.ToUpper().Trim() == command.Nome.ToUpper()) && estado.Nome.ToUpper().Trim() != command.Nome.ToUpper())
                {
                    string mensagem = "Já existe um Estado cadastrado com este nome.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                estado.Atualizar(command.Nome, command.ValorJuros);

                Logger.LogInformation(Infra.Extensions.Logs.AtualizandoEntidade(entityName, command.Id));

                if (estado.HasNotifications)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida(entityName));
                    return CommandResult.Invalid(estado.Notifications.ToNotificationsString());
                }

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }
        public CommandResult Remover(string id)
        {
            string entityName = nameof(Estado);
            string commandName = $"Removendo {entityName}";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            try
            {

                if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESTADO))
                {
                    Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESTADO, UsuarioAtual.Login));
                    return CommandResult.Forbidden();
                }

                Estado estado = DatabaseContext.Estados.FirstOrDefault(x => x.Id == id);
                Logger.LogInformation(Infra.Extensions.Logs.ObtendoEntidade(entityName, id));

                if (estado is null)
                {
                    Logger.LogInformation(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                    return CommandResult.Invalid(Infra.Extensions.Logs.EntidadeNaoEncontrada(entityName, id));
                }

                //if (DatabaseContext.Prepostos.Any(x => x.EstadoId == id))
                //{
                //    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com  Preposto.";
                //    Logger.LogInformation(mensagem);
                //    return CommandResult.Invalid(mensagem);
                //}

                if (DatabaseContext.AdvogadosDosEscritorios.Any(x => x.EstadoId == id))
                {
                    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com Advogado do Escritório.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Comarcas.Any(x => x.EstadoId == id))
                {
                    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com Comarca.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Profissionais.Any(x => x.EstadoId == id))
                {
                    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com Profissional.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.Partes.Any(x => x.EstadoId == id))
                {
                    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com Parte.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                if (DatabaseContext.LinhaProcessos.Any(x => x.EstadoId == id))
                {
                    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com Linhas do Processo.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                var mensagemExclusaoMunicipio = validaExclusaoMunicipio(estado.Municipios);
              
                if (!String.IsNullOrEmpty(mensagemExclusaoMunicipio))
                {
                    Logger.LogInformation(mensagemExclusaoMunicipio);
                    return CommandResult.Invalid(mensagemExclusaoMunicipio);
                }

                estado.Municipios.ToList().ForEach(municipio =>
                {
                    DatabaseContext.Remove(estado);
                    Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(nameof(Municipio), id));

                });

                string commandRemocao = $"Validar remoção {entityName}";
                Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandRemocao));

                DatabaseContext.Remove(estado);
                Logger.LogInformation(Infra.Extensions.Logs.RemovendoEntidade(entityName, id));

                DatabaseContext.SaveChanges();
                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));

                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada(commandName));
                return CommandResult.Valid();
            }
            catch (Exception ex)
            {

                if (ex.HResult == -2146233088 && ex.InnerException != null && ex.InnerException.HResult == -2147467259)
                {
                    string mensagem = "Não é possível excluir esse estado, pois ele está relacionado com alguma tabela.";
                    Logger.LogInformation(mensagem);
                    return CommandResult.Invalid(mensagem);
                }

                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }
        }

        private string validaExclusaoMunicipio(IEnumerable<Municipio> municipios)
        {

            if (DatabaseContext.Processos.Any(p =>
              municipios.Any(m => m.Id == p.MunicipioId && p.EstadoId == m.EstadoId)
              && p.TipoProcessoId == TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Id))
            {
                string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.TRIBUTARIO_ADMINISTRATIVO.Nome+ ".";
                Logger.LogInformation(mensagem);
                return mensagem;
            }

            if (DatabaseContext.Processos.Any(p =>
              municipios.Any(m => m.Id == p.MunicipioId && p.EstadoId == m.EstadoId)
            && p.TipoProcessoId == TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Id))
            {
                string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.TRABALHISTA_ADMINISTRATIVO.Nome + ".";
                return mensagem;
            }

            if (DatabaseContext.Processos.Any(p =>
            municipios.Any(m => m.Id == p.MunicipioId && p.EstadoId == m.EstadoId)
            && p.TipoProcessoId == TipoProcesso.CIVEL_ADMINISTRATIVO.Id))
            {
                string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.CIVEL_ADMINISTRATIVO.Nome + ".";             
                return mensagem;
            }

            if (DatabaseContext.Processos.Any(p =>
            municipios.Any(m => m.Id == p.MunicipioId && p.EstadoId == m.EstadoId)
            && p.TipoProcessoId == TipoProcesso.CRIMINAL_ADMINISTRATIVO.Id))
            {
                string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.CRIMINAL_ADMINISTRATIVO.Nome + ".";
                return mensagem;
            }

            if (DatabaseContext.Processos.Any(p =>
             municipios.Any(m => m.Id == p.MunicipioId && p.EstadoId == m.EstadoId)
            && p.TipoProcessoId == TipoProcesso.DENUNCIA_FISCAL.Id))
            {
                string mensagem = "Não é possível excluir esse município, pois ele está relacionado com Processo " + TipoProcesso.DENUNCIA_FISCAL.Nome + ".";
                return mensagem;
            }

            return String.Empty;
        }

    }
}
