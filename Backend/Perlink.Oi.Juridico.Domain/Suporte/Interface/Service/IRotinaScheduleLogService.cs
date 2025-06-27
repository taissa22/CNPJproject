using Perlink.Oi.Juridico.Domain.Suporte.Entity;
using Perlink.Oi.Juridico.Domain.Suporte.Enum;
using Shared.Domain.Interface.Service;
using System;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Suporte.Interface.Service
{
    public interface IRotinaScheduleLogService : IBaseService<RotinaScheduleLog, long>
    {
        Task InserirLog(RotinaScheduleEnum rotina, string mensagem, long idRegistro);
        Task InserirLogError(RotinaScheduleEnum rotina, Exception ex, long idRegistro);
        Task InserirLog(RotinaScheduleEnum rotina, string mensagem, long idRegistro, bool visualizacaoEstaDisponivel);
        Task InserirLogError(RotinaScheduleEnum rotina, Exception ex, long idRegistro, bool visualizacaoEstaDisponivel);
    }
}
