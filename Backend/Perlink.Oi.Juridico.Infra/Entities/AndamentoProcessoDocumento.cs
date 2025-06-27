using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;

namespace Perlink.Oi.Juridico.Infra.Entities
{
#nullable enable
    public sealed class AndamentoProcessoDocumento : Notifiable, IEntity, INotifiable
	{
        public int Id { get; set; }
        public int CodProcesso { get; set; }
        public byte SequencialAndamento { get; set; }
        public DateTime DataVinculo { get; set; }
        public string? CodDocumentoDigitalizado { get; set; }
        public string? NomeArquivo { get; set; }
        public byte? CodTipoDocumento { get; set; }
        public string? Comentario { get; set; }
        public int? NumPaginaInicial { get; set; }
        public int? NumPaginaFinal { get; set; }
        public int? IdAutoDocumentoGed { get; set; }
       
    }
}
