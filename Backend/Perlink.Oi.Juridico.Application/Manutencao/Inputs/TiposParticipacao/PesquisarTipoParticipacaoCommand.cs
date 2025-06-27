using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Commands;

namespace Perlink.Oi.Juridico.Application.Manutencao.Inputs.TiposParticipacao
{
    public class PesquisarTipoParticipacaoCommand : ICommand
    {
        [FromQuery(Name = "descricao")]
        public string Descricao { get; set; }

        [FromQuery(Name = "propriedade")]
        public string Propriedade { get; set; }

        // ASC ou DESC
        [FromQuery(Name = "direcao")]
        public string Direcao { get; set; }

        [FromQuery(Name = "pagenumber")]
        public int PageNumber { get; set; }

        [FromQuery(Name = "pagesize")]
        public int PageSize { get; set; }

        [FromQuery(Name = "isexport")]
        public bool IsExportMethod { get; set; }
    }
}
