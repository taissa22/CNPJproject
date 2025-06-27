using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oi.Juridico.WebApi.V2.Areas.Relatorios.ATM.DTOs
{
    public class ObterIndicesDoFechamentoResponse
    {
        public string CodEstado { get; set; }
        public decimal CodIndice { get; set; }
        public static List<ObterIndicesDoFechamentoResponse> GerarListaEstadosComIndiceZero()
        {
            List<ObterIndicesDoFechamentoResponse> lista = new List<ObterIndicesDoFechamentoResponse>();
            string[] estados = new string[]
            {
            "AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT",
            "MS", "MG", "PA", "PB", "PR", "PE", "PI", "RJ", "RN", "RS", "RO",
            "RR", "SC", "SP", "SE", "TO"
            };

            foreach (var estado in estados)
            {
                lista.Add(new ObterIndicesDoFechamentoResponse
                {
                    CodEstado = estado,
                    CodIndice = 0
                });
            }

            return lista;
        }
    }


  
}
