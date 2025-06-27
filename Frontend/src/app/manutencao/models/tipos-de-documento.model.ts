export class TipoDeDocumento {
    private constructor(codigo: number, descricao: string, ativo: boolean, codTipoProcesso: number, tipoDeProcesso: {id: number, nome: string}, cadastraProcesso: boolean, requerDataAudienciaPrazo: boolean, prioritarioFilaCadastroProcesso: boolean, utilizadoEmProtocolo: boolean, codTipoPrazo: number, tipoDePrazo: {id: number, descricao: string}, indDocumentoApuracao : boolean, indEnviarAppPreposto : boolean, descricaoMigracao :string, idMigracao?:number, ativoDePara? : boolean) {
      this.codigo = codigo;
      this.idMigracao = idMigracao;
      this.descricao = descricao;
      this.ativo = ativo;
      this.codTipoProcesso = codTipoProcesso;
      this.tipoDeProcesso = tipoDeProcesso;
      this.cadastraProcesso = cadastraProcesso;
      this.requerDataAudienciaPrazo = requerDataAudienciaPrazo;
      this.prioritarioFilaCadastroProcesso = prioritarioFilaCadastroProcesso;
      this.utilizadoEmProtocolo = utilizadoEmProtocolo;
      this.codTipoPrazo = codTipoPrazo;
      this.tipoDePrazo = tipoDePrazo;
      this.indDocumentoApuracao = indDocumentoApuracao;
      this.indEnviarAppPreposto = indEnviarAppPreposto;
      this.descricaoMigracao = descricaoMigracao;
      this.ativoDePara = ativoDePara;
    }
  
    readonly codigo: number;
    readonly idMigracao?: number;
    readonly descricao: string;
    readonly ativo: boolean;
    readonly codTipoProcesso: number;
    readonly tipoDeProcesso: {id: number, nome: string};
    readonly cadastraProcesso: boolean;
    readonly requerDataAudienciaPrazo: boolean;
    readonly prioritarioFilaCadastroProcesso: boolean;
    readonly utilizadoEmProtocolo: boolean;
    readonly codTipoPrazo: number;
    readonly tipoDePrazo: {id: number, descricao: string};
    readonly indDocumentoApuracao: boolean; 
    readonly indEnviarAppPreposto: boolean; 
    readonly descricaoMigracao: string;
    readonly ativoDePara?: boolean; 

  
    static fromObj(obj: TipoDocumentoBack): TipoDeDocumento {
      return new TipoDeDocumento(obj.id, obj.descricao, obj.ativo, obj.codTipoProcesso, obj.tipoProcesso, obj.marcadoCriacaoProcesso, obj.indRequerDatAudiencia, obj.indPrioritarioFila, obj.indDocumentoProtocolo, obj.codTipoPrazo, obj.tipoPrazo,obj.indDocumentoApuracao,obj.indEnviarAppPreposto, obj.descricaoMigracao,obj.idMigracao, obj.ativoDePara);
    }
}
export class TipoDocumentoBack {
  id: number;
  descricao: string;
  ativo: boolean;
  codTipoProcesso: number;
  tipoProcesso: {id: number, nome: string};
  marcadoCriacaoProcesso: boolean;
  indRequerDatAudiencia: boolean;
  indPrioritarioFila: boolean;
  indDocumentoProtocolo: boolean;
  codTipoPrazo: number;
  tipoPrazo: {id: number, descricao: string};
  indDocumentoApuracao: boolean; 
  indEnviarAppPreposto: boolean;  
  descricaoMigracao: string;
  idMigracao?: number;
  ativoDePara?: boolean;
}

  
