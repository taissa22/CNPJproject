using System;

namespace Perlink.Oi.Juridico.Infra.Dto
{
    public class AlocacoesFuturas
    {
        public AlocacoesFuturas(string tipo,
                                DateTime data,
                                string uF,
                                string comarca,
                                int? varaId,
                                string varaNome ,
                                string numeroProcesso ,
                                string tipoProcesso ,
                                int? codProcessoInterno)
        {
            Tipo = tipo;
            Data = data;
            UF = uF;
            Comarca = comarca;
            VaraId = varaId;
            VaraNome = varaNome;
            NumeroProcesso = numeroProcesso;
            TipoProcesso = tipoProcesso;
            CodProcessoInterno = codProcessoInterno;
        }

        public string Tipo { get; set; }
        public DateTime? Data { get; set; }
        public string UF { get; set; }
        public string Comarca { get; set; }
        public int? VaraId { get; set; }
        public string VaraNome { get; set; }
        public string NumeroProcesso { get; set; }
        public string TipoProcesso { get; set; }
        public int? CodProcessoInterno { get; set; }
    }
}