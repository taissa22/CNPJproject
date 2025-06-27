using Perlink.Oi.Juridico.Application.Compartilhado.ViewModel.General;
using Shared.Domain.Commands;
using System.Collections.Generic;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposAudiencias
{
    public class PesquisarTipoAudienciaCommand : ICommand
    {
        public PesquisarTipoAudienciaCommand() { }

        public PesquisarTipoAudienciaCommand(long? codTipoProcesso, string descricao, bool isExportMethod) {
            CodTipoProcesso = codTipoProcesso;
            Descricao = descricao;
            IsExportMethod = isExportMethod;
        }

        public long? CodTipoProcesso { get; set; }

        public string Descricao { get; set; }

        public IEnumerable<SortOrderVM> SortOrders { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public bool IsExportMethod { get; set; }
    }
}
