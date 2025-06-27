using Perlink.Oi.Juridico.Domain.Suporte.Entity;
using Perlink.Oi.Juridico.Domain.Suporte.Enum;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Suporte.Interface.Repository
{
    public interface IRotinaScheduleLogRepository : IBaseCrudRepository<RotinaScheduleLog, long>
    {
        Task<IEnumerable<RotinaScheduleLog>> ListarPorRotinaERegistro(RotinaScheduleEnum rotina, Int64 idRegistro);

    }
}
