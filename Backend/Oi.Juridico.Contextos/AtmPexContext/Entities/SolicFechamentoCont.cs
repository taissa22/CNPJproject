﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Oi.Juridico.Contextos.AtmPexContext.Entities
{
    public class SolicFechamentoCont
    {
        public int CodSolicFechamentoCont { get; set; }
        public byte? CodTipoFechamento { get; set; }
        public byte? NumeroDeMeses { get; set; }
        public string IndExecucaoImediata { get; set; }
        public byte? MesContabil { get; set; }
        public byte? AnoContabil { get; set; }
        public DateTime? DataPrevia { get; set; }
        public string IndFechamentoMensal { get; set; }
        public byte? PeriodicidadeExecucao { get; set; }
        public DateTime? DataEspecifica { get; set; }
        public DateTime? DataDiariaIni { get; set; }
        public DateTime? DataDiariaFim { get; set; }
        public bool? DiaDaSemana { get; set; }
        public byte? DiaDoMes { get; set; }
        public string IndUltimoDiaDoMes { get; set; }
        public string IndSomenteDiaUtil { get; set; }
        public DateTime? DataCadastro { get; set; }
        public DateTime? DatUltimoAgend { get; set; }
        public DateTime? DatProximoAgend { get; set; }
        public DateTime? DatProximoFechamento { get; set; }
        public string IndAtivo { get; set; }
        public string CodUsuarioSolicitacao { get; set; }
        public string IndGerarBaseDadosFec { get; set; }
        public string CodTipoFechamentoTrab { get; set; }
        public decimal? ValPercentProcOutliers { get; set; }
        public decimal? ValAjusteDesvioPadrao { get; set; }
        public decimal? PercentualHaircut { get; set; }
        public decimal? MultDesvioPadrao { get; set; }
        public string IndAplicarHaircutProcGar { get; set; }
        public string ObsValCorteOutliers { get; set; }
        public decimal? ValCorteOutliers { get; set; }
    }
}
