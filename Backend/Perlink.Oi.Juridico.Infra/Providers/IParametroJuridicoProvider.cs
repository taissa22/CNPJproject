using Perlink.Oi.Juridico.Infra.Entities;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Infra.Providers {
    public interface IParametroJuridicoProvider {
        Queue<string> TratarCaminhoDinamico(string parametroId);
        CommandResult<ParametroJuridico> Obter(string parametroId);
    }
}
