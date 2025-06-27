import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ParametroJuridicoService {
  private endPoint: string = `${environment.api_url}/parametros`;

  constructor(private http: HttpClient) { }

  obter(id: string): Promise<any> {
    const url = `${this.endPoint}/${id}`;
    return this.http.get<any>(url).toPromise();
  }
}
