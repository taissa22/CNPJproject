using System;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class ArquivoBBDTO
    {
        #region propriedades para o endpoint
        public long CodigoLote { get; set; }
        public long NumeroLoteBB { get; set; }
        public bool EnviarServidor { get; set; }
        public DateTime? DataGuia { get; set; }
        #endregion propriedades para o endpoint

        #region propriedades do arquivo
        public string NomeArquivo { get; set; }
        public StringBuilder ConteudoArquivo { get; set; }
        public string CodigoEstado { get; set; }
        public string NumeroAgenciaDepositaria { get; set; }
        #endregion propriedades do arquivo
    }
}
