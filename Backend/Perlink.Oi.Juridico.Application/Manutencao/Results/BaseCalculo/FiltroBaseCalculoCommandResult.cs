using System.ComponentModel.DataAnnotations;

namespace Perlink.Oi.Juridico.Application.Manutencao.Result.BaseCalculos
{
    public class FiltroBaseCalculoCommandResult
    {
        [Display(Name = "Código")]
        public long CodBaseCalculo { get; set; }

        [Display(Name = "Descrição da Base de Cálculo")]
        public string Descricao { get; set; }

        [Display(Name = "Cálculo Inicial")]
        public string EhCalculoInicial { get; set; }
    }
}
