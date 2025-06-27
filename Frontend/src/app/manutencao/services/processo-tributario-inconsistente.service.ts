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

@Injectable({
  providedIn: 'root'
})
export class ProcessoTributarioInconsistenteService {
  constructor(@Inject(LOCALE_ID) private _locale:string, 
  private http: HttpClient, 
  private exportarService: TransferenciaArquivos) { }

  private readonly href: string = `${environment.api_url}/manutencao/processotributarioinconsistente`;

  public obterPaginado(
    pagina: number,
    quantidade: number,  
    coluna: 'id' | 'nome' | 'corrigePrincipal' | 'corrigeMultas' | 'corrigeJuros',
    direcao: 'asc' | 'desc'
  ):Observable<{total:number,data:Array<ProcessoInconsistente>}>{
      let url: string = `${this.href}/?pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;      
      return this.http.get<{ data: ProcessoInconsistente[], total: number }>(url)
        .pipe(
          map(x => {
            return {
              total: x.total,
              data: x.data.map(e => ProcessoInconsistente.fromObj(e))
            };
          })
        );
  }
  

  async excluir(): Promise<void> {
    try {
      await this.http.delete(`${this.href}`).toPromise();
    } catch (error) {
      throw HttpErrorResult.fromError(error);
    }
  }
  

  async exportar(coluna: 'id' | 'nome' | 'corrigePrincipal' | 'corrigeMultas' | 'corrigeJuros',
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
