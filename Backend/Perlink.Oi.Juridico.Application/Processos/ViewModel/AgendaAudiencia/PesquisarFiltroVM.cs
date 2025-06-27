using DocumentFormat.OpenXml.Spreadsheet;
using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Processos.ViewModel.AgendaAudiencia
{
    public class PesquisarFiltroVM
    {
        public IEnumerable<FilterVM> Filters { get; set; }

        public IEnumerable<SortOrderVM> SortOrders { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool IsExportMethod { get; set; }
    }
}

