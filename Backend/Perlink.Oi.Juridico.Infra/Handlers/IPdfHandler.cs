using Perlink.Oi.Juridico.Infra.Seedwork;

namespace Perlink.Oi.Juridico.Infra.Handlers {
    public interface IPdfHandler {

        CommandResult SplitPdfToPages(string filePath, string outputDirectory);

        CommandResult<string> ExtractWordFromPdf(string filePath, int wordIndex);
    }
}
