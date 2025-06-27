using PdfSharpCore.Pdf.IO;
using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;
using System.Text;
using PigPDFdoc = UglyToad.PdfPig.PdfDocument;
using SharpPDFdoc = PdfSharpCore.Pdf.PdfDocument;

namespace Perlink.Oi.Juridico.Infra.Handlers.Implementations {
    public class PdfHandler : IPdfHandler {
        public CommandResult<string> ExtractWordFromPdf(string filePath, int wordIndex) {
            try {
                using PigPDFdoc documentToExtract = PigPDFdoc.Open(filePath);
                return CommandResult<string>.Valid(documentToExtract.GetPage(1).GetWords().Skip(wordIndex - 1).FirstOrDefault().Text);
            }
            catch (Exception ex)
            {
                return CommandResult<string>.Invalid(ex.Message);
            }
        }

        public CommandResult SplitPdfToPages(string filePath, string outputDirectory)
        {
           string RemoveSpecialChar(string filename) {
                StringBuilder sb = new StringBuilder();
                foreach (char c in filename) {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_') {
                        sb.Append(c);
                    }
                }
                return sb.ToString();
            }

            try
            {
                using SharpPDFdoc documentToSplit = PdfReader.Open(filePath, PdfDocumentOpenMode.Import);

                string fileName = Path.GetFileNameWithoutExtension(filePath);

                fileName = RemoveSpecialChar(fileName);

                for (int i = 0; i < documentToSplit.PageCount; i++)
                {
                    var page = documentToSplit.Pages[i];

                    var pagefileName = $"{ fileName }page{ i + 1 }.pdf";

                    using SharpPDFdoc newDocument = new SharpPDFdoc();
                    newDocument.AddPage(page);
                    newDocument.Save(Path.Combine(outputDirectory, pagefileName));
                }

                return CommandResult.Valid();
            }
            catch (Exception ex)
            {
                return CommandResult.Invalid(ex.Message);
            }
        }
    }
}
