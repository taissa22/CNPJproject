using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposAudiencias
{
    public class ExportarTipoAudienciaCommand : ICommand
    {
        public ExportarTipoAudienciaCommand() { }

        public ExportarTipoAudienciaCommand(long? codTipoProcesso, string descricao, bool isExportMethod)
        {
            CodTipoProcesso = codTipoProcesso;
            Descricao = descricao;
            IsExportMethod = isExportMethod;
        }

        [FromQuery(Name = "codigotipoprocesso")]
        public long? CodTipoProcesso { get; set; }

        [FromQuery(Name = "descricao")]
        public string Descricao { get; set; }

        [FromQuery(Name = "propriedade")]
        public string Propriedade { get; set; }

        [FromQuery(Name = "direcao")]
        public string Direcao{ get; set; }

        public bool IsExportMethod { get; set; }
    }
}
