import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HttpErrorResult } from '@core/http';
import { environment } from '@environment';
import { Estado } from '@manutencao/models/estado.model';
import { TransferenciaArquivos } from '@shared/services/transferencia-arquivos.service';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: "root"
})
export class EstadoService {
  
  private urlBase: string = environment.api_url + '/manutencao/estado';

  constructor(private http : HttpClient,private exportarService: TransferenciaArquivos) { }


  public obterPaginado(
    pagina: number,
    quantidade: number,
    estadoId: string,
    coluna: 'sigla' | 'nome' | 'taxaJuros',
    direcao: 'asc' | 'desc'
  ):Observable<{total:number,data:Array<Estado>}>{
      let url: string = `${this.urlBase}/?pagina=${pagina}&quantidade=${quantidade}&estadoId=${estadoId}&coluna=${coluna}&direcao=${direcao}`;

      return this.http.get<{ data: Estado[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => Estado.fromObj(e))
            };
          })
        );
  }

  public obterTodos():Observable<Array<Estado>>{
      let url: string = `${this.urlBase}/ObterTodos`;
      return this.http.get<Estado[]>(url)
        .pipe(map(x =>  x.map(e => Estado.fromObj(e))));
  }

  async exportar(
    coluna: 'sigla' | 'nome' | 'taxaJuros',
    direcao: 'asc' | 'desc',
    estadoId: string ): Promise<void> {
    try {
      let url = `${this.urlBase}/exportar/?estadoId=${estadoId}&coluna=${coluna}&direcao=${direcao}`;
   
      const response: any = await this.http.get(url, this.exportarService.downloadOptions).toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
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

  async remover(id : string): Promise<void>{
    try {
     const url: string = `${this.urlBase}/${id}`;
     await this.http.delete(url).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

}
