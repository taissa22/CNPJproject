import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';

@Injectable({
    providedIn: 'root'
})

export class BreadcrumbsService {
  constructor(protected http: HttpClient) { }

  private url() {
    return `${environment.api_v2_url}/menu/breadcrumb`;
  }

  public async nomeBreadcrumb(permissao : string): Promise<string> {
    try {
      const urlBase = this.url();
      return await this.http.get<string>(`${urlBase}?permissao=${permissao}`).toPromise();
    } 
    catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
