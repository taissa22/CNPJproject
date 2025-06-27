using System;
using System.IO;
using System.Linq;

namespace Oi.Juridico.Shared.V2.Helpers
{
    public class CsvFileHelper
    {
        public static bool ArquivoVazio(string file)
        {
            try
            {
                return File.ReadAllLines(file).Count() < 2;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool QuantidadeColunasValida(string file, int numColunas)
        {
            try
            {
                var fileCollumms = File.ReadAllLines(file)[0].Split(';').Length;
                return fileCollumms == numColunas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool QuantidadeLinhasInvalida(string file, int numLinhas)
        {
            try
            {
                var linhas = File.ReadAllLines(file).Count();
                return linhas > numLinhas;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
