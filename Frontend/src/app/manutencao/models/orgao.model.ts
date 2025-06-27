import { TipoOrgao } from './tipo-orgao.enum';
import { Competencia } from './competencias.model';
import { Formatter } from '@manutencao/formatter';

export class Orgao {
  id: number;
  nome: string;
  telefoneDDD: string;
  telefone: string;
  tipoOrgao: TipoOrgao | number;
  estado: { id: string; nome: string };
  competencias: Array<Competencia>;

  get telefoneCompleto(): string {
    if (this.telefoneDDD && this.telefone) {
      return Formatter.formatTelefone(this.telefoneDDD, this.telefone);
    }
    return;
  }

  get nomesCompetencias(): string {
    const nomesCompetencias: Array<string> = this.competencias.map(p => p.nome).sort((a, b) => {
      if (a < b) {
        return -1;
      }
      if (a > b) {
        return 1;
      }
      return 0;
    });
    const competenciasConcatenadas = nomesCompetencias.length > 1 ? nomesCompetencias.join(', ') : nomesCompetencias.toString();
    return competenciasConcatenadas.length > 40 ? `${competenciasConcatenadas.substring(0, 40)}...` : competenciasConcatenadas;
  }

  static fromJson(o: Orgao): Orgao {
    const orgao = new Orgao();
    orgao.id = o.id;
    orgao.nome = o.nome;
    orgao.telefoneDDD = o.telefoneDDD;
    orgao.telefone = o.telefone;
    orgao.tipoOrgao = TipoOrgao.fromJson(TipoOrgao.Todos.filter((t) => t.valor === (typeof o.tipoOrgao === 'number' ? o.tipoOrgao : o.tipoOrgao.valor))[0]);
    orgao.estado = o.estado;
    orgao.competencias = [];
    o.competencias.forEach(x => orgao.competencias.push(Competencia.fromJson(x)));
    return orgao;
  }
}
