using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Application.Manutencao.Sorts;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Infra;
using Perlink.Oi.Juridico.Infra.Constants;
using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Extensions;
using Perlink.Oi.Juridico.Infra.External;
using Perlink.Oi.Juridico.Infra.Providers;
using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Perlink.Oi.Juridico.Application.Manutencao.Repositories.Implementations
{
    internal class EscritorioEstadoRepository : IEscritorioEstadoRepository
    {
        private IDatabaseContext DatabaseContext { get; }
        private ILogger<IEscritorioEstadoRepository> Logger { get; }
        private IUsuarioAtualProvider UsuarioAtual { get; }

        public EscritorioEstadoRepository(IDatabaseContext databaseContext, ILogger<IEscritorioEstadoRepository> logger, IUsuarioAtualProvider usuarioAtual)
        {
            DatabaseContext = databaseContext;
            Logger = logger;
            UsuarioAtual = usuarioAtual;
        }

        private IQueryable<EscritorioEstadoDTO> ObterBase(int escritorioId, int tipoProcessoid)
        {
            string logName = "Escritório";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Ordenar {logName}"));


            IQueryable<EscritorioEstadoDTO> query = (from a in DatabaseContext.EscritoriosEstados
                                                  where (tipoProcessoid <= 0 || a.TipoProcesso.Id == tipoProcessoid) && (a.Profissional.Id == escritorioId)
                                                  select new EscritorioEstadoDTO(a.Estado.Id,true,a.TipoProcesso.Id));
           
            return query;
        }

      
        public CommandResult<IReadOnlyCollection<EscritorioEstadoDTO>> Obter(int escritorioId, int tipoProcessoid)
        {
            string logName = "Escritório Estado";
            Logger.LogInformation(Infra.Extensions.Logs.IniciandoOperacao($"Obter {logName}"));

            if (!UsuarioAtual.TemPermissaoPara(Permissoes.ACESSAR_ESCRITORIO))
            {
                Logger.LogInformation(Infra.Extensions.Logs.PermissaoNegada(Permissoes.ACESSAR_ESCRITORIO, UsuarioAtual.Login));
                return CommandResult<IReadOnlyCollection<EscritorioEstadoDTO>>.Forbidden();
            }

            Logger.LogInformation(Infra.Extensions.Logs.Obtendo(logName));
            var resultado = ObterBase(escritorioId, tipoProcessoid).ToArray();


            Logger.LogInformation(Infra.Extensions.Logs.Retornando(logName));
            return CommandResult<IReadOnlyCollection<EscritorioEstadoDTO>>.Valid(resultado);
        }
    }
}
