using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Perlink.Oi.Juridico.WebApi.Helpers
{
    /// <summary>
    /// Classe base usada para passar comportamentos comuns a todas as subclasses controladoras
    /// </summary>
    [Authorize]
    public abstract class JuridicoControllerBase : ControllerBase
    {
        /// <summary>
        /// Método para tentar recuperar o tipo Mime do arquivo. Caso não encontre, retorna o tipo padrão.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        protected string GetContentType(string file)
        {
            var provider = new FileExtensionContentTypeProvider();            
            if (!provider.TryGetContentType(file, out string contentType))
                contentType = "application/octet-stream";
            return contentType;
        }
    }
}
