export class PautaProconPesquisaModel {
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
  grupoProcon: string;
  separarPautaPorEmpresaCheck: boolean;
  statusDeAudiencia:string;
  preposto:[];
  requerPreposto: string;
  situacaoProcessoCC: string;
  procon: string;
}
