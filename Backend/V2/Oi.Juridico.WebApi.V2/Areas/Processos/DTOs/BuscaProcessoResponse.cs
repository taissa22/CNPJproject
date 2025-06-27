using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Processos.DTOs
{
    public class BuscaProcessoResponse
    {
        public int id { get; set; } // IdEscritório
        public string nome { get; set; } // Nome escritório
        public List<ProcessoResponse> processos { get; set; }
    }
    public class ProcessoResponse
    {
        public long id { get; set; }
        public string nro_processo { get; set; } = "";          
        public string procedimento { get; set; } = "";
        public string orgao { get; set; } = "";
        public string competencia { get; set; } = "";
        public string estado { get; set; } = "";
        public string municipio { get; set; } = "";
        public List<string> assunto { get; set; } = new();
        public string criticidade { get; set; } = "";
        public string status { get; set; } = "";
        public string instauracao { get; set; } = "";

        //Judicial
        public string acao { get; set; } = "";
        public string comarca { get; set; } = "";
        public string vara { get; set; } = "";
        public string nro_processo_antigo { get; set; } = "";

        //UltimosAcessos
        public string data_ultimo_acesso { get; set; } = "";

        public Dictionary<string, List<string>> partes { get; set; } = new();
    }
}
