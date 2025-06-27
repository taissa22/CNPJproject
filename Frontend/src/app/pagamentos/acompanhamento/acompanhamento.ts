export interface CargaCompromissoParcela {
    id: number;
    idCompromisso: number;
    seqLancamento: number | null;
    nroParcela: number;
    valor: number;
    vencimento: string; // Use string para a data, ou você pode usar Date se quiser manipulá-las como objetos de data
    status: number;
    motivoExclusao: string | null;
    numeroPedidoSAP : number;
  }

  export interface ClasseCredito{
    id : number,
    descricao : string
  }
  
  export interface CargaCompromisso {
    id: number;
    codAgendCargaComp: number;
    codProcesso: string;
    codTipoProcesso: number;
    codCatPagamento: number;
    docAutor: string;
    qtdParcelas: number;
    motivoExclusao: string | null;
    nomeBeneficiario: string;
    dataPrimeiraParcela: string;
    nroGuia: number;
    codBancoArrecadador: number;
    codFornecedor: number;
    codFormaPgto: number;
    codCentroCusto: number;
    comentarioLancamento: string;
    comentarioSap: string;
    borderoBeneficiario: string | null;
    borderoDoc: string | null;
    borderoBanco: string | null;
    borderoBancoDv: string | null;
    borderoAgencia: string | null;
    borderoAgenciaDv: string | null;
    borderoCc: string | null;
    borderoCcDv: string | null;
    borderoValor: number | null;
    borderoCidade: string | null;
    borderoHistorico: string | null;
    valorTotal: number;
    codigoCredor: string | null;
    column1: string | null;
    docCredor: string | null;
    classeCredito : string | null;
    cargaCompromissoParcela: CargaCompromissoParcela[]; 
  }
  