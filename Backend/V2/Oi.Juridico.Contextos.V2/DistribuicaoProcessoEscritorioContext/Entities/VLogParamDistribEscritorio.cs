﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.DistribuicaoProcessoEscritorioContext.Entities
{
    public partial class VLogParamDistribEscritorio
    {
        public DateTime DatLog { get; set; }
        public string Operacao { get; set; }
        public decimal? CodParamDistribuicao { get; set; }
        public string CodUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public int? CodProfissionalA { get; set; }
        public int? CodProfissionalD { get; set; }
        public string NomProfissionalA { get; set; }
        public string NomProfissionalD { get; set; }
        public string SolicitanteA { get; set; }
        public string SolicitanteD { get; set; }
        public DateTime? DatVigenciaInicialA { get; set; }
        public DateTime? DatVigenciaInicialD { get; set; }
        public DateTime? DatVigenciaFinalA { get; set; }
        public DateTime? DatVigenciaFinalD { get; set; }
        public decimal? PorcentagemProcessosA { get; set; }
        public decimal? PorcentagemProcessosD { get; set; }
        public short? PrioridadeA { get; set; }
        public short? PrioridadeD { get; set; }
    }
}