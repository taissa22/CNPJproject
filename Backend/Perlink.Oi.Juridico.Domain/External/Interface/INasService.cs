using System.Collections.Generic;
using System.Threading.Tasks;

namespace Perlink.Oi.Juridico.Domain.External.Interface
{
    public interface INasService
    {
        void SaveFileFromTempDir(string fromFile, string toPath);
        Queue<string> TratarCaminhoDinamicoNoParametroJuridico(string path);
        
        Task<byte[]> GetFileFromNAs(string fileName, string parameterName);
        string GetFilePathFromNAs(string fileName, string parameterName);

    }
}
