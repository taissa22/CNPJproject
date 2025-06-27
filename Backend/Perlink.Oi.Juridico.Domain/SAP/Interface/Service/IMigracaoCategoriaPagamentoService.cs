using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Service
{
  public interface IMigracaoCategoriaPagamentoService : IBaseCrudService<MigracaoCategoriaPagamento, long>
    {
        Task<MigracaoCategoriaPagamento> CadastrarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento);

        Task<MigracaoCategoriaPagamento> AtualizarMigracaoCategoriaPagamento(MigracaoCategoriaPagamento migCategoriaPagamento);        

        Task<MigracaoCategoriaPagamento> BuscarPorIDMigracaoCategoriaPagamento(long migCodigoTipoProcesso);

        void RemoverCodMigCivel(long migCodigoCivel);



    }
}
