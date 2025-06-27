import { TransferenciaArquivos } from '../../shared/services/transferencia-arquivos.service';
import { Sort } from '../../shared/types/sort';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';


import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';
import { Evento } from '@manutencao/models/evento.model';
import { DecisaoEvento } from '@manutencao/models/decisao-evento.model';

@Injectable({
  providedIn: 'root'
})
export class DecisaoEventoService {
  constructor(@Inject(LOCALE_ID) 
    private _locale:string, 
    private http: HttpClient, 
    private exportarService: TransferenciaArquivos) { }


  private endPoint: string = 'decisaoevento';

  private urlteste: string = `${environment.api_url}/manutencao/${this.endPoint}`;

  protected url(parametros: string = ''): string {
    return `${environment.api_url}/manutencao/${this.endPoint}/${parametros}`;
  }

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  
  public obter(coluna: string, direcao: string, pagina: number, quantidade: number, eventoId: number): 
    Observable<{ data: Array<DecisaoEvento>, total: number }> {      
      const url: string = this.url(`?eventoId=${eventoId}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`);
      return this.http.get<{ data: Array<DecisaoEvento>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => DecisaoEvento.fromObj(o)) };
          }));
  } 

  
  async incluir(eventoId: number,
                descricao: string,
                riscoPerda: boolean,
                perdaPotencial?: string,
                reverCalculo? : boolean,
                decisaoDefault? : boolean): Promise<void> {
    const obj = { eventoId: eventoId,
                  descricao: descricao,
                  riscoPerda: riscoPerda,
                  perdaPotencial: perdaPotencial,
                  reverCalculo : reverCalculo,
                  decisaoDefault : decisaoDefault};
    try {
      await this.http.post(`${this.urlteste}`, obj).toPromise();      
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(id: number,
                eventoId: number,
                descricao: string,
                riscoPerda: boolean,
                perdaPotencial?: string,
                reverCalculo? : boolean,
                decisaoDefault? : boolean): Promise<void> {
    const obj = { id : id,
                  eventoId: eventoId,
                  descricao: descricao,
                  riscoPerda: riscoPerda,
                  perdaPotencial: perdaPotencial,
                  reverCalculo : reverCalculo,
                  decisaoDefault : decisaoDefault};
    try {
      await this.http.put(`${this.urlteste}`, obj).toPromise();      
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(item: DecisaoEvento): Promise<void> {
    try {
      await this.http.delete(`${this.urlteste}/${item.id}/${item.eventoId}`).toPromise();
    } catch (error) {
      console.error(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async exportar(
    sort: Sort, 
    tipoDeProcesso?: number,
    pesquisa?: string,  ): Promise<void> {
    try {
      let url = `${this.url}/exportar/${tipoDeProcesso}?coluna=${sort.column}&direcao=${sort.direction}`;
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
