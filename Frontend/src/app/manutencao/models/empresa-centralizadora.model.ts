import { Convenio } from './convenio.model';

export class EmpresaCentralizadora {
  ordem: number;
  codigo: number;
  nome: string;
  convenios: Array<Convenio>;

  static fromJson(e: EmpresaCentralizadora): EmpresaCentralizadora {
    const empresa = new EmpresaCentralizadora();
    empresa.ordem = e.ordem;
    empresa.codigo = e.codigo;
    empresa.nome = e.nome;
    empresa.convenios = [];
    e.convenios.forEach(c => empresa.convenios.push(Convenio.fromJson(c)));
    return empresa;
  }
}
