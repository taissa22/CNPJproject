export interface IPedidosResultado{
  pedidos: Array<IPedidos>
}



export interface IPedidos {
  codigoProcesso: number;
  codigoPedido: number;
  descricaoPedido: string;
  codigoRiscoPerda: string;
  numeroContrato: string;
}
