using System.ComponentModel.DataAnnotations;

namespace Perlink.Oi.Juridico.Application.Manutencao.Results.TiposAudiencias
{
    public class FiltroTipoAudienciaCommandResult
    {
        [Display(Name = "Código")]
        public long CodTipoAudiencia { get; set; }

        [Display(Name = "Sigla")]
        public string Sigla { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Tipo de Processo")]
        public string TipoDeProcesso { get; set; }

        [Display(Name = "Ativo")]
        public string Ativo { get; set; }
    }
}
