import { HelperAngular } from '@shared/helpers/helper-angular';
import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '@environment';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { ParametizacaoClosing } from '@manutencao/models/parametrizacao.closing.model';
import { HttpErrorResult } from '@core/http';
import { map } from 'rxjs/operators';
import { Escritorio } from 'src/app/processos/agendaDeAudienciasDoCivelEstrategico/models';

@Injectable({
  providedIn: 'root'
})
export class ManutencaoParametrizacaoClosingService{
  private readonly href: string = `${environment.api_v2_url}/parametrizacao-closing`;

  constructor(
      private messageService: HelperAngular,
      private fb: FormBuilder,
      private api: HttpClient,
      private exportarService: TransferenciaArquivos,
      private http: HttpClient) { }

  atualizar(parametizacaoClosing: ParametizacaoClosing): Observable<any> {
    return this.http.put(`${this.href}/atualizar`, parametizacaoClosing);
  }

  obter(): Observable<ParametizacaoClosing[]> {
    try {
      return this.http
      .get<ParametizacaoClosing[]>(`${this.href}/obter`)
    }
    catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  obterEscritorios(): Observable<Escritorio[]> {
    try {
      return this.http
      .get<Escritorio[]>(`${this.href}/obter-escritorio`)
    }
    catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(): Promise<void> {
    try {
      const response: any = await this.http.get(`${this.href}/exportar`, this.exportarService.downloadOptions).toPromise();
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

}