using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Handlers.Implementations
{
    public class XlsxHandler : IXlsxHandler {

        public CommandResult<bool> ArquivoVazio(string file) {
            try {
            
                bool arquivoVazio = true;
                using (FastExcel.FastExcel xlsxFile = new FastExcel.FastExcel(new FileInfo(file), true)) {
                    FastExcel.Worksheet worksheet = xlsxFile.Read(1);                 

                    foreach (var row in worksheet.Rows) {
                        //pula o cabeçalho
                        if (row.RowNumber < 2) {
                            continue;
                        }
                        arquivoVazio = string.IsNullOrEmpty(row.GetCellByColumnName("A").Value.ToString());
                        break;
                    }
                }

                return CommandResult<bool>.Valid(arquivoVazio);
            } catch (Exception ex) {
                return CommandResult<bool>.Invalid(ex.Message);
            }
            
        }

        public CommandResult<bool> LayoutValido(string file, int colummNumber) {
            try {
                var cellsCount = 0;
                using (FastExcel.FastExcel xlsxFile = new FastExcel.FastExcel(new FileInfo(file), true)) {
                    FastExcel.Worksheet worksheet = xlsxFile.Read(1);

                    var row = worksheet.Rows.First();
                    cellsCount = row.Cells.Count();                    
                }
                return CommandResult<bool>.Valid(cellsCount == colummNumber);
            } catch (Exception ex) {
                return CommandResult<bool>.Invalid(ex.Message);
            }

        }
    }
}
