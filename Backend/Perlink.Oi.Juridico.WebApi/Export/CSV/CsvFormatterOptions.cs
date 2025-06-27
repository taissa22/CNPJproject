using System.Text;

namespace Perlink.Oi.Juridico.WebApi.Export.CSV
{
    public class CsvFormatterOptions
    {
        public CsvFormatterOptions() { }

        public CsvFormatterOptions(string delimiter)
        {
            CsvDelimiter = delimiter;
        }

        public bool UseSingleLineHeaderInCsv { get; set; } = true;

        public string CsvDelimiter { get; set; } = ";";

        public Encoding Encoding { get; set; } = Encoding.GetEncoding("UTF-8");

        public bool IncludeExcelDelimiterHeader { get; set; } = false;
    }
}
