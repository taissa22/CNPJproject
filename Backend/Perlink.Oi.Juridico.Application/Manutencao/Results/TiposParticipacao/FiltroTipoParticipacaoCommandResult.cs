using System.ComponentModel.DataAnnotations;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposParticipacao
{
    public class FiltroTipoParticipacaoCommandResult
    {
        [Display(Name = "Código")]
        public long CodTipoParticipacao { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
    }
}
