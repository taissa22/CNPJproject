import { Feriado } from './../models/feriado.model';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { LocalStorageService } from './local-storage.service';
import { WeekDay } from '@angular/common';
import { HttpErrorResult } from '@core/http';
import { isNullOrUndefined } from 'util';

@Injectable({ providedIn: 'root' })
export class FeriadoService {
  private chave = 'lista_feriados';

  constructor(
    private http: HttpClient,
    private localStorageService: LocalStorageService) { }

  private url(endpoint: string): string {
    return `${environment.api_url}/${endpoint}`;
  }

  async obterFuturos(): Promise<Array<Feriado>> {
    try {
      const url = this.url(`agenda-de-audiencias-do-civel-estrategico/feriados/futuros`);
      const feriados = await this.http.get<Array<any>>(url).toPromise();
      if (isNullOrUndefined(feriados)) {
        return new Array<Feriado>();
      }
      return this.converterParaArrayDeFeriado(feriados);
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      if (Array.isArray(httpError.error)) {
        throw new HttpErrorResult(httpError.error);
      } else {
        throw new HttpErrorResult(['Erro desconhecido']);
      }
    }
  }

  private converterParaArrayDeFeriado(array: Array<any>) {
    if (isNullOrUndefined(array)) {
      return null;
    }
    return array.map(p => new Feriado(p.id, new Date(p.data), null));
  }

  // Método para carregar chave ao iniciar a aplicação, não utilizar em outro contexto!
  iniciarChaveDeFeriados() {

    if (this.localStorageService.hasItem(this.chave)) {
      this.localStorageService.removeItem(this.chave);
    }

    this.obterFuturos().then(res => {
      this.localStorageService.setItemJson(this.chave, res);
    });
  }

  obterFeriados(): Array<Feriado> {
    if (!this.localStorageService.hasItem(this.chave)) {
      this.iniciarChaveDeFeriados();
    }

    const feriados = this.localStorageService.getItemJson(this.chave) as Array<any>;
    return this.converterParaArrayDeFeriado(feriados);
  }

  possuiFeriadoNoDia(data: Date) {
    const dataParaComparar = new Date(data.getFullYear(), data.getMonth(), data.getDate());
    const feriados = this.obterFeriados();
    const feriadosNoDia = !isNullOrUndefined(feriados) ? feriados.filter(f => {
      return f.data.getTime() === dataParaComparar.getTime();
    }) : [];

    return feriadosNoDia.length > 0;
  }

  obterProximoDiaUtil(data: Date) {
    let proximoDiaUtil = this.adicionarDias(data, 1);
    while (!this.ehDiaUtil(proximoDiaUtil)) {
      proximoDiaUtil = this.adicionarDias(proximoDiaUtil, 1);
    }
    return new Date(proximoDiaUtil);
  }

  private ehDiaUtil(data: Date) {
    return data.getDay() !== WeekDay.Saturday
      && data.getDay() !== WeekDay.Sunday
      && !this.possuiFeriadoNoDia(data);
  }

  private adicionarDias(data: Date, dias: number) {
    const dataCalculada = data;
    dataCalculada.setDate(dataCalculada.getDate() + dias);
    return dataCalculada;
  }
}
