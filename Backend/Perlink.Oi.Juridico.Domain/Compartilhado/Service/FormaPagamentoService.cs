using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository;
using Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service;
using Perlink.Oi.Juridico.Domain.SAP.DTO.ManutencaoFormaPagamento;
using Shared.Domain.Impl.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Service
{
    public class FormaPagamentoService : BaseCrudService<FormaPagamento, long>, IFormaPagamentoService
    {
        IFormaPagamentoRepository repository;

        public FormaPagamentoService(IFormaPagamentoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<FormaPagamentoGridDTO>> GetFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros)
        {
            return await repository.GetFormaPagamentoGridManutencao(filtros);
        }

        public async Task<int> GetTotalFormaPagamentoGridManutencao(FormaPagamentoFiltroDTO filtros)
        {
            return await repository.GetTotalFormaPagamentoGridManutencao(filtros);
        }

        public async Task<IEnumerable<FormaPagamentoGridExportarDTO>> GetFormaPagamentoGridExportarManutencao(FormaPagamentoFiltroDTO filtroDTO)
        {
            return await repository.GetFormaPagamentoGridExportarManutencao(filtroDTO);
        }
        public async Task<FormaPagamento> CadastrarFormaPagamento(FormaPagamento formaPagamento)
        {
            return await repository.CadastrarFormaPagamento(formaPagamento);
        }

    }
}
