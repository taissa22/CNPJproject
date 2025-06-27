import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { Municipio } from '@manutencao/models/municipio.model';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class MunicipioService {
  
  private urlBase: string = environment.api_url + '/manutencao/municipio';

  constructor(private http : HttpClient) { }
  
  public obterPaginado(
    pagina: number,
    quantidade: number,
    estadoId: string,
    municipioId: number,
    coluna: 'codigo' | 'nome' | 'estado',
    direcao: 'asc' | 'desc'
  ):Observable<{total:number,data:Array<Municipio>}>{
      let url: string = `${this.urlBase}/?pagina=${pagina}&quantidade=${quantidade}&estadoId=${estadoId}&municipioId=${municipioId}&coluna=${coluna}&direcao=${direcao}`;

      return this.http.get<{ data: Municipio[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => Municipio.fromObj(e))
            };
          })
        );
  }

  public obterTodos(
    estadoId: string,
  ):Observable<Array<Municipio>>{
      let url: string = `${this.urlBase}/ObterTodos?estadoId=${estadoId}`;
      return this.http.get<Array<Municipio>>(url).pipe(map(m =>m.map( o => Municipio.fromObj(o))));
  }


  async atualizar(obj : any): Promise<void>{
    try {
     await this.http.put(this.urlBase,obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async criar(obj : any): Promise<void>{
    try {
     await this.http.post(this.urlBase,obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async remover(id : number): Promise<void>{
    try {
     const url: string = `${this.urlBase}/${id}`;
     await this.http.delete(url).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }
}
