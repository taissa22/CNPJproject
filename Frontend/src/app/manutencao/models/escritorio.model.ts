import { BooleanNullToFalsePipe } from "@shared/pipes/booleanNullToFalse.pipe";
import { Sweetalert2ModuleConfig } from "@sweetalert2/ngx-sweetalert2";

export class Escritorio {
  private constructor(
    id: number,
    nome: string,
    ativo: boolean,
    ehEscritorio: boolean,
    endereco: string,
    civelEstrategico: boolean,
    tipoPessoaId: string,
    CPF: string,
    indAreaCivel : boolean,
    indAreaJuizado : boolean,
    indAreaRegulatoria : boolean,
    indAreaTrabalhista : boolean,
    indAreaTributaria : boolean,
    cep : number,
    cidade : string,
    email : string,
    estadoId : string,
    bairro : string,
    site : string,
    indAdvogado : boolean,
    indAreaCivelAdministrativo  : boolean,
    indAreaCriminalAdministrativo   : boolean,
    indAreaCriminalJudicial  : boolean,
    indAreaPEX  : boolean,
    indAreaProcon  : boolean,
    alertaEm  : number,
    codProfissionalSAP : string,
    CNPJ : string,
    TelefoneDDD : string,
    Telefone : string,
    celular: string,
    celularDDD: string,
    Fax : string,
    FaxDDD : string,
    enviarAppPreposto : boolean

) {
  this.id = id;
  this.nome = nome;
  this.ativo = ativo;
  this.ehEscritorio  =  ehEscritorio;
  this.endereco  = endereco;
  this.civelEstrategico  =  civelEstrategico;
  this.tipoPessoaId  = tipoPessoaId;
  this.CPF = CPF;
  this.indAreaCivel = indAreaCivel;
  this.indAreaJuizado = indAreaJuizado;
  this.indAreaRegulatoria = indAreaRegulatoria;
  this.indAreaTrabalhista = indAreaTrabalhista;
  this.indAreaTributaria = indAreaTributaria;
  this.cep = cep;
  this.cidade = cidade;
  this.email = email;
  this.estadoId = estadoId;
  this.bairro = bairro;
  this.site = site;
  this.indAdvogado =indAdvogado;
  this.indAreaCivelAdministrativo=indAreaCivelAdministrativo;
  this.indAreaCriminalAdministrativo=indAreaCriminalAdministrativo;
  this.indAreaCriminalJudicial=indAreaCriminalJudicial;
  this.indAreaPEX=indAreaPEX;
  this.indAreaProcon = indAreaProcon;
  this.alertaEm = alertaEm;
  this.codProfissionalSAP = codProfissionalSAP;
  this.CNPJ = CNPJ;
  this.TelefoneDDD  = TelefoneDDD;
  this.Telefone = Telefone;
  this.celular = celular;
  this.celularDDD = celularDDD;
  this.Fax = Fax;
  this.FaxDDD  = FaxDDD
  this.enviarAppPreposto = enviarAppPreposto;
}

  readonly id: number;
  readonly nome: string;
  readonly ativo: boolean;
  readonly ehEscritorio: boolean;
  readonly endereco: string;
  readonly civelEstrategico: boolean;
  readonly tipoPessoaId: string;
  readonly CPF: string;
  readonly indAreaCivel : boolean;
  readonly indAreaJuizado : boolean;
  readonly indAreaRegulatoria : boolean;
  readonly indAreaTrabalhista : boolean;
  readonly indAreaTributaria : boolean;
  readonly cep : number;
  readonly cidade : string;
  readonly email : string;
  readonly estadoId : string;
  readonly bairro : string;
  readonly site : string;
  readonly indAdvogado : boolean;
  readonly indAreaCivelAdministrativo : boolean;
  readonly indAreaCriminalAdministrativo : boolean;
  readonly indAreaCriminalJudicial : boolean;
  readonly indAreaPEX : boolean;
  readonly indAreaProcon  : boolean;
  readonly alertaEm : number;
  readonly codProfissionalSAP : string;
  readonly CNPJ : string;
  readonly Telefone : string;
  readonly TelefoneDDD : string;
  readonly celular : string;
  readonly celularDDD : string;
  readonly Fax : string;
  readonly FaxDDD : string;
  readonly enviarAppPreposto : boolean;


  static fromObj(obj: any): Escritorio {
    return ({
      id: obj.id,
      nome: obj.nome,
      ativo: obj.ativo,
      ehEscritorio: obj.ehEscritorio,
      endereco: obj.endereco,
      civelEstrategico: obj.civelEstrategico,
      tipoPessoaId: obj.tipoPessoaValor,
      CPF : obj.cpf,
      indAreaCivel: obj.indAreaCivel,
      indAreaJuizado: obj.indAreaJuizado,
      indAreaRegulatoria: obj.indAreaRegulatoria,
      indAreaTrabalhista: obj.indAreaTrabalhista,
      indAreaTributaria: obj.indAreaTributaria,
      cep: obj.cep,
      cidade: obj.cidade,
      email: obj.email,
      estadoId: obj.estadoId,
      bairro: obj.bairro,
      site : obj.site,
      indAdvogado : obj.indAdvogado,
      indAreaCivelAdministrativo : obj.indAreaCivelAdministrativo,
      indAreaCriminalAdministrativo : obj.indAreaCriminalAdministrativo,
      indAreaCriminalJudicial : obj.indAreaCriminalJudicial,
      indAreaPEX : obj.indAreaPEX,
      indAreaProcon  : obj.indAreaProcon,
      alertaEm  : obj.alertaEm,
      codProfissionalSAP  : obj.codProfissionalSAP,
      CNPJ : obj.cnpj,
      Telefone : obj.telefone,
      TelefoneDDD : obj.telefoneDDD,
      celular : obj.celular,
      celularDDD : obj.celularDDD,
      Fax : obj.fax,
      FaxDDD : obj.faxDDD,
      enviarAppPreposto : obj.enviarAppPreposto
    });
  }

}
