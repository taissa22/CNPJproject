import { Objeto } from './objeto.model';
export class Evento {
  private constructor(
    id: number,
    nome: string,
    possuiDecisao: boolean,
    ehCivel: boolean,
    ehTrabalhista: boolean,
    ehRegulatorio: boolean,
    ehPrazo: boolean,
    ehTributarioAdm: boolean,
    ehTributarioJudicial: boolean,
    finalizacaoEscritorio: boolean,
    tipoMulta: string,
    ehTrabalhistaAdm: boolean,
    notificarViaEmail: boolean,
    ehJuizado: boolean,
    reverCalculo: boolean,
    atualizaEscritorio: boolean,
    ehCivelEstrategico: boolean,
    instanciaId: number,
    preencheMulta: boolean,
    alterarExcluir: boolean,
    ativo: boolean,
    ehCriminalJudicial: boolean,
    ehCriminalAdm: boolean,
    ehCivelAdm: boolean,
    exigeComentario: boolean,
    finalizacaoContabil: boolean,
    ehProcon: boolean,
    ehPexJuizado: boolean,
    ehPexCivelConsumidor: boolean,
    ativoEstrategico: boolean,
    ativoConsumidor: boolean,
    descricaoEstrategico: string,
    descricaoConsumidor: string,
    idDescricaoConsumidor? : number,
    idDescricaoEstrategico? : number,
) {
  this.id = id;
  this.nome = nome;
  this.possuiDecisao = possuiDecisao;
  this.ehCivel = ehCivel;
  this.ehTrabalhista = ehTrabalhista;
  this.ehRegulatorio = ehRegulatorio;
  this.ehPrazo = ehPrazo;
  this.ehTributarioAdm = ehTributarioAdm;
  this.ehTributarioJudicial = ehTributarioJudicial;
  this.finalizacaoEscritorio = finalizacaoEscritorio;
  this.tipoMulta = tipoMulta;
  this.ehTrabalhistaAdm = ehTrabalhistaAdm;
  this.notificarViaEmail = notificarViaEmail;
  this.ehJuizado = ehJuizado;
  this.reverCalculo = reverCalculo;
  this.atualizaEscritorio = atualizaEscritorio;
  this.ehCivelEstrategico = ehCivelEstrategico;
  this.instanciaId = instanciaId;
  this.preencheMulta = preencheMulta;
  this.alterarExcluir = alterarExcluir;
  this.ativo = ativo;
  this.ehCriminalJudicial = ehCriminalJudicial;
  this.ehCriminalAdm = ehCriminalAdm;
  this.ehCivelAdm = ehCivelAdm;
  this.exigeComentario = exigeComentario;
  this.finalizacaoContabil = finalizacaoContabil;
  this.ehProcon = ehProcon;
  this.ehPexJuizado = ehPexJuizado;
  this.ehPexCivelConsumidor = ehPexCivelConsumidor;
  this.ativoEstrategico = ativoEstrategico;
  this.idDescricaoEstrategico = idDescricaoEstrategico;
  this.descricaoEstrategico = descricaoEstrategico;
  this.ativoConsumidor = ativoConsumidor;
  this.idDescricaoConsumidor = idDescricaoConsumidor;
  this.descricaoConsumidor = descricaoConsumidor;
}
  readonly id : number;
  readonly nome: string;
  readonly possuiDecisao: boolean;
  readonly ehCivel: boolean;
  readonly ehTrabalhista: boolean;
  readonly ehRegulatorio: boolean;
  readonly ehPrazo: boolean;
  readonly ehTributarioAdm: boolean;
  readonly ehTributarioJudicial: boolean;
  readonly finalizacaoEscritorio: boolean;
  readonly tipoMulta: string;
  readonly ehTrabalhistaAdm: boolean;
  readonly notificarViaEmail: boolean;
  readonly ehJuizado: boolean;
  readonly reverCalculo: boolean;
  readonly atualizaEscritorio: boolean;
  readonly ehCivelEstrategico: boolean;
  readonly instanciaId: number;
  readonly preencheMulta: boolean;
  readonly alterarExcluir: boolean;
  readonly ativo: boolean;
  readonly ehCriminalJudicial: boolean;
  readonly ehCriminalAdm: boolean;
  readonly ehCivelAdm: boolean;
  readonly exigeComentario: boolean;
  readonly finalizacaoContabil: boolean;
  readonly ehProcon: boolean;
  readonly ehPexJuizado: boolean;
  readonly ehPexCivelConsumidor: boolean;
  readonly ativoEstrategico: boolean;
  readonly idDescricaoEstrategico?: number;
  readonly descricaoEstrategico: string;
  readonly ativoConsumidor: boolean;
  readonly idDescricaoConsumidor?: number;
  readonly descricaoConsumidor: string;

  static fromObj(obj: any): Evento {
    return ({
      id : obj.id,
      nome : obj.nome,
      possuiDecisao : obj.possuiDecisao,
      ehCivel : obj.ehCivel,
      ehTrabalhista : obj.ehTrabalhista,
      ehRegulatorio : obj.ehRegulatorio,
      ehPrazo : obj.ehPrazo,
      ehTributarioAdm : obj.ehTributarioAdm,
      ehTributarioJudicial : obj.ehTributarioJudicial,
      finalizacaoEscritorio : obj.finalizacaoEscritorio,
      tipoMulta : obj.tipoMulta,
      ehTrabalhistaAdm : obj.ehTrabalhistaAdm,
      notificarViaEmail : obj.notificarViaEmail,
      ehJuizado : obj.ehJuizado,
      reverCalculo : obj.reverCalculo,
      atualizaEscritorio : obj.atualizaEscritorio,
      ehCivelEstrategico : obj.ehCivelEstrategico,
      instanciaId : obj.instanciaId,
      preencheMulta : obj.preencheMulta,
      alterarExcluir : obj.alterarExcluir,
      ativo : obj.ativo,
      ehCriminalJudicial : obj.ehCriminalJudicial,
      ehCriminalAdm : obj.ehCriminalAdm,
      ehCivelAdm : obj.ehCivelAdm,
      exigeComentario : obj.exigeComentario,
      finalizacaoContabil : obj.finalizacaoContabil,
      ehProcon : obj.ehProcon,
      ehPexJuizado : obj.ehPexJuizado,
      ehPexCivelConsumidor : obj.ehPexCivelConsumidor,
      ativoEstrategico: obj.ativoEstrategico,
      idDescricaoEstrategico : obj.idDescricaoEstrategico,
      descricaoEstrategico: obj.descricaoEstrategico,
      ativoConsumidor: obj.ativoConsumidor,
      idDescricaoConsumidor : obj.idDescricaoConsumidor,
      descricaoConsumidor: obj.descricaoConsumidor,
    });
  }



}
