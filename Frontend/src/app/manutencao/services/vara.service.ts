import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { HttpErrorResult } from "@core/http";
import { Vara } from "@manutencao/models/vara.model";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VaraService {
  private urlBase: string = environment.api_url + '/manutencao/vara';
  constructor(private http: HttpClient) {

  }

  public obterPaginado(
    comarcaId: number,
    pagina: number,
    quantidade: number,
    coluna: "Id"|"Tipo"|"Endereco"|"EscritorioJuizado"|"TribunalDeJustica"|"VaraBB",
    direcao: 'asc' | 'desc',
    textoPesquisado? : string 
  ):Observable<{total:number,data:Array<Vara>}>{
      let url: string = `${this.urlBase}/?comarcaId=${comarcaId}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      if(textoPesquisado) url += `&pesquisa=${textoPesquisado}`;
      return this.http.get<{ data: Vara[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => Vara.fromObj(e))
            };
          })
        );
  }

  async criar(obj : any): Promise<void>{
    try {
     await this.http.post(this.urlBase,obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }
  
  async atualizar(obj : any): Promise<void>{
    try {
     await this.http.put(this.urlBase,obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }
 
  async remover(varaId : number, comarcaId : number , tipoVaraId : number): Promise<void>{
    try {
     const url: string = `${this.urlBase}/?VaraId=${varaId}&ComarcaId=${comarcaId}&TipoVaraId=${tipoVaraId}`;
     await this.http.delete(url).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }
}