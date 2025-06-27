using Perlink.Oi.Juridico.Infra.Seedwork;
using Perlink.Oi.Juridico.Infra.Seedwork.Notifying;
using System;
using System.Linq;

namespace Perlink.Oi.Juridico.Infra.Entities
{
    public sealed class ProcessoTributarioInconsistente : Notifiable, IEntity, INotifiable
    {
        public ProcessoTributarioInconsistente(string? tipoProcesso,
                                     int? codigoProcesso,
                                     string? numeroProcesso,
                                     string? estado,
                                     string? comarcaOrgao,
                                     string? varaMunicipio,
                                     string? empresadoGrupo,
                                     string? escritorio,
                                     string? objeto,
                                     DateTime? periodo,
                                     decimal? valorTotalCorrigido,
                                     decimal? valorTotalPago)
        {
            TipoProcesso = tipoProcesso;
            CodigoProcesso = codigoProcesso;
            NumeroProcesso = numeroProcesso;
            Estado = estado;
            ComarcaOrgao = comarcaOrgao;
            VaraMunicipio = varaMunicipio;
            EmpresadoGrupo = empresadoGrupo;
            Escritorio = escritorio;
            Objeto = objeto;
            Periodo = periodo;
            ValorTotalCorrigido = valorTotalCorrigido;
            ValorTotalPago = valorTotalPago;
        }

        public string? TipoProcesso { get; private set; }
        public int? CodigoProcesso { get; private set; }
        public string? NumeroProcesso { get; private set; }
        public string? Estado { get; private set; }
        public string? ComarcaOrgao { get; private set; }
        public string? VaraMunicipio { get; private set; }
        public string? EmpresadoGrupo { get; private set; }
        public string? Escritorio { get; private set; }
        public string? Objeto { get; private set; }
        public DateTime? Periodo { get; private set; }
        public decimal? ValorTotalCorrigido { get; private set; }
        public decimal? ValorTotalPago { get; private set; }
    }
}
