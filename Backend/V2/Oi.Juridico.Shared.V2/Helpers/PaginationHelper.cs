using System;
using System.Collections.Generic;
using System.Text;

namespace Oi.Juridico.Shared.V2.Helpers
{
    public static class PaginationHelper
    {
        public static int PagesToSkip(int quantidade, int total, int pagina)
        {
            var quantidadePaginas = Math.Ceiling((decimal)total / quantidade);

            var paginasParaPular = quantidadePaginas >= pagina ? pagina - 1 : (int)quantidadePaginas == 0 ? (int)quantidadePaginas : (int)quantidadePaginas - 1;

            return paginasParaPular * quantidade;
        }
    }
}
