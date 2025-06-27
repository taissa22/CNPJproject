using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
  public interface IMigCategoriaPagamentoEstrategicoRepository : IBaseCrudRepository<MigracaoCategoriaPagamentoEstrategico, long>
    {
        Task<MigracaoCategoriaPagamentoEstrategico> CadastrarMigCategoriaPagamentoEstrategico(MigracaoCategoriaPagamentoEstrategico migCategoriaPagamentoEstrategico);

        Task<MigracaoCategoriaPagamentoEstrategico> AtualizarMigCategoriaPagamentoEstrategico(MigracaoCategoriaPagamentoEstrategico migCategoriaPagamentoEstrategico);

        Task<MigracaoCategoriaPagamentoEstrategico> BuscarPorIDMigCategoriaPagamentoEstrategico(long migCodigoEstrategico);

        void RemoverCodMigEstrategico(long migCodigoEstrategico);
    }
}
