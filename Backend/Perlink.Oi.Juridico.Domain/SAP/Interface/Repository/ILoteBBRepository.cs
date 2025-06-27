using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Perlink.Oi.Juridico.Domain.SAP.DTO;
using Perlink.Oi.Juridico.Domain.SAP.Entity;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.SAP.Interface.Repository
{
    public interface ILoteBBRepository : IBaseCrudRepository<Lote, long>
    {
        string[] ValidaLancamentosLoteBB(long numeroLoteBB);

        bool MontarHeader(ArquivoBBDTO arquivoBBDTO);

        bool MontarDetalheArquivo(ArquivoBBDTO arquivoBBDTO);

        bool MontarTrailerArquivo(ArquivoBBDTO arquivoBBDTO);

        bool ExisteParametroConvenio(long codigoLote, string codigoEstado);

        Task<IList<LancamentoProcesso>> ObterLancamentosPorLoteComAssociacao(long codigoLote);

        Task<List<string>> RecuperarEstados(long CodigoLote);
    }
}