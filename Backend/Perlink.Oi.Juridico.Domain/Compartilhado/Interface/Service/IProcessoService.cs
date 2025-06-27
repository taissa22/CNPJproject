using Microsoft.Extensions.Logging;
using Perlink.Oi.Juridico.Domain.Compartilhado.DTO;
using Perlink.Oi.Juridico.Domain.Compartilhado.Enum;
using Perlink.Oi.Juridico.Domain.Compartilhado.Entities;
using Shared.Domain.Interface.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.Interface.Service
{
    public interface IProcessoService : IBaseCrudService<Processo, long> {
        Task<ICollection<Processo>> RecuperarPorIdentificador(string numeroProcesso, long codigoInterno, long codigoTipoProcesso);
        Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoInterno(long codigoProcesso, long codigoTipoProcesso, bool ehEscritorio, IList<long> codigosEscritorio = null);
        Task<ICollection<ProcessoDTO>> RecuperarProcessoPeloCodigoTipoProcesso(string numeroProcesso, long codigoTipoProcesso, bool ehEscritorio, IList<long> codigosEscritorio = null);
        Task<string> ExportacaoPrePosRj(TipoProcessoEnum tipoProcesso, ILogger logger);
        Task ExpurgoPrePosRj(ILogger logger);
    }
}
