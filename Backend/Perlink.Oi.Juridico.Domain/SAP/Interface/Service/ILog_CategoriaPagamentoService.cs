using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
    public interface ILog_CategoriaPagamentoService : IBaseCrudService<Log_CategoriaPagamento, long>
    {
        Task IncluirLog(Log_CategoriaPagamento categoriaLog);
    }
}
