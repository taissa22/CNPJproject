import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '@environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { Sort } from '@shared/types/sort';
import { Page } from '@shared/types/page';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { DeparaStatusAudiencia } from '@manutencao/models/depara-status-audiencia.model';
import { Acao } from '@manutencao/models/acao.model';
import { HttpErrorResult } from '@core/http';

@Injectable({
  providedIn: 'root'
})
export class AcaoService {

  private endPoint: string = 'Acao';

  constructor(private http: HttpClient, private exportarService: TransferenciaArquivos) { }

  protected url(parametros: string = ''): string { return `${environment.api_v2_url}/${this.endPoint}/${parametros}`; }

  public obter(sort: Sort, page: Page, tipoProcesso: number, pesquisa?: string): Observable<{ data: Array<Acao>, total: number }> {

    let url: string = this.url(`ObterAcao?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}&tipoProcesso=${tipoProcesso}`);

    if (pesquisa) { url = `${url}&pesquisa=${pesquisa}` };

    return this.http.get<{ data: Array<Acao>, total: number }>(url)
      .pipe(map(x => {
        return {
          total: x.total,
          data: x.data.map(o => Acao.fromObj(o))
        };
      }));
  }

  async excluir(codigo: number, tipoProcesso: number): Promise<void> {
    try {
      const url: string = `${environment.api_v2_url}/Acao/?id=${codigo}&tipoProcesso=${tipoProcesso}`;
      await this.http.delete(url).toPromise();
    } catch (error) {
      throw error;
    }
  }

  async exportar(sort: Sort, tipoProcesso: number, pesquisa?: string): Promise<void> {
    try {

      let link = this.url(`Exportar?coluna=${sort.column}&direcao=${sort.direction}&tipoProcesso=${tipoProcesso}`);

      if (pesquisa) { link = `${link}&pesquisa=${pesquisa}`; }

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

  async ObterNaturezaAcaoBB(): Promise<any> {
    try {
      let url: string = this.url(`ObterNaturezaAcaoBB`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async ObterAcoesCivelEstrategico(): Promise<any> {
    try {
      let url: string = this.url(`ObterAcoesCivelEstrategico`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async ObterAcoesCivelConsumidor(): Promise<any> {
    try {
      let url: string = this.url(`ObterAcoesCivelConsumidor`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  incluir(acao: Acao) {
    const url: string = `${environment.api_v2_url}/Acao/Incluir`;
    return this.http.post(url, acao).toPromise();
  }

  alterar(acao: Acao) {
    const url: string = `${environment.api_v2_url}/Acao/Alterar`;
    return this.http.put(url, acao).toPromise();
  }

}
