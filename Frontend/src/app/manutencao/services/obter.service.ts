import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Regional } from '@shared/models/regional.model';
import { HttpErrorResult } from '@core/http/http-error-result';
import { CentroCusto } from '@shared/models/centro-custo.model';
import { Fornecedor } from '@shared/models/fornecedor.model';
import { EmpresaCentralizadora } from '@manutencao/models';
import { InterfaceBb } from '@manutencao/models/interface-bb.model';

@Injectable({
  providedIn: 'root'
})
export class ObterService {
  constructor(private http: HttpClient) { }

  private urlBase: string = environment.api_url + '/manutencao';

  public async obterRegionais(): Promise<Array<Regional>> {
    try {
      return await this.http
        .get<Array<Regional>>(`${this.urlBase}/regionais`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterEmpresasCentralizadoras(): Promise<Array<EmpresaCentralizadora>> {
    try {
      return await this.http
        .get<Array<EmpresaCentralizadora>>(`${this.urlBase}/empresas-centralizadoras`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterCentrosDeCusto(): Promise<Array<CentroCusto>> {
    try {
      return await this.http
        .get<Array<CentroCusto>>(`${this.urlBase}/centros-de-custo`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterFornecedores(): Promise<Array<Fornecedor>> {
    try {
      return await this.http
        .get<Array<Fornecedor>>(`${this.urlBase}/fornecedores`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterInterfacesBB(): Promise<Array<InterfaceBb>> {
    try {
      return await this.http
        .get<Array<InterfaceBb>>(`${this.urlBase}/interfaces-bb`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async obterEmpresasSap(): Promise<Array<any>> {
    try {
      return await this.http
        .get<Array<any>>(`${this.urlBase}/empresas-sap`)
        .toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
