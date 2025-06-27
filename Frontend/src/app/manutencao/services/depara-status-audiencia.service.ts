import { HttpClient } from '@angular/common/http';

import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';

import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { HttpErrorResponse } from '@angular/common/http';
import { DeparaStatusAudiencia } from '@manutencao/models/depara-status-audiencia.model';
import { DeparaStatusResponse } from '@manutencao/models/depara-status-response.model';

@Injectable({
  providedIn: 'root'
})
export class DeparaStatusAudienciaService {

  private endPoint: string = 'DeparaStatusAudiencia';

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros: string = ''): string { return `${environment.api_v2_url}/${this.endPoint}/${parametros}`; }

  public obter(sort: Sort, page: Page, tipoProcesso: number, pesquisa?: string): Observable<{ data: Array<DeparaStatusAudiencia>, total: number }> {
    let url: string = this.url(`ObterDeParaStatus?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}&tipoProcesso=${tipoProcesso}`);
    if (pesquisa) {
      url = `${url}&pesquisa=${pesquisa}`
    }
    return this.http.get<{ data: Array<DeparaStatusAudiencia>, total: number }>(url)
      .pipe(map(x => {
        return {
          total: x.total,
          data: x.data.map(o => DeparaStatusAudiencia.fromObj(o))
        };
      }));
  }

  public excluir(id: number): Observable<any> {
    const url: string = `${environment.api_v2_url}/DeparaStatusAudiencia/?id=${id}`;
    return this.http.delete(url);
  }

  async exportar(sort: Sort, page: Page,tipoProcesso: number, pesquisa?: string): Promise<void> {
    try {

      let link = this.url(`Exportar?coluna=${sort.column}&direcao=${sort.direction}&tipoProcesso=${tipoProcesso}`);

      if (pesquisa) {
        link = `${link}&pesquisa=${pesquisa}`;
      }

      const response: any = await this.http.get(link, this.exportarService.downloadOptions).toPromise();
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

  async obterStatusAppPrepostoEStatusSisjur(): Promise<any> {
    try {
      let url: string = this.url(`ObterStatusAppPrepostoEStatusSisjur`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  incluir(deparaStatusAudiencia: DeparaStatusAudiencia) {
    const url: string = `${environment.api_v2_url}/DeparaStatusAudiencia/Incluir`;
    return this.http.post(url, deparaStatusAudiencia).toPromise();
  }

  alterar(deparaStatusAudiencia: DeparaStatusAudiencia) {
    const url: string = `${environment.api_v2_url}/DeparaStatusAudiencia/Alterar`;
    return this.http.put(url, deparaStatusAudiencia).toPromise();
  }

}
