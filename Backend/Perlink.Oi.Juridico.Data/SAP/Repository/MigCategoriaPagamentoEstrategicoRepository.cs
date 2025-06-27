using Microsoft.EntityFrameworkCore;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Perlink.Oi.Juridico.Domain.SAP.Interface.Repository;
using Shared.Data;
using Shared.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Data.SAP.Repository
{
   public class MigCategoriaPagamentoEstrategicoRepository : BaseCrudRepository<MigracaoCategoriaPagamentoEstrategico, long>, IMigCategoriaPagamentoEstrategicoRepository
    {

        private readonly JuridicoContext dbContext;

        public MigCategoriaPagamentoEstrategicoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<MigracaoCategoriaPagamentoEstrategico> AtualizarMigCategoriaPagamentoEstrategico(MigracaoCategoriaPagamentoEstrategico migCategoriaPagamentoEstrategico)
        {
            await base.Atualizar(migCategoriaPagamentoEstrategico);
            return migCategoriaPagamentoEstrategico;
        }

        public async Task<MigracaoCategoriaPagamentoEstrategico> BuscarPorIDMigCategoriaPagamentoEstrategico(long migCodigoEstrategico)
        {
            return await dbContext.MigracaoCategoriaPagamentoEstrategico.FirstOrDefaultAsync(x => x.CodCategoriaPagamentoEstra == migCodigoEstrategico);
        }

        public  async Task<MigracaoCategoriaPagamentoEstrategico> CadastrarMigCategoriaPagamentoEstrategico(MigracaoCategoriaPagamentoEstrategico migCategoriaPagamentoEstrategico)
        {
            await base.Inserir(migCategoriaPagamentoEstrategico);
            return migCategoriaPagamentoEstrategico;
        }

        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async void RemoverCodMigEstrategico(long migCodigoEstrategico)
        {
            var buscaPorIdConsumidor = await dbContext.MigracaoCategoriaPagamentoEstrategico.FirstOrDefaultAsync(x => x.CodCategoriaPagamentoEstra == migCodigoEstrategico);

            dbContext.Remove(buscaPorIdConsumidor);
        }
    }
}
