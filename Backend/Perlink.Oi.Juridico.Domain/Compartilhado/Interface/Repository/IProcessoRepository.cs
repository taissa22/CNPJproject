using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Repository
{
    public interface IProcessoRepository : IBaseCrudRepository<Processo, long> {
        Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso);

        Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, IList<long> codigosEscritorio);

        Task<ICollection<Processo>> RecuperarPorIdentificador(string numeroProcesso, long codigoProcesso, long codigoTipoProcesso);

        Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoInterno(long codigoProcesso, long codigoTipoProcesso);

        Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoInterno(long codigoProcesso, long codigoTipoProcesso, IList<long> codigosEscritorio);

        Task<IEnumerable<ProcessoExportacaoPrePosRJDTO>> ExportacaoPrePosRj(TipoProcessoEnum tipoProcesso, int qtdeMesesParaExportacao);

        Task<IEnumerable<ExportacaoPrePosRJ>> ExpurgoPrePosRj(int parametro);

        string RecuperarNumeroProcessoCartorio(long ID);
    }
}
