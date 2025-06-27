using System;
using System.Collections.Generic;
using System.Text;

namespace Perlink.Oi.Juridico.Infra.Constants
{
    public class OcorrenciasMigracaoPedidosSAP
    {
        public const string NUMERO_PEDIDO_ANTES_NAO_PREENCHIDO = "Número pedido anterior é de preenchimento obrigatorio.";
        public const string NUMERO_PEDIDO_DEPOIS_NAO_PREENCHIDO = "Número pedido novo é de preenchimento obrigatorio.";
        public const string LANCAMENTO_NAO_ENCONTRADO = "Nenhum lançamento encontrado para o número pedido anterior informado.";
        public const string LOTE_NAO_ENCONTRADO = "Nenhum lote encontrado para o número pedido anterior informado.";
        public const string LANCAMENTO_NUMERO_PEDIDO_DEPOIS_ENCONTRADO = "Lançamento encontrado para o número pedido novo informado.";
        public const string LOTE_NUMERO_PEDIDO_DEPOIS_ENCONTRADO = "Lote encontrado para o número pedido novo informado.";
        public const string NUMERO_PEDIDO_ANTES_FORMATO_INVALIDO = "O número do pedido anterior precisa ser númerico com até 10 dígitos.";
        public const string NUMERO_PEDIDO_DEPOIS_FORMATO_INVALIDO = "O número do pedido novo precisa ser númerico com até 10 dígitos.";
        public const string LANCAMENTO_E_LOTE_NAO_ENCONTRADOS = "Nenhum lançamento ou lote encontrados para o número de pedido anterior {0}.";

    }
}
