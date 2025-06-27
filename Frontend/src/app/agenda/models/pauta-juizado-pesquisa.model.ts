export class PautaJuizadoPesquisaModel {
  porJuizado: string;
  periodoInicio: Date;
  periodoFim: Date;
  tipoAudiencia:string;
  empresaDoGrupo:string;
  estado:string;
  comarca: string;
  juizado: string;
  situacaoProcesso: boolean;
  audienciaSemPreposto: string;
  empresaCentralizadora: string;
  grupoJuizado: string;
  separarPautaPorEmpresaCheck: boolean;
  statusDeAudiencia:string;
  preposto:[];
}
