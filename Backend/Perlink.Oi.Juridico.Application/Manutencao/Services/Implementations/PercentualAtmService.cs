using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Repositories;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Enums;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;
using EnumEstado = Perlink.Oi.Juridico.Infra.Enums.EstadoEnum;

namespace Perlink.Oi.Juridico.Application.Manutencao.Services.Implementations
{
    public class PercentualAtmService : IPercentualAtmService
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IAssuntoService> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }
        private IPercentualATMRepository PercentualAtmRepository { get; }

        public PercentualAtmService(IDatabaseContext databaseContext, ILogger<IAssuntoService> logger, IUsuarioAtualProvider usuarioAtual, IPercentualATMRepository repository)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
            PercentualAtmRepository = repository;
        }

        public CommandResult AtualizarPercentualAtmCC(IFormFile upload, DateTime dataVigencia, int codTipoProcesso)
        {
            string entityName = "PercentualATM";
            string commandName = $"Criar / Atualizar {entityName}";

            string _msgPlanilhaForaPadrao = "Planilha fora do padrão esperado. Por favor, revise-a.";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao(commandName));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_PERCENTUAL_ATM))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_PERCENTUAL_ATM, UsuarioAtual.Login));
                return CommandResult.Forbidden();
            }

            try
            {
                using (var reader = new StreamReader(upload.OpenReadStream()))
                {
                    reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(';');

                        if (values.Count() != 2 || !EnumEstado.IsValid(values[0].Trim()))
                        {
                            Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"UF: {values[0].Trim()} - registro inválido."));
                            throw new Exception(_msgPlanilhaForaPadrao);
                        }

                        this.ValidarPercenutual(values[1].Trim());

                        var percentualUFBD = PercentualAtmRepository.RecuperarVigenciaParaUF(values[0].Trim(), dataVigencia, codTipoProcesso);
                        Logger.LogInformation(Infra.Extensions.Logs.CriandoEntidade(entityName));

                        if (percentualUFBD is null)
                        {
                            var estado = EnumEstado.PorId(values[0].Trim());

                            PercentualATM _percentualUF = PercentualATM.CriarPercentualAtm(estado, values[1],
                            dataVigencia, codTipoProcesso);

                            if (_percentualUF.HasNotifications)
                            {
                                Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"UF: {_percentualUF.EstadoId} - registro inválido."));
                                throw new Exception(_percentualUF.Notifications.FirstOrDefault().Message);
                            }
                            else
                            {
                                DatabaseContext.Add(_percentualUF);
                                Logger.LogInformation(Infra.Extensions.Logs.SalvandoEntidade(entityName));
                                Logger.LogInformation(Infra.Extensions.Logs.OperacaoFinalizada($"UF: {_percentualUF.EstadoId} registrada com sucesso."));
                            }
                        }
                        else
                        {
                            percentualUFBD.AtualizarPercentualAtm(Convert.ToDecimal(values[1].Replace(".", ",").Trim()), codTipoProcesso);
                            if (percentualUFBD.HasNotifications)
                            {
                                Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"UF: {percentualUFBD.EstadoId} - registro inválido."));
                                throw new Exception(percentualUFBD.Notifications.FirstOrDefault().Message);
                            }
                        }
                    }
                }
                DatabaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, Infra.Extensions.Logs.OperacaoComErro(commandName));
                return CommandResult.Invalid(ex.Message);
            }

            return CommandResult.Valid();
        }

        private bool ValidarPercenutual(string percentual)
        {
            const string _msgPercentualZero = "Existem percentuais inválidos.";
            const string _msgPercentualAcimaPermitido = "Existem percentuais maiores do que 100%.";
            decimal _percentual = 0;

            if (string.IsNullOrEmpty(percentual))
            {
                Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{percentual} - registro inválido."));
                throw new Exception(_msgPercentualZero);
            }

            if (!decimal.TryParse(percentual, out _percentual))
            {
                Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{percentual} - registro inválido."));
                throw new Exception(_msgPercentualZero);
            }

            if (_percentual > 100)
            {
                Logger.LogInformation(Infra.Extensions.Logs.EntidadeInvalida($"{percentual} - registro inválido."));
                throw new Exception(_msgPercentualAcimaPermitido);
            }

            return true;
        }
    }
}