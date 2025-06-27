import { HttpClient, HttpErrorResponse, } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { HttpErrorResult } from '@core/http/http-error-result';
import { queryStringGenerator } from '@shared/helpers/url-helper';

export abstract class BuscarService<T> {

  protected endPoint: string;

  constructor(protected http: HttpClient) { }

  protected url(endPoint: string): string {
    return `${environment.api_url}/${endPoint}`;
  }

  async listar(): Promise<Array<T>> {
    try {
    return await this.http
      .get<Array<T>> (this.url(this.endPoint))
      .toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  pesquisar(propriedades: Array<string>, valores: Array<any>): Promise<Array<T>> {
    try {
      return this.http.get<any> (
        this.url(`${this.endPoint}${queryStringGenerator(propriedades, valores)}`)
      ).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  linkExportar(propriedades: Array<string>, valores: Array<any>): string {
    return this.url(`${this.endPoint}/exportar${queryStringGenerator(propriedades, valores)}`);
  }

  obterQuantidade(propriedades: Array<string>, valores: Array<any>): Promise<number> {
    try {
      return this.http.get<any> (
        this.url(`${this.endPoint}/obterQuantidade${queryStringGenerator(propriedades, valores)}`)
      ).toPromise();
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }
}
