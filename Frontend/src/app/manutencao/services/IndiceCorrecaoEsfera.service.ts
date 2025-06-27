import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
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
import { IndiceCorrecaoEsfera } from '@manutencao/models/IndiceCorrecaoEsfera.model';
import { List } from 'linqts';
import { ProcessoInconsistente } from '@manutencao/models/processo-Inconsistente.model';

@Injectable({
  providedIn: 'root'
})
export class IndiceCorrecaoEsferaService {
  constructor(@Inject(LOCALE_ID) private _locale:string, private http: HttpClient, private tipoProcessoService: IndiceCorrecaoEsferaService, private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/indice-correcao-esfera`;

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  public obterPaginado(
    esferaId : number,
    pagina: number,
    quantidade: number,  
    coluna: 'id' | 'nome' | 'corrigePrincipal' | 'corrigeMultas' | 'corrigeJuros',
    direcao: 'asc' | 'desc'
  ):Observable<{total:number,data:Array<IndiceCorrecaoEsfera>}>{
      let url: string = `${this.href}/?esferaId=${esferaId}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;      
      return this.http.get<{ data: IndiceCorrecaoEsfera[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => IndiceCorrecaoEsfera.fromObj(e))
            };
          })
        );
  }

  
  async incluir(    
    esferaId: number, 
    dataVigencia: Date,
    indiceId : number

  ): Promise<{total:number,data:Array<ProcessoInconsistente>}> {
    const obj = { 
      esferaId: esferaId, 
      dataVigencia: dataVigencia,
      indiceId : indiceId
    };
    try {
      return await this.http.post<{total:number,data:Array<ProcessoInconsistente>}>(`${this.href}`, obj).toPromise();      
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir( esferaId: number, dataVigencia: Date, indiceId : number): Promise<{total:number,data:Array<ProcessoInconsistente>}> {

    const data = dataVigencia.toLocaleString().replace("/", "-");
    try {
      return await this.http.delete<{total:number,data:Array<ProcessoInconsistente>}>(`${this.href}/${esferaId}/${dataVigencia}/${indiceId}`).toPromise();

    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }
  
}
