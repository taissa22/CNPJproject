import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';

@Injectable({
  providedIn: 'root'
})
export class LogProcessamentoContingenciaService {

  constructor(protected http: HttpClient) { }

  private baseUrl = environment.api_v2_url + '/api/LogProcessamentoContingencia';

  obterlog(filtro: Array<number>, ordem: string, dataInicial: string, dataFinal: string, asc: boolean, page: number, size: number) {
    let link = this.baseUrl + `/obter-log`;
    let url = ''
    if(filtro.length > 0){
      for(let i = 0; i < filtro.length; i++){
        url += `filtro=${filtro[i]}&`
      }
    }
    if(ordem){
      url += `ordem=${ordem}&`
    }
    if(dataInicial && dataFinal){
      url += `dataInicial=${dataInicial}&dataFinal=${dataFinal}&`
    }
    link += `?${url}asc=${asc}&page=${page}&size=${size}`
    return this.http.get<any>(link)
  }

  excluirProcesso(id: number) {
    let link = this.baseUrl + `/excluir-processo?id=${id}`
    return this.http.delete<any>(link)
  }
}
