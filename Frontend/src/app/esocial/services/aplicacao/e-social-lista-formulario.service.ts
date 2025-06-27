import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { RetornoLista } from '../../models/retorno-lista';

@Injectable({
  providedIn: 'root'
})
export class ESocialListaFormularioService {

  private readonly href: string = `${environment.api_v2_url}/api/esocial/v1/ESocialListasFormulario`;

  constructor(
    private http: HttpClient,
  ) { }

  public async obterStatusFormularioAsync(): Promise<Array<RetornoLista>> {
    let url: string = `${this.href}/lista/status-formulario`;
    return this.http.get<Array<RetornoLista>>(url).toPromise();
  }

  public async obterStatusReclamanteAsync(): Promise<any> {
    let url: string = `${this.href}/lista/status-reclamante`;
    return this.http.get<any>(url).toPromise();
  }

  public async obterOrigemProcessoDemandaAsync(): Promise<any> {
    let url: string = `${this.href}/lista/origem-processo-demanda`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterUfVaraTramitacaoProcessoJudicialAsync(): Promise<any> {
    let url: string = `${this.href}/lista/uf`;
    return await this.http.get<any>(url).toPromise()
  }


  public async obterCodigoMunicipioAsync(uf: string): Promise<any> {
    let url: string = `${this.href}/lista/codigo-municipio/${uf}`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoAmbitoCelebracaoAcordoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-de-ambito-de-celebracao-do-acordo`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterDependenteAsync(): Promise<any> {
    let url: string = `${this.href}/lista/dependente`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoDependenteAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-dependente`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterTipoRendimentoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-rendimento`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterTipoContratoTSVEAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-contrato-tsve`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterCodigoCategoriaAsync(): Promise<any> {
    let url: string = `${this.href}/lista/codigo-categoria`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterNaturezaAtividadeAsync(): Promise<any> {
    let url: string = `${this.href}/lista/natureza-atividade`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterMotivoTerminoTSVEAsync(): Promise<any> {
    let url: string = `${this.href}/lista/motivo-termino-tsve`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterCodigoCBOAsync(): Promise<any> {
    let url: string = `${this.href}/lista/codigoCBO`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoRegimeTrabalhistaAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-regime-trabalhista`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoRegimePrevidenciarioAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-regime-previdenciario`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoContratoTempoParcialAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-contrato-tempo-parcial`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoInscricaoSucessaoVinculoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-inscricao-sucessao-vinculo`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoContratoVinculoDesligamentoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-contrato-vinculo-desligamento`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterMotivoDesligamentoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/motivo-desligamento`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterUnidadePagamentoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/unidade-pagamento`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterTipoInscricaoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-inscricao`;
    return await this.http.get<any>(url).toPromise();
  }


  public async obterRepercussaoProcessoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/repercussao-processo`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterIndRepercussaoProcessoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/ind-repercussao-processo`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterGrauExposicaoAsync(): Promise<any> {
    let url: string = `${this.href}/lista/grau-exposicao`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterCodigoCategoriaTrabalhadorPeriodoReferenciaAsync(): Promise<any> {
    let url: string = `${this.href}/lista/codigo-categoria-trabalhador-periodo-referencia`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterCodigoReceitaAsync(): Promise<any> {
    let url: string = `${this.href}/lista/codigo-receita`;
    return await this.http.get<any>(url).toPromise();
  }

  public async obterCodigoReceitaIRRFAsync(): Promise<any> {
    let url: string = `${this.href}/lista/codigo-receita-IRRF`;
    return await this.http.get<any>(url).toPromise();
  }
  
  public async obterListaTipoProcessoIRRFAsync(): Promise<any> {
    let url: string = `${this.href}/lista/tipo-processo-IRRF`;
    return await this.http.get<any>(url).toPromise();
  }
  
  public async obterListaIndApuracaoValoresAsync(): Promise<any> {
    let url: string = `${this.href}/lista/valores/ind-apuracao`;
    return await this.http.get<any>(url).toPromise();
  }
 
  public async obterListaTipoDeducoesValoresAsync(): Promise<any> {
    let url: string = `${this.href}/lista/deducao/tipo-deducoes`;
    return await this.http.get<any>(url).toPromise();
  }
}
