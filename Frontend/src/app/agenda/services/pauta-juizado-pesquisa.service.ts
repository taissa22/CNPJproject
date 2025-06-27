import { HttpClient } from '@angular/common/http';
import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';
import { Injectable } from '@angular/core';
import { PautaJuizadoPesquisaModel } from '../models/pauta-juizado-pesquisa.model';
import { LocalStorageService } from '@core/services/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class PautaJuizadoPesquisaService {

  private pautaJuizadoPesquisaModel: PautaJuizadoPesquisaModel;

  constructor(private http: HttpClient, private localStorage: LocalStorageService) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/PautaJuizadoPesquisa/${parametros}`;
  }

  public async carregarTipoAudiencia(): Promise<any> {
    try {
      const url: string = this.url(`ListaTipoAudiencia`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarEmpresasDoGrupo(escritorio: boolean, contador: boolean, login: string): Promise<any> {
    try {
      const body = { 'escritorio': escritorio, 'contador': contador, 'login': login }
      const url: string = this.url(`ListaEmpresasDoGrupo`);
      return await this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarJuizadosPorComarca(comarca): Promise<any> {
    try {
      const url: string = this.url(`ListaJuizadosPorComarca/${comarca}`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarEmpresaCentralizadora(): Promise<any> {
    try {
      const url: string = this.url(`ListaEmpresasCentralizadoras`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarStatusAudiencia(): Promise<any> {
    try {
      const url: string = this.url(`ListaStatusDeAudiencia`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarGrupoDeJuizado(): Promise<any> {
    try {
      const url: string = this.url(`ListaGrupoDeJuizados`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarPreposto(): Promise<any> {
    try {
      const url: string = this.url(`ListaPrepostos`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }


  public guard(model: PautaJuizadoPesquisaModel): void {

    this.localStorage.setItem('filtroPesquisaPautaJuizado', JSON.stringify(model));
  }

  public rescue(): PautaJuizadoPesquisaModel {
    const filtrosStorage = this.localStorage.getItem('filtroPesquisaPautaJuizado');
    let filtroPesquisaPautaJuizado: PautaJuizadoPesquisaModel;

    filtrosStorage != null ? filtroPesquisaPautaJuizado = JSON.parse(filtrosStorage) : filtroPesquisaPautaJuizado = null;

    return filtroPesquisaPautaJuizado;
  }



}
