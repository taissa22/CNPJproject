export class PautaCivelConsumidorPesquisaModel {
  periodoInicio: Date;
  periodoFim: Date;
  tipoAudiencia:string;
  empresaDoGrupo:string;
  estado:string;
  comarca: string;
  vara: string;
  situacaoProcesso: boolean;
  audienciaSemPreposto: string;
  empresaCentralizadora: string;
  grupoJuizado: string;
  statusDeAudiencia:string;
  preposto:[];
  requerPreposto: string;
  situacaoProcessoCC: string;
}
