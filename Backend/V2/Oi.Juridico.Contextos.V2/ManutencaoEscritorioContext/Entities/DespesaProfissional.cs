﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Oi.Juridico.Contextos.V2.ManutencaoEscritorioContext.Entities
{
    public partial class DespesaProfissional
    {
        public int CodProfissional { get; set; }
        public int CodSeqDespesa { get; set; }
        public int? CodPedidoSap { get; set; }
        public short? NroItemPedidoSap { get; set; }
        public decimal ValAtribuido { get; set; }
        public DateTime? DatCompensacao { get; set; }
        public decimal? ValPedido { get; set; }
        public string NomFornecedor { get; set; }
        public string DscMaterial { get; set; }
        public int? CentroCusto { get; set; }
        public string NroDocumento { get; set; }

        public virtual Profissional CodProfissionalNavigation { get; set; }
    }
}