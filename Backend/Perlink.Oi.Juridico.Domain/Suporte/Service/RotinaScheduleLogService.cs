using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.Suporte.Entity;
using Perlink.Oi.Juridico.Domain.Suporte.Enum;
using Perlink.Oi.Juridico.Domain.Suporte.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Suporte.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Suporte.Service
{
    public class RotinaScheduleLogService : BaseService<RotinaScheduleLog, long>, IRotinaScheduleLogService
    {
        private readonly IRotinaScheduleLogRepository repository;
        private readonly ILogger logger;
        public RotinaScheduleLogService(IRotinaScheduleLogRepository repository, ILogger<RotinaScheduleLogService> logger) : base(repository)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task InserirLog(RotinaScheduleEnum rotina, string mensagem, long idRegistro)
        {
            await InserirLog(rotina, mensagem, idRegistro, true);
        }

        public async Task InserirLog(RotinaScheduleEnum rotina, string mensagem, long idRegistro, bool visualizacaoEstaDisponivel)
        {

            this.logger.LogInformation($"{mensagem} - {idRegistro}");

            var log = new RotinaScheduleLog
            {
                IdRotina = rotina.GetHashCode(),
                IdRegistro = idRegistro,
                Mensagem = mensagem,
                VisualizacaoEstaDisponivel = visualizacaoEstaDisponivel,
                DataDaOcorrencia = DateTime.Now,
                Status = 0
            };

            await repository.Inserir(log);
            await repository.CommitAsync();
        }

        public async Task InserirLogError(RotinaScheduleEnum rotina, Exception ex, long idRegistro)
        {
            await InserirLogError(rotina, ex, idRegistro, true);
        }

        public async Task InserirLogError(RotinaScheduleEnum rotina, Exception ex, long idRegistro, bool visualizacaoEstaDisponivel)
        {

            Exception excecao = ex;
            var body = new StringBuilder();

            body.AppendLine("Ocorreu um Erro Inesperado:");

            while (excecao != null)
            {

                body.AppendLine($"Erro: {excecao.Message}");
                body.AppendLine($"StackTrace: {excecao.StackTrace}");
                body.AppendLine("-----------------");

                excecao = excecao.InnerException;
            }

            this.logger.LogError($"{body} - {idRegistro}");

            var log = new RotinaScheduleLog
            {
                IdRotina = rotina.GetHashCode(),
                IdRegistro = idRegistro,
                Mensagem = body.ToString(),
                VisualizacaoEstaDisponivel = visualizacaoEstaDisponivel,
                DataDaOcorrencia = DateTime.Now,
                Status = 1
            };

            await repository.Inserir(log);
            await repository.CommitAsync();
        }
    }
}
