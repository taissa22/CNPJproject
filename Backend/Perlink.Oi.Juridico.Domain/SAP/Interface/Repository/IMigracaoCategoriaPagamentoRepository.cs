using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
   public interface IMigracaoCategoriaPagamentoRepository : IBaseCrudRepository<MigracaoCategoriaPagamento, long>
    {

        Task<MigracaoCategoriaPagamento> CadastrarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento);

        Task<MigracaoCategoriaPagamento> AtualizarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento);    

        Task<MigracaoCategoriaPagamento> BuscarPorIDMigracaoCategoriaPagamento(long migCodigoTipoProcesso);
       void RemoverCodMigCivel(long migCodigoCivel);


    }
}
