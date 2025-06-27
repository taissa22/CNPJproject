using DocumentFormat.OpenXml.Office2010.Excel;
using Oi.Juridico.WebApi.V2.DTOs.Contingencia.Fechamento;
using Perlink.Oi.Juridico.Infra.Entities;

namespace Oi.Juridico.WebApi.V2.Areas.FechamentoContingencia.Models
{
    public class LogProcessamentoContingenciaModel
    {
        public int Id { get; set; }
        public string TipoFechamento { get; set; } = string.Empty;
        public string EmpresaCentralizadora  { get; set; } = string.Empty;
        public string Parametros { get; set; } = string.Empty;
        public string FechamentoMes  { get; set; } = string.Empty;
        public string AgendadoPara { get; set; } = string.Empty;
        public string Execucao { get; set; } = string.Empty;
        public string Situacao { get; set; } = string.Empty;
        public DateTime? Data { get; internal set; }
        public string MensagemErro { get; internal set; }
    }
}
