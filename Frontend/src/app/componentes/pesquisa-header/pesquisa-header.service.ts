import { JsonPipe } from '@angular/common';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})


export class PesquisaHeaderService {
  constructor(private http: HttpClient) { }

  public load() {
    var permissoesMenu = localStorage.getItem('permissoesMenu');
    if (permissoesMenu != null && permissoesMenu != "") {
      var obj = JSON.parse(permissoesMenu)
      return obj;
    } else {
      var permissoes = this.GetPermissoes();
      var str = JSON.stringify(permissoes)
      localStorage.setItem('permissoesMenu', str);
      return permissoes
    }
  }

  private GetPermissoes() {
    let auth_token = localStorage.getItem("jwtToken");
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${auth_token}`
    })
    const body = JSON.stringify(environment.s1_url);

    return this.http.post(environment.api_v2_url + '/Menu/pesquisar', body, { headers });
  }
}
