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
    public class MigCategoriaPagamentoEstrategicoService : BaseCrudService<MigracaoCategoriaPagamentoEstrategico, long>, IMigCategoriaPagamentoEstrategicoService
    {

        private readonly IMigCategoriaPagamentoEstrategicoRepository repository;

        public MigCategoriaPagamentoEstrategicoService(IMigCategoriaPagamentoEstrategicoRepository repository) : base(repository)
        {
            this.repository = repository;
        }

        public async Task<MigracaoCategoriaPagamentoEstrategico> AtualizarMigCategoriaPagamentoEstrategico(MigracaoCategoriaPagamentoEstrategico migCategoriaPagamentoEstrategico)
        {
            return await repository.AtualizarMigCategoriaPagamentoEstrategico(migCategoriaPagamentoEstrategico);
        }

        public async Task<MigracaoCategoriaPagamentoEstrategico> BuscarPorIDMigCategoriaPagamentoEstrategico(long migCodigoEstrategico)
        {
            return await repository.BuscarPorIDMigCategoriaPagamentoEstrategico(migCodigoEstrategico);
        }

        public async Task<MigracaoCategoriaPagamentoEstrategico> CadastrarMigCategoriaPagamentoEstrategico(MigracaoCategoriaPagamentoEstrategico migCategoriaPagamentoEstrategico)
        {
            return await repository.CadastrarMigCategoriaPagamentoEstrategico(migCategoriaPagamentoEstrategico);
        }

        public void RemoverCodMigEstrategico(long migCodigoEstrategico)
        {
            repository.RemoverCodMigEstrategico(migCodigoEstrategico);
        }
    }
}
