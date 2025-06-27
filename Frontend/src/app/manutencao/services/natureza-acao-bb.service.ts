import { NaturezaAcaoBB } from '@manutencao/models/naturezaAcaoBB.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { HttpErrorResult } from '@core/http';


@Injectable({
  providedIn: 'root'
})
export class NaturezaAcaoBbService {

  protected endPoint: string = 'naturezas';

  constructor(private api: HttpClient) { }

  protected url(): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/`;
  }

  async obter(): Promise<Array<NaturezaAcaoBB>> {
    try {
      return this.api
        .get<Array<NaturezaAcaoBB>>(this.url())
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
}
