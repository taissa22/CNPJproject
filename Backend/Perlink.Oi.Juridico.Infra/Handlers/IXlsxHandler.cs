using Microsoft.AspNetCore.Http;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Infra.Handlers {
    public interface IXlsxHandler {
        CommandResult<bool> ArquivoVazio(string file);
        CommandResult<bool> LayoutValido(string file, int colummNumber);
    }
}
