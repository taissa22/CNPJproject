using Perlink.Oi.Juridico.Infra.Dto;
using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Application.DocumentoCredor.Services {
    public interface IComprovanteService {

        CommandResult ExecutarAgendamento();
        CommandResult ExecutarRotinaPeloNAS(string arquivoPDFComprovantes, string arquivoBaseSAP);
        CommandResult ExecutarRotina(string arquivoPDFComprovantes, string arquivoBaseSAP);
        CommandResult<string> GeraCargaArquivoComprovantes(string pdfFile, string sapFbl3nFile);
        CommandResult<ProcessamentoCargaInfo> ProcessaCargaComprovantes(string zipFile, int agendamentoId);
        CommandResult Expurgar();
    }
}
