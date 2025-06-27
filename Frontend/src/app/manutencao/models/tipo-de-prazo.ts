export interface ITipoDePrazo {
    codigo: number;
    descricao: string;
    tipoDeProcesso: {id: number, nome: string};
    ativo: boolean;
    prazoServico: boolean;
    prazoDocumento:boolean;
    idMigracao: number;
    ativoDePara: boolean
    descricaoMigracao: string;
  }

  export class TipoDePrazo implements ITipoDePrazo {
    private constructor(
      codigo: number,
      descricao: string,
      tipoDeProcesso: {id: number, nome: string},
      ativo: boolean,
      prazoServico: boolean,
      prazoDocumento:boolean,
      idMigracao?: number,
      ativoDePara?: boolean,
      descricaoMigracao?: string
    ) {
      this.codigo = codigo;
      this.descricao = descricao;
      this.tipoDeProcesso = tipoDeProcesso;
      this.ativo = ativo;
      this.prazoServico = prazoServico;
      this.prazoDocumento = prazoDocumento;
      this.idMigracao = idMigracao;
      this.ativoDePara = ativoDePara;
      this.descricaoMigracao = descricaoMigracao;
    }

    readonly codigo: number;
    readonly descricao: string;
    readonly tipoDeProcesso: {id: number, nome: string};
    readonly ativo: boolean;
    readonly prazoServico: boolean;
    readonly prazoDocumento: boolean;
    readonly idMigracao: number;
    readonly ativoDePara: boolean;
    readonly descricaoMigracao: string;

    static fromObj(obj: any): TipoDePrazo {
      return ({
        codigo: obj.id,
        descricao: obj.descricao,
        tipoDeProcesso: obj.tipoProcesso,
        ativo: obj.ativo,
        prazoServico: obj.eh_Servico,
        prazoDocumento: obj.eh_Documento,
        idMigracao: obj.idMigracao,
        ativoDePara: obj.ativoDePara,
        descricaoMigracao: obj.descricaoMigracao
      });
    }
  }
