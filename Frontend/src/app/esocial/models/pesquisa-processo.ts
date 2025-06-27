export class PesquisaProcesso {
    private constructor(codProcesso: number, nroProcessoCartorio : string, nomeComarca : string, ufVara : string, nomeVara : string, indAtivo : string, nomeEmpresaGrupo : string, indProprioTerceiro : string) {
      this.codProcesso = codProcesso;
      this.nroProcessoCartorio = nroProcessoCartorio;
      this.nomeComarca = nomeComarca;
      this.ufVara = ufVara;
      this.nomeVara = nomeVara;
      this.indAtivo = indAtivo;
      this.nomeEmpresaGrupo = nomeEmpresaGrupo;
      this.indProprioTerceiro = indProprioTerceiro;
    }
  
    readonly codProcesso: number;
    readonly nroProcessoCartorio: string;
    readonly nomeComarca: string;
    readonly ufVara : string;
    readonly nomeVara : string;
    readonly indAtivo : string;
    readonly nomeEmpresaGrupo : string;
    readonly indProprioTerceiro : string;
  
    static fromObj(obj: any): PesquisaProcesso {
      return new PesquisaProcesso(obj.codProcesso, obj.nroProcessoCartorio, obj.nomeComarca, obj.ufVara, obj.nomeVara , obj.indAtivo, obj.nomeEmpresaGrupo, obj.indProprioTerceiro);
    }
  }