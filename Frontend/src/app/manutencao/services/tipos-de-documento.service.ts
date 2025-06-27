import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { DatePipe} from '@angular/common';
import { Inject } from '@angular/core';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { List } from 'linqts';
import { TipoDeDocumento, TipoDocumentoBack } from '@manutencao/models/tipos-de-documento.model';
import { TipoPrazo } from 'src/app/processos/agendaDeAudienciasDoCivelEstrategico/models';

@Injectable({
  providedIn: 'root'
})
export class TipoDeDocumentoService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private tipoProcessoService: TipoProcessoService, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/tipos-de-documentos`;

  tipoProcessoCombo = new EventEmitter;
  tipoProcessoModal: any[] = [];

  tiposDeProcesso = []

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  obter(
    sort: Sort,
    page: Page,
    tipoDeProcesso: number,
    pesquisa?: string,
  ): Observable<QueryResult<TipoDeDocumento>> {
    let url: string = `${this.href}/${tipoDeProcesso}?pagina=${page.index + 1}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http
      .get<{ total: number; data: Array<TipoDocumentoBack> }>(url)
      .pipe(
        map(x => {
          return {
            total: x.total,
            lista: x.data.map(obj => TipoDeDocumento.fromObj(obj))
          };
        })
      );

  }

  async getTiposDeProcesso(): Promise<Array<TiposProcesso>> {
    try {
      const tipoDeProcesso = 0;
      const url: string = `${this.href}/${tipoDeProcesso}`;
      const tiposProcesso = await this.http.get<any>(url).toPromise();
      return tiposProcesso.map((tipoProcesso: any) => {
        return {
          id: tipoProcesso.id,
          descricao: tipoProcesso.nome
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async getTiposDePrazo(): Promise<Array<TipoPrazo>> {
    try {
      const url: string = `${this.href}/ObterComboboxTiposPrazo`;
      const tiposProcesso = await this.http.get<any>(url).toPromise();
      return tiposProcesso.map((tipoPrazo: any) => {
        return {
          id: tipoPrazo.id,
          descricao: tipoPrazo.descricao
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async incluir(
    descricao: string,
    tipoDeProcesso: number,
    ativo: boolean,
    cadastraProcesso: boolean,
    requerAudiencia: boolean,
    prioritarioFila: boolean,
    utilizadoEmProtocolo: boolean,
    documentoApuracao:boolean,
    enviarAppPreposto: boolean,
    tipoDePrazo?: number,
    idEstrategico?:number,
    idConsumidor?: number

  ): Promise<void> {
    const obj = {
      descricao: descricao.toUpperCase(),
      codTipoProcesso: tipoDeProcesso,
      ativo: ativo,
      marcadoCriacaoProcesso: cadastraProcesso,
      IndRequerDatAudiencia: requerAudiencia,
      IndPrioritarioFila: prioritarioFila,
      IndDocumentoProtocolo: utilizadoEmProtocolo,
      IndDocumentoApuracao:documentoApuracao,
      IndEnviarAppPreposto:enviarAppPreposto,
      CodTipoPrazo: tipoDePrazo,
      IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor
    };
    try {
      await this.http.post(`${this.href}`, obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(
    codigo: number,
    descricao: string,
    tipoDeProcesso: number,
    ativo: boolean,
    cadastraProcesso: boolean,
    requerAudiencia: boolean,
    prioritarioFila: boolean,
    utilizadoEmProtocolo: boolean,
    documentoApuracao:boolean,
    enviarAppPreposto: boolean,
    tipoDePrazo?: number,
    idEstrategico?:number,
    idConsumidor?:number
  ): Promise<void> {
    const obj = {
      id: codigo,
      descricao: descricao.toUpperCase(),
      codTipoProcesso: tipoDeProcesso,
      ativo: ativo,
      marcadoCriacaoProcesso: cadastraProcesso,
      IndRequerDatAudiencia: requerAudiencia,
      IndPrioritarioFila: prioritarioFila,
      IndDocumentoProtocolo: utilizadoEmProtocolo,
      IndDocumentoApuracao:documentoApuracao,
      IndEnviarAppPreposto:enviarAppPreposto,
      CodTipoPrazo: tipoDePrazo,
      IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor
    };
    try {
      await this.http.put(`${this.href}`, obj).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigo: number): Promise<void> {
    try {
      await this.http.delete(`${this.href}/${codigo}`).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(
    sort: Sort,
    tipoDeProcesso?: number,
    pesquisa?: string,  ): Promise<void> {
    try {
      let url = `${this.href}/exportar/${tipoDeProcesso}?coluna=${sort.column}&direcao=${sort.direction}`;
      if (pesquisa) {
        url = `${url}&pesquisa=${pesquisa}`
      }
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);

    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async ObterDescricaoDeParaCivelEstrategico(): Promise<Array<TipoDeDocumento>> {
    try {
      const url: string = `${this.href}/ObterDescricaoDeParaCivelEstrategico`;
      const tipoDocumento = await this.http.get<any>(url).toPromise();
      return tipoDocumento.map((tipoDocumento: any) => {
        return {
          codigo: tipoDocumento.id,
          descricao: tipoDocumento.descricao,
          ativo: tipoDocumento.ativo
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  async ObterDescricaoDeParaCivelConsumidor(): Promise<Array<TipoDeDocumento>> {
    try {
      const url: string = `${this.href}/ObterDescricaoDeParaCivelConsumidor`;
      const tipoDocumento = await this.http.get<any>(url).toPromise();
      return tipoDocumento.map((tipoDocumento: any) => {
        return {
          codigo: tipoDocumento.id,
          descricao: tipoDocumento.descricao,
          ativo: tipoDocumento.ativo
        }
      })
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
