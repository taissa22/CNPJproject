using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services {
    public interface IDocumentoService {

        CommandResult ExecutarAgendamento();
        CommandResult ExecutarRotinaPeloNAS(string arquivoDePara);
        CommandResult ExecutarRotina(string arquivoDePara);

        CommandResult<ProcessamentoCargaInfo> ProcessaCargaDocumentos(string arquivoDePara, int agendamentoId);
        void Expurgar();
    }
}
