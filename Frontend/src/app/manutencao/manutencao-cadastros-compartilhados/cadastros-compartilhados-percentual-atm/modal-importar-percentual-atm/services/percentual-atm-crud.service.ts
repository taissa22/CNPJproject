import { Injectable } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { BehaviorSubject } from 'rxjs';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PercentualAtmCrudService {
  constructor(private http: HttpClient, private fb: FormBuilder) {}

  registerForm: FormGroup;
  fecharModal = new BehaviorSubject<boolean>(false);
  sucesso = new BehaviorSubject<boolean>(false);
  valoresEdicaoFormulario = new BehaviorSubject<any>(null);
  valores = new BehaviorSubject([]);
  form: FormGroup;
  private endPoint: string = 'percentual-atm';

  inicializarForm(): FormGroup {
    this.registerForm = this.fb.group({});
    return this.registerForm;
  }

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  downloadArquivoURL(): string {
    try {
      return this.url(`planilha-padrao`);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  public async existeVigencia(vigencia: string) {
    try {
      const url: string = this.url(`existeVigencia?dataVigencia=${vigencia}`);
      return await this.http.get<boolean>(url).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
}
