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
   public class MigracaoCategoriaPagamentoRepository : BaseCrudRepository<MigracaoCategoriaPagamento, long> , IMigracaoCategoriaPagamentoRepository
    {

        private readonly JuridicoContext dbContext;

        public MigracaoCategoriaPagamentoRepository(JuridicoContext dbContext, IAuthenticatedUser user) : base(dbContext, user)
        {
            this.dbContext = dbContext;
        }

        public async Task<MigracaoCategoriaPagamento> AtualizarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento)
        {
            await base.Atualizar(migCategoriaPagamento);
            return migCategoriaPagamento;
        }

        public async Task<MigracaoCategoriaPagamento> CadastrarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento)
        {
            await base.Inserir(migCategoriaPagamento);
            return migCategoriaPagamento;
        }
      

        public async Task<MigracaoCategoriaPagamento> BuscarPorIDMigracaoCategoriaPagamento(long migCodigoTipoProcesso)
        {
            return await dbContext.MigracaoCategoriaPagamento.FirstOrDefaultAsync(x => x.CodCategoriaPagamentoCivel == migCodigoTipoProcesso); 

        }
        public override Task<IDictionary<string, string>> RecuperarDropDown()
        {
            throw new NotImplementedException();
        }

        public async void RemoverCodMigCivel(long migCodigoCivel)
        {
            var busca = await dbContext.MigracaoCategoriaPagamento.FirstOrDefaultAsync(x => x.CodCategoriaPagamentoCivel == migCodigoCivel);

            dbContext.Remove(busca);
        }
    }
}
