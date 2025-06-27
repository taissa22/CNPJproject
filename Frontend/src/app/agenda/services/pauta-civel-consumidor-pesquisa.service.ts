import { HttpClient } from '@angular/common/http';
import { environment } from '@environment';
import { HttpErrorResult } from '@core/http';
import { Injectable } from '@angular/core';
import { PautaCivelConsumidorPesquisaModel } from '../models/pauta-civel-consumidor-pesquisa.model';
import { LocalStorageService } from '@core/services/local-storage.service';


@Injectable({
  providedIn: 'root'
})
export class PautaCivelConsumidorPesquisaService {

  private pautaCivelConsumidorPesquisaModel: PautaCivelConsumidorPesquisaModel;

  constructor(private http: HttpClient, private localStorage: LocalStorageService) { }

  protected url(parametros: string = ''): string {
    return `${environment.api_v2_url}/api/PautaCivelConsumidorPesquisa/${parametros}`;
  }


  public guard(model: PautaCivelConsumidorPesquisaModel): void {

    this.localStorage.setItem('filtroPesquisaPautaCC', JSON.stringify(model));
  }

  public rescue(): PautaCivelConsumidorPesquisaModel {
    let filtrosStorage = this.localStorage.getItem('filtroPesquisaPautaCC');
    let filtroPesquisaPautaCC: PautaCivelConsumidorPesquisaModel;

    filtrosStorage == undefined ? filtrosStorage = null :  filtrosStorage ;

    filtrosStorage != null ? filtroPesquisaPautaCC = JSON.parse(filtrosStorage) : filtroPesquisaPautaCC = null;

    return filtroPesquisaPautaCC;
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

}
