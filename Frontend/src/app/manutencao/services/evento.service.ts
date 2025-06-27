import { TransferenciaArquivos } from './../../shared/services/transferencia-arquivos.service';
import { Sort } from './../../shared/types/sort';
import { Injectable, EventEmitter, PipeTransform, LOCALE_ID } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';

import { BehaviorSubject, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';


import { environment } from '@environment';
import { Page } from '@shared/types/page';
import { HttpErrorResult } from '@core/http';
import { Inject } from '@angular/core';
import { Evento } from '@manutencao/models/evento.model';
import { TipoProcessoService } from '@core/services/sap/tipo-processo.service';
import { EventoDisponivel } from '@manutencao/models/evento-disponivel.model';
import { DualListModel } from '@core/models/dual-list.model';

@Injectable({
  providedIn: 'root'
})
export class EventoService {
  constructor(@Inject(LOCALE_ID) private _locale:string, 
              private http: HttpClient, 
              private tipoProcessoService: TipoProcessoService, 
              private exportarService: TransferenciaArquivos
            ) { }

  private endPoint: string = 'evento';

  private url: string = `${environment.api_url}/manutencao/${this.endPoint}`;

  tiposDeProcesso = []

  propriedade: string = 'descricao'
  direcao: string = 'asc'

  public obter(
    sort: Sort, page: Page, tipoprocesso: number,pesquisa : string
  ): Observable<{ data: Array<Evento>, total: number }> {
      const url: string =  `${this.url}?tipoprocesso=${tipoprocesso}&pagina=${page.index}&quantidade=${page.size}&coluna=${sort.column}&direcao=${sort.direction}&pesquisa=${pesquisa}`;

      return this.http.get<{ data: Array<Evento>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => Evento.fromObj(o)) };
          }));
  } 

  public obterDependente(coluna: string, direcao: string, pagina: number, quantidade: number, eventoId: number): 
    Observable<{ data: Array<Evento>, total: number }> {      
      const url: string = `${this.url}/obterDependente/?eventoId=${eventoId}&pagina=${pagina}&quantidade=${quantidade}&coluna=${coluna}&direcao=${direcao}`;
      return this.http.get<{ data: Array<Evento>, total: number }>(url)
        .pipe(map(x => { return {
                                  total: x.total,
                                  data: x.data.map(o => Evento.fromObj(o)) };
          }));
  } 

  public obterDisponiveis(eventoId: number): 
    Promise< Array<EventoDisponivel>> {      
      const url: string = `${this.url}/ObterDisponiveis/?eventoId=${eventoId}`;
     try {
      return this.http.get<Array<EventoDisponivel>>(url).toPromise();
     } catch (error) {      
      throw HttpErrorResult.fromError(error);
     }

  } 
  

  async incluir(obj : any): Promise<void> {    
    try {  
          
      await this.http.post(`${this.url}`, obj).toPromise();      
    } catch (error) {       
     // throw HttpErrorResult.fromError(error);
     const httpError: HttpErrorResponse = error;
     throw HttpErrorResult.fromError(error);
    }
  }

  async alterar(obj :any): Promise<void> {  
    try { 
           
      await this.http.put(`${this.url}`, obj).toPromise(); 
    } catch (error) {
      const httpError: HttpErrorResponse = error;
      throw HttpErrorResult.fromError(error);
    }
  }

  async alterarDependentes(obj : Array<DualListModel>, eventoId : number): Promise<void> {  
    try { 
      let command = {...{eventoId : eventoId},lista: obj};      
      await this.http.put(`${this.url}/atualizar-dependentes`, command).toPromise();        
      
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }

  async excluir(codigo: number, tipoProcesso: number): Promise<void> {
    try {
      await this.http.delete(`${this.url}/${codigo}/${tipoProcesso}`).toPromise();
    } catch (error) {
      console.log(error);
      throw HttpErrorResult.fromError(error);
    }
  }  

  async exportar(
    sort: Sort,   
    tipoprocesso: number,
    pesquisa? : string  ): Promise<void> {
    try {
      
      let url: string = `${this.url}/exportar/?tipoprocesso=${tipoprocesso}&coluna=${sort.column}&direcao=${sort.direction}`;
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

  async exportarDependente(
    sort: Sort,
    pesquisa? : string  ): Promise<void> {
    try {
      
      let url: string = `${this.url}/exportar-dependentes/?coluna=${sort.column}&direcao=${sort.direction}&pesquisa=${pesquisa}`;
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

  public obterDescricaoEstrategico(): 
    Promise< Array<Evento>> {      
      const url: string = `${this.url}/ObterDescricaoDeParaEstrategico`;
     try {
      return this.http.get<Array<Evento>>(url).toPromise();
     } catch (error) {      
      throw HttpErrorResult.fromError(error);
     }

  } 

  public obterDescricaoConsumidor(): 
  Promise< Array<Evento>> {      
    const url: string = `${this.url}/ObterDescricaoDeParaConsumidor`;
   try {
    return this.http.get<Array<Evento>>(url).toPromise();
   } catch (error) {      
    throw HttpErrorResult.fromError(error);
   }

} 
  
  
}
