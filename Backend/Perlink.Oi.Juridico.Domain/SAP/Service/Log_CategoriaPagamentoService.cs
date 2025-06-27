using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
    public class Log_CategoriaPagamentoService : BaseCrudService<Log_CategoriaPagamento, long>, ILog_CategoriaPagamentoService
    {
        private readonly ILog_CategoriaPagamentoRepository repository;
        public Log_CategoriaPagamentoService(ILog_CategoriaPagamentoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task IncluirLog(Log_CategoriaPagamento categoriaLog)
        {
            await repository.Inserir(categoriaLog);
        }
    }
}