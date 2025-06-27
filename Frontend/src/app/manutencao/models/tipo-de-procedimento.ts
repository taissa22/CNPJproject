export class TipoDeProcedimento {
    private constructor(codigo: number, descricao: string, indAtivo: boolean, tipoParticipacao1: {codigo: number, descricao: string}, indOrgao1: boolean, 
      tipoParticipacao2: {codigo: number, descricao: string}, indOrgao2: boolean, tipoDeProcesso: {id: number, nome: string}, 
      indAdministrativo: boolean,  indCivelAdministrativo: boolean, indCriminalAdministrativo: boolean, indTributario: boolean, 
      indTrabalhistaAdm: boolean, indPoloPassivoUnico: boolean, indProvisionado: boolean) {
      this.codigo = codigo;
      this.descricao = descricao;
      this.indAtivo = indAtivo;
      this.tipoParticipacao1 = tipoParticipacao1;
      this.indOrgao1 = indOrgao1;
      this.tipoParticipacao2 = tipoParticipacao2;
      this.indOrgao2 = indOrgao2;
      this.tipoDeProcesso = tipoDeProcesso;
      this.indAdministrativo = indAdministrativo;
      this.indCivelAdministrativo = indCivelAdministrativo;
      this.indCriminalAdministrativo = indCriminalAdministrativo;
      this.indTributario = indTributario;
      this.indTrabalhistaAdm = indTrabalhistaAdm;
      this.indPoloPassivoUnico = indPoloPassivoUnico;
      this.indProvisionado = indProvisionado;
    }
  
  readonly codigo: number;
  readonly descricao: string;
  readonly indAtivo: boolean;
  readonly tipoParticipacao1: {codigo: number, descricao: string};
  readonly indOrgao1: boolean;
  readonly tipoParticipacao2: {codigo: number, descricao: string};
  readonly indOrgao2: boolean;
  readonly tipoDeProcesso: {id: number, nome: string};
  readonly indAdministrativo: boolean;
  readonly indCivelAdministrativo: boolean;
  readonly indCriminalAdministrativo: boolean;
  readonly indTributario: boolean;
  readonly indTrabalhistaAdm: boolean;
  readonly indPoloPassivoUnico: boolean;
  readonly indProvisionado: boolean;
  
    static fromObj(obj: TipoProcedimentoBack): TipoDeProcedimento {
      return new TipoDeProcedimento(obj.codigo, obj.descricao, obj.indAtivo, obj.tipoDeParticipacao1, 
        obj.indOrgao1, obj.tipoDeParticipacao2, obj.indOrgao2, obj.tipoProcesso, 
        obj.indAdministrativo, obj.indCivelAdministrativo, obj.indCriminalAdministrativo, obj.indTributario, obj.indTrabalhistaAdm, obj.indPoloPassivoUnico, obj.indProvisionado);
    }
}
export class TipoProcedimentoBack {
  readonly codigo: number;
  readonly descricao: string;
  readonly indAtivo: boolean;
  readonly tipoDeParticipacao1: {codigo: number, descricao: string};
  readonly indOrgao1: boolean;
  readonly tipoDeParticipacao2: {codigo: number, descricao: string};
  readonly indOrgao2: boolean;
  readonly tipoProcesso: {id: number, nome: string};
  readonly indAdministrativo: boolean;
  readonly indCivelAdministrativo: boolean;
  readonly indCriminalAdministrativo: boolean;
  readonly indTributario: boolean;
  readonly indTrabalhistaAdm: boolean;
  readonly indPoloPassivoUnico: boolean;
  readonly indProvisionado: boolean;
}


  
