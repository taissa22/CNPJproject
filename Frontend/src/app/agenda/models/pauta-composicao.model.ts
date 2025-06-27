export class PautaComposicao {
  private constructor(
    hora: string,
    tipo: string,
    juizado: string,
    processoEmpresaGrupo: string,
    assunto: string,
    pedido: string,    
    valorCausa: string,
    terceirizado: string,
    codEstado: string,
    nomComarca: string,
    codVara: number,
    nomTipoVara: string,
    nroProcessoCartorio: string,
    nomParte: string,
    codProcesso: string,
    codParte: string
) {
  this.hora = hora;
  this.tipo = tipo;
  this.juizado = juizado;
  this.processoEmpresaGrupo = processoEmpresaGrupo;
  this.assunto = assunto;
  this.pedido = pedido;
  this.valorCausa = valorCausa;
  this.terceirizado = terceirizado; 
  this.codEstado = codEstado;
  this.nomComarca = nomComarca;
  this.codVara = codVara;
  this.nomTipoVara = nomTipoVara;
  this.nroProcessoCartorio = nroProcessoCartorio;
  this.nomParte = nomParte; 
  this.codProcesso = codProcesso;
  this.codParte = codParte;
}

  readonly hora: string;
  readonly tipo: string;
  readonly juizado: string;
  readonly processoEmpresaGrupo: string;
  readonly assunto: string;
  readonly pedido: string;
  readonly valorCausa: string;
  readonly terceirizado: string;
  readonly codEstado: string;
  readonly nomComarca: string;
  readonly codVara: number;
  readonly nomTipoVara: string;
  readonly nroProcessoCartorio: string;
  readonly nomParte: string;
  readonly codProcesso: string;
  readonly codParte: string;

  static fromObj(obj: any): PautaComposicao {
    return ({
      hora: obj.hora,
      tipo: obj.tipo,
      juizado: obj.juizado,
      processoEmpresaGrupo: obj.processoEmpresaGrupo,
      assunto: obj.assunto,
      pedido: obj.pedido,
      valorCausa: obj.valorCausa,
      terceirizado: obj.terceirizado,
      codEstado: obj.codEstado,
      nomComarca: obj.nomComarca,
      codVara: obj.codVara,
      nomTipoVara: obj.nomTipoVara,
      nroProcessoCartorio: obj.nroProcessoCartorio,
      nomParte: obj.nomParte,
      codProcesso: obj.codProcesso,
      codParte: obj.codParte,
    });
  }

}
