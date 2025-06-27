import { HttpClient } from '@angular/common/http';
import { environment } from '@environment';
import { Injectable } from '@angular/core';
import { PautaProconPesquisaModel } from '../models/pauta-procon-pesquisa.model';
import { HttpErrorResult } from '@core/http';
import { LocalStorageService } from '@core/services/local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class PautaProconPesquisaService {

  //private pautaProconPesquisaModel: PautaProconPesquisaModel;

  constructor(private http: HttpClient, private localStorage: LocalStorageService) { }

  protected url(parametros: string = ''): string { return `${environment.api_v2_url}/api/PautaProconPesquisa/${parametros}`; }

  public guard(model: PautaProconPesquisaModel): void {

    this.localStorage.setItem('filtroPesquisaPautaProcon', JSON.stringify(model));
  }

  public rescue(): PautaProconPesquisaModel {
    const filtrosStorage = this.localStorage.getItem('filtroPesquisaPautaProcon');
    let filtroPesquisaPautaProcon: PautaProconPesquisaModel;

    filtrosStorage != null ? filtroPesquisaPautaProcon = JSON.parse(filtrosStorage) : filtroPesquisaPautaProcon = null;

    return filtroPesquisaPautaProcon;
  }

  public async carregarTipoAudiencia(): Promise<any> {
    try {
      const url: string = this.url(`ListaTipoAudiencia`);
      return await this.http.get<any>(url).toPromise();
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

  public async carregarEmpresasDoGrupo(escritorio: boolean, contador: boolean, login: string): Promise<any> {
    try {
      const body = { 'escritorio': escritorio, 'contador': contador, 'login': login }
      const url: string = this.url(`ListaEmpresasDoGrupo`);
      return await this.http.post<any>(url, body).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async carregarGrupoProcon(): Promise<any> {
    try {
      const url: string = this.url(`ListaGrupoProcon`);
      return await this.http.get<any>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
