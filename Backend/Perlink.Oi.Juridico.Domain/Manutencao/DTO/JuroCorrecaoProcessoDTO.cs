using System;

namespace Perlink.Oi.Juridico.Domain.Manutencao.JurosCorrecaoProcesso.DTO
{
    public class JuroCorrecaoProcessoDTO
    {
        public long CodTipoProcesso { get; set; }

        public string NomTipoProcesso { get; set; }

        public DateTime DataVigencia { get; set; }

        public double? ValorJuros { get; set; }
    }
}
