import { EmpresaModel } from './empresa.model';

export class GrupoEmpresaContabilSapModel {
  id: number;
  nome: string;
  empresasGrupo: EmpresaModel[];
  qtdEmpresasIniciaisControle: number;
  excluido: boolean;
  persistido: boolean;
  nomeAnterior: string;
  recuperanda: boolean;
  recuperandaAnterior: boolean;
}
