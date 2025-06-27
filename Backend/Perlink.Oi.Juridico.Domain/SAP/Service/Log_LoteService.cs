using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class Log_LoteService : BaseCrudService<Log_Lote, long>, ILog_LoteService
    {
        private readonly ILog_LoteRepository repository;
        public Log_LoteService(ILog_LoteRepository repository) : base(repository)
        {
            this.repository = repository;
        }


        public async Task<IEnumerable<LogLoteDTO>> ObterHistorico(long codigoLote)
        {
            return await repository.ObterHistorico(codigoLote);
        }
    }
}