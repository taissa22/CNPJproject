import { environment } from './../../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

export abstract class ObterDadosService<T> {

  constructor(protected api: HttpClient) { }

  protected abstract endPoint: string;

  protected get url(): string {
    return environment.api_url + this.endPoint;
  }

  obterDados(): Observable<Array<T>> {
    return this.api.get<Array<T>> (this.url);
  }
}
