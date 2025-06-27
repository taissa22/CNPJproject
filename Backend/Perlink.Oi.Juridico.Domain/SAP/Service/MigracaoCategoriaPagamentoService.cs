using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Service;
using Shared.Domain.Impl.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Service
{
  public class MigracaoCategoriaPagamentoService : BaseCrudService<MigracaoCategoriaPagamento, long>, IMigracaoCategoriaPagamentoService
    {
        private readonly IMigracaoCategoriaPagamentoRepository repository;

        public MigracaoCategoriaPagamentoService(IMigracaoCategoriaPagamentoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<MigracaoCategoriaPagamento> AtualizarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento)
        {
            return await repository.AtualizarMigracaoCategoriaPagamento(migCategoriaPagamento);
        }

        public async Task<MigracaoCategoriaPagamento> CadastrarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento)
        {
            return await repository.CadastrarMigracaoCategoriaPagamento(migCategoriaPagamento);
        }

        public async Task<MigracaoCategoriaPagamento> BuscarPorIDMigracaoCategoriaPagamento(long migCodigoTipoProcesso)
        {
            return await repository.BuscarPorIDMigracaoCategoriaPagamento(migCodigoTipoProcesso);
        }

        public async Task ExcluirCategoria(long migCodigoTipoProcesso)
        {
            await repository.RemoverPorId(migCodigoTipoProcesso);
        }

        public  void RemoverCodMigCivel(long migCodigoCivel)
        {
             repository.RemoverCodMigCivel(migCodigoCivel);
        }
    }
}
