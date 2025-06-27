import { EstadosModel } from "./estados.model";

export class GrupoDeEstadosModel {
    id: number;
    nome: string;
    nomeAnterior: string;
    numeroEstadoIniciais: number;
    persistido: boolean;
    excluido: boolean;
    estadosGrupo: EstadosModel[];
  }
