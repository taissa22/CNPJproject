using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Domain.SAP.DTO
{
    public class EmpresaSapDTO
    {
        public long Id { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public bool IndicaEnvioArquivoSolicitacao { get; set; }
        public bool IndicaAtivo { get; set; }
        public string CodigoOrganizacaocompra { get; set; }
    }
}
