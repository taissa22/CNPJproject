export class EmpresaDoGrupo {
  constructor(id: number, nome: string, codCentroSap?: string) {
    this.id = id;
    this.nome = nome;
    this.codCentroSap = codCentroSap;
  }

  id: number;
  nome: string;
  codCentroSap?: string;
}
