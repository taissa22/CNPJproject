using Perlink.Oi.Juridico.Infra.Seedwork;
using System;
using System.IO;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Handlers.Implementations
{
    public class CsvHandler : ICsvHandler
    {
        public CommandResult<bool> ArquivoVazio(string file)
        {
            try
            {
                return CommandResult<bool>.Valid(File.ReadAllLines(file).Count() < 2);
            }
            catch (Exception ex)
            {
                return CommandResult<bool>.Invalid(ex.Message);
            }
        }

        public CommandResult<bool> LayoutValido(string file, int colummNumber)
        {
            try
            {
                var fileCollumms = File.ReadAllLines(file)[0].Split(';').Length;
                return CommandResult<bool>.Valid(fileCollumms == colummNumber);
            }
            catch (Exception ex)
            {
                return CommandResult<bool>.Invalid(ex.Message);
            }
        }
    }
}