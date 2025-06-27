using System;
using CsvHelper.Configuration.Attributes;

namespace Perlink.Oi.Juridico.Domain.Compartilhado.DTO
{
    public class ProcessoExportacaoPrePosRJDTO
    {
        [Name("Código Interno")]
        [Index(0)]
        public long Id { get; set; }

        [Name("Número do Processo")]
        [Index(5)]
        public string NumeroProcessoCartorio { get; set; }

        [Name("Data Finalização")]
        [Index(3)]
        public DateTime? DataFinalizacao { get; set; }

        [Name("Data Finalização Contábil")]
        [Index(2)]
        public DateTime? DataFinalizacaoContabil { get; set; }

        [Name("Pré-Pós RJ")]
        [Index(1)]
        public string PrePos { get; set; }

        [Name("Influencia na Contingência")]
        [Index(4)]
        public string ConsiderarProvisao { get; set; }
    }
}

