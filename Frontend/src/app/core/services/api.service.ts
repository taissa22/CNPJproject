import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import Swal from 'sweetalert2';

import { JwtService } from './jwt.service';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ApiService {
  constructor(
    private http: HttpClient,
    private jwtService: JwtService
  ) { }

  private formatErrors(error: any) {
    Swal.fire({
      title: 'Ocorreu um erro',
      html: 'Ocorreu um erro no sistema, por favor entre em contato com o administrador do sistema.',
      icon: 'warning',
      confirmButtonColor: '#6F62B2',
      confirmButtonText: 'OK'
    });
    return throwError(error.error);
  }

  get(path: string, options?: any): Observable<any> {
    return this.http.get(`${environment.api_url}${path}`, options)
      .pipe(catchError(this.formatErrors));
  }

  getv2(path: string, options?: any): Observable<any> {
    return this.http.get(`${environment.api_v2_url}${path}`, options)
      .pipe(catchError(this.formatErrors));
  }

  // tslint:disable-next-line: ban-types
  put(path: string, body: Object = {}): Observable<any> {
    return this.http.put(
      `${environment.api_url}${path}`,
      JSON.stringify(body)
    ).pipe(catchError(this.formatErrors));
  }

  post<T = any>(path: string, body: Object = {}): Observable<T> {
    return this.http.post<T>(
      `${environment.api_url}${path}`,
      body
    ).pipe(catchError(this.formatErrors));
  }


  upload(path: string, body: FormData): Observable<any> {
    // let reqHeader = new HttpHeaders({ 'Content-Type': 'application/x-www-urlencoded' });
    return this.http.post(
      `${environment.api_url}${path}`,
      body/*,
      { headers: reqHeader }*/
    ).pipe(catchError(this.formatErrors));
  }

  delete(path): Observable<any> {
    return this.http.delete(
      `${environment.api_url}${path}`
    ).pipe(catchError(this.formatErrors));
  }
}
