export interface IResultado {
    id: number;
    descricaoLote: string;
    formaPagamento: string;
    nomeEmpresaGrupo: string;
    nomeUsuario: string;
    dataCriacao: string;
    codigoStatusPagamento: number;
    statusPagamento: string;
    dataCriacaoPedido: string;
    numeroPedidoSAP: number;
  existeBordero: boolean;
  numeroLoteBB: string;
}
