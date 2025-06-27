import { EmpresaModel } from './empresa.model';
import { GrupoEmpresaContabilSapModel } from './grupo-empresa-contabil-sap.model';

export class ListaEmpresasGrupoEEmpresasDisponiveisModel {
  grupoXEmpresas: Array<GrupoEmpresaContabilSapModel>;
  empresasDisponiveis: Array<EmpresaModel>;
}
