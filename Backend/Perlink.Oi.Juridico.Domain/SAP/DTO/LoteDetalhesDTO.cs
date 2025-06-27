using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class LoteDetalhesDTO
    {

        public long Id { get; set; }
        public string NomeEmpresaGrupo { get; set; }
        public string Fornecedor { get; set; }
        public long QuantLancamento { get; set; }
        public string DataEnvioEscritorio { get; set; }
        public string FormaPagamento { get; set; }
        public long? NumeroLoteBB { get; set; }
        public string CentroCusto { get; set; }
        public double Valor { get; set; }
        public bool exibirLoteBB = true;
        public string DataRetornoBB { get; set; }
        public string DataGeracaoArquivoBB { get; set; }
    }
}
