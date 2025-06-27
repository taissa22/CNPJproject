using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Repositories {
    public interface IParametroJuridicoRepository {
        CommandResult<ParametroJuridico> Obter(string parametroId);
    }
}
