import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Sort } from '../../shared/types/sort';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';

import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { QueryResult } from '@shared/types/query-result';
import { HttpErrorResult } from '@core/http';
import { DatePipe} from '@angular/common';
import { Inject } from '@angular/core';
import { Esfera } from '@manutencao/models/esfera.model';
import { List } from 'linqts';
import { ProcessoInconsistente } from '@manutencao/models/processo-Inconsistente.model';
import { MotivoProvavelZero } from '@manutencao/models/motivo-provavel-zero';

@Injectable({
  providedIn: 'root'
})
export class MotivoProvavelZeroService {
  constructor(@Inject(LOCALE_ID) private _locale:string, 
  private http: HttpClient, 
  private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/motivo-provavel-zero`;

  public obterPaginado(
    pagina: number,
    quantidade: number,  
    coluna: 'id' | 'descricao',
    direcao: 'asc' | 'desc'
  ):Observable<{total:number,data:Array<MotivoProvavelZero>}>{
      let url: string = `${this.href}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;      
      return this.http.get<{ data: MotivoProvavelZero[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => MotivoProvavelZero.fromObj(e))
            };
          })
        );
  }
  
  
  async incluir(descricao : string): Promise<void> {
    const obj = {descricao : descricao};
    try {      
      await this.http.post(`${this.href}`, obj).toPromise(); 
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
   }
  } 


  async excluir(id : number): Promise<void> {
    try {
      await this.http.delete<void>(`${this.href}${'/'+id}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  
  async atualizar(id : number, descricao : string): Promise<void>{
    const obj = {Id : id, descricao : descricao};
    try {
     await this.http.put(this.href,obj).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }


  async exportar(coluna: 'id' | 'descricao' ,
                 direcao: 'asc' | 'desc'): Promise<void> {
    try {
      let url = `${this.href}/exportar?coluna=${coluna}&direcao=${direcao}`;     
      const response: any = await this.http
        .get(url, this.exportarService.downloadOptions)
        .toPromise();
      const file = response.body;
      const filename = this.exportarService.obterNomeArquivo(response);
      this.exportarService.download(file, filename);
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }


  
}
