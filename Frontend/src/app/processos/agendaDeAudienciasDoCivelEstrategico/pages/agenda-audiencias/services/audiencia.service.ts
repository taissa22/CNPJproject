import { SortOrderView } from './../../../../../shared/interfaces/Sort/SortOrderView';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { isNullOrUndefined } from 'util';
import { environment } from './../../../../../../environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Audiencia } from '../model/audiencia.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AudienciaService {
  constructor(private api: HttpClient, private exportarService: TransferenciaArquivos) { }

  private url(endPoint: string = '') {
    return `${environment.api_url}/agenda-de-audiencias-do-civel-estrategico/audiencias/${endPoint}`;
  }

  private urlBase(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  public async obterPaginado(filtro: {
    pagina: number, quantidade: number, dataInicial: Date,
    dataFinal: Date, escritorioId: number, estadoId: string,
    comarcaId: number, empresaGrupoId: number, prepostoId: number,
    assuntoId: number, closing: number, classificacaoProcessoId : string, comarca: QueryOrder, estado: QueryOrder, vara: QueryOrder,
    tipoVara: QueryOrder, dataAudiencia: QueryOrder, horaAudiencia: QueryOrder,
  }): Promise<{ data: Audiencia[], total: number }> {
    try {
      const urlBase = this.url(`obter`);
      return await this.api.post<{ data: Audiencia[], total: number }>(urlBase, filtro).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  async exportar(parametros: any): Promise<void> {
    try {
      const response: any = await this.api
        .post(this.url('exportar'), parametros, this.exportarService.downloadOptions)
        .toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  gerarUrl(path: string, parametros: any, adicionarOrdenacao: boolean = true) {
    let url: string = path + '?';
    const dataInicialForm = new Date(parametros.DataInicial);
    const dataFinalForm = new Date(parametros.DataFinal);

    url += 'dataRange=' + JSON.stringify({ dataInicial: dataInicialForm.toISOString(), dataFinal: dataFinalForm.toISOString() }) + '&';
    url += 'page=' + JSON.stringify({
      pagina: parametros.Paginacao.paginaAtual,
      quantidade: parametros.Paginacao.totalDeRegistrosPorPagina
    }) + '&';

    url += `estado=${JSON.stringify({ query: parametros.Estado })}&`;
    url += `comarca=${JSON.stringify({ query: parametros.Comarca !== '' ? parametros.Comarca : 0 })}&`;
    url += `empresaGrupo=${JSON.stringify({ query: parametros.EmpresaGrupo !== '' ? parametros.EmpresaGrupo : 0 })}&`;
    url += `preposto=${JSON.stringify({ query: parametros.Preposto !== '' ? parametros.Preposto : 0 })}&`;
    url += `escritorio=${JSON.stringify({ query: parametros.Escritorio !== '' ? parametros.Escritorio : 0 })}&`;

    if (adicionarOrdenacao && !isNullOrUndefined(parametros.Ordenacao)) {
      parametros.Ordenacao.forEach((o, index) => {
        url += `${o.property}=${JSON.stringify({ ordem: index, asc: o.direction })}&`;
      });
    }
    return encodeURI(url);
  }

  public async atualizar(command: {
    id: number, processoId: string, escritorioId: number, advogadoId: number, prepostoId: number, tipoAudienciaId: number
  }): Promise<void> {
    try {
      await this.api.put<void>(this.urlBase(`agenda-de-audiencias-do-civel-estrategico/audiencias`), command).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}

export type QueryOrder = { direction: string, ordem: number, column: string };
