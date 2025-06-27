using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.Contextos.V2.Extensions
{
    public class PaginatedQueryResult<T>
    {
        public IReadOnlyCollection<T> Data { get; set; }
        public int Total { get; set; }
    }

    public class Pagination
    {
        public static int PagesToSkip(int quantidade, int total, int pagina)
        {
            var quantidadePaginas = Math.Ceiling((decimal)total / quantidade);

            var paginasParaPular = quantidadePaginas >= pagina ? pagina -1: (int)quantidadePaginas == 0 ? (int)quantidadePaginas : (int)quantidadePaginas - 1;

            return paginasParaPular * quantidade;
        }
    }
}
