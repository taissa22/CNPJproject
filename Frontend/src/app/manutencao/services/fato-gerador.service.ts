import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Sort } from '../../shared/types/sort';
import { Injectable, LOCALE_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';


import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';
import { FatoGerador } from '@manutencao/models/fato-gerador.model';

@Injectable({
  providedIn: 'root'
})
export class FatoGeradorService {
  constructor(@Inject(LOCALE_ID) private _locale:string, 
              private http: HttpClient, 
              private exportarService: TransferenciaArquivos
            ) { }

  private endPoint: string = 'fato-gerador';

  private url: string = `${environment.api_url}/manutencao/${this.endPoint}`;
  
  public obter(
    sort: Sort, page: Page, pesquisa : string
  ): Observable<{ data: Array<FatoGerador>, total: number }> {
      let url: string =  `${this.url}?pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}`;
      if (pesquisa)
      {        
        url = url + `&pesquisa=${pesquisa}`;
      }
      return this.http.get<{ data: Array<FatoGerador>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => FatoGerador.fromObj(o)) };
          }));
  } 
  

  async incluir(obj : any): Promise<void> {    
    try {  
          
      await this.http.post(`${this.url}`, obj).toPromise();      
    } catch (error) {       
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(obj :any): Promise<void> {  
    try { 
           
      await this.http.put(`${this.url}`, obj).toPromise(); 
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigo: number): Promise<void> {
    try {
      await this.http.delete(`${this.url}/${codigo}`).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }  

  async exportar(sort: Sort, pesquisa? : string): Promise<void> {
    try {
      
      let url: string = `${this.url}/exportar/?coluna=${sort.column}&direcao=${sort.direction}`;
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
