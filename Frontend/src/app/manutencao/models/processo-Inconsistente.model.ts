export class ProcessoInconsistente {
    private constructor(tipoProcesso: string, codigoProcesso: string, numeroProcesso: string, estado : string, comarcaOrgao : string, 
      varaMunicipio : string,empresadoGrupo : string,escritorio : string, objeto : string, periodo : Date,valorTotalCorrigido : number,valorTotalPago : number ) {
      this.tipoProcesso = tipoProcesso;
      this.codigoProcesso = codigoProcesso;
      this.numeroProcesso = numeroProcesso;  
      this.estado = estado;
      this.comarcaOrgao = comarcaOrgao;
      this.varaMunicipio = varaMunicipio;
      this.empresadoGrupo = empresadoGrupo;
      this.escritorio = escritorio;
      this.objeto = objeto;      
      this.periodo = periodo;
      this.valorTotalCorrigido = valorTotalCorrigido;
      this.valorTotalPago = valorTotalPago;        
    }  

    readonly tipoProcesso : string;
    readonly codigoProcesso :string;
    readonly numeroProcesso : string;
    readonly estado : string;
    readonly comarcaOrgao : string;
    readonly varaMunicipio : string;
    readonly empresadoGrupo : string;
    readonly escritorio : string;
    readonly objeto : string;    
    readonly periodo : Date;
    readonly valorTotalCorrigido : number;
    readonly valorTotalPago : number;    
  
    static fromObj(obj: any): ProcessoInconsistente {
      return new ProcessoInconsistente(obj.tipoProcesso, obj.codigoProcesso, obj.numeroProcesso,obj.estado, obj.comarcaOrgao ,obj.varaMunicipio ,obj.empresadoGrupo ,
                                       obj.escritorio, obj.objeto, obj.periodo ,obj.valorTotalCorrigido ,obj.valorTotalPago);
    }
}


  
