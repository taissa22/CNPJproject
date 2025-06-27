using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.ContingenciaCCPorMedia.DTO
{
    public class FechamentoContingenciaCCPorMediaDTO
    {
        public long Id { get; set; }
        //public DateTime DataFechamento { get; set; }
        //public string NomeUsuario { get; set; }
        //public long NumeroMeses { get; set; }
        //public decimal? PercentualHaircut { get; set; }
        //public decimal? MultDesvioPadrao { get; set; }
        //public string IndAplicarHaircut { get; set; }
        //public DateTime DataExecucao { get; set; }
        //public string Empresas { get; set; }


        public long? CodSolicFechamentoCont { get; set; }
        public long? CodEmpresaCentralizadora { get; set; }
        public DateTime DataFechamento { get; set; }
        public DateTime DataGeracao { get; set; }
        public DateTime? DataIndMensal { get; set; }
        //public EmpresasCentralizadoras CodEmpresaCentralizadora { get; set; }
        public string IndBaseGerada { get; set; }
        public string IndMensal { get; set; }
        public DateTime? MesAnoFechamento { get; set; }
        public long NroMesesMediaHistorica { get; set; }
        public decimal? PerHaircut { get; set; }
        public string CodUsuario { get; set; }
        public decimal ValorCorte { get; set; }


    }
}
