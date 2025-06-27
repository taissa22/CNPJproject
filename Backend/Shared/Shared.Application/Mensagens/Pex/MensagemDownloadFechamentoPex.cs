using System;

namespace Shared.Application.Mensagens.Pex
{
    public class MensagemDownloadFechamentoPex
    {
        public string CodigoUsuario { get; set; }
        public string Email { get; set; }
        public long CodigoSolicitacaoFechamento { get; set; }
        public DateTime DataFechamento { get; set; }
    }
}
