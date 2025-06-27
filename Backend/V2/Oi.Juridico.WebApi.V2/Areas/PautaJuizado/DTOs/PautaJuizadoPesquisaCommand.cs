using System;

namespace Oi.Juridico.WebApi.V2.Areas.PautaJuizado.DTOs
{
    public class PautaJuizadoPesquisaCommand
    {
        public string PeriodoInicio { get; set; }

        public string PeriodoFim { get; set; }

        public string TipoAudiencia { get; set; }

        public string EmpresaDoGrupo { get; set; }

        public string Estado { get; set; }

        public string Comarca { get; set; }

        public string Juizado { get; set; }

        public bool SituacaoProcesso { get; set; }

        public string AudienciaSemPreposto { get; set; }

        public string EmpresaCentralizadora { get; set; }

        public string GrupoJuizado { get; set; }

        public bool SepararPautaPorEmpresaCheck { get; set; }

        public string StatusDeAudiencia { get; set; }

        public List<string> Preposto { get; set; }
    }
}
