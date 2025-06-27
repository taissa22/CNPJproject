import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { HttpErrorResult } from "@core/http/http-error-result";
import { Comarca } from "@manutencao/models/comarca.model";
import { TransferenciaArquivos } from "@shared/services/transferencia-arquivos.service";
import { promise } from "protractor";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ComarcaService {
  private urlBase: string = environment.api_url + '/manutencao/comarca';
  constructor(private http: HttpClient,private exportarService: TransferenciaArquivos) {

  }

  public obterPaginado(
    pagina: number,
    quantidade: number,
    codigoEstado: string,
    coluna: 'id' | 'nome' | 'estadoId' | 'comarcaBBId',
    direcao: 'asc' | 'desc',
    pesquisa?: string
  ):Observable<{total:number,data:Array<Comarca>}>{
      let url: string = `${this.urlBase}/?pagina=${pagina}&quantidade=${quantidade}&codigoEstado=${codigoEstado}&coluna=${coluna}&direcao=${direcao}`;
      if(pesquisa) {
         url+= `&pesquisa=${pesquisa}`;
      }
      return this.http.get<{ data: Comarca[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => Comarca.fromObj(e))
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

  async remover(id : number): Promise<void>{
    try {
     const url: string = `${this.urlBase}/${id}`;
     await this.http.delete(url).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(
    coluna: 'id' | 'nome' | 'estadoId' | 'comarcaBBId',
    direcao: 'asc' | 'desc',
    estadoId: string,
    pesquisa: string  ): Promise<void> {
    try {
      let url = `${this.urlBase}/exportar/?coluna=${coluna}&direcao=${direcao}&estadoId=${estadoId}`;
      if (pesquisa) {
        url = `${url}&pesquisa=${pesquisa}`
      }
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

}
