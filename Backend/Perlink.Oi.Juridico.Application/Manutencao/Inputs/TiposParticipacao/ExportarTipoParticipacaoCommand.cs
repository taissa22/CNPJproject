using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposParticipacao
{
    public class ExportarTipoParticipacaoCommand : ICommand
    {
        [FromQuery(Name = "descricao")]
        public string Descricao { get; set; }

        [FromQuery(Name = "propriedade")]
        public string Propriedade { get; set; }

        [FromQuery(Name = "direcao")]
        public string Direcao { get; set; }

        [FromQuery(Name = "isexport")]
        public bool IsExportMethod { get; set; }
    }
}
