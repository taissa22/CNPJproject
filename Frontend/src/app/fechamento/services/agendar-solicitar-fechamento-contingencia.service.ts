import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { Observable } from 'rxjs';
import { FechamentoDto } from '../models/contigencia/agendar-solicitar-fechamento-contingencia/fechamento-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AgendarSolicitarFechamentoContingenciaService {

  constructor(protected http: HttpClient) { }

  private url = environment.api_v2_url + '/api/FechamentoContingencia'

  obterAgendamentos(page: number, modulos: number[], ordem?: string, asc?: boolean): Observable<any> {
    let link = this.url + `/obter-agendamentos?`    
    for(var i =0 ; i< modulos.length ; i++){
      link += `modulos=${modulos[i]}&`;
    }
    if(ordem != undefined){
      link += `ordem=${ordem}&asc=${asc}&`
    }
    link += `page=${page}&size=10`
    return this.http.get<any>(link)
  }
  
  obterFechamentos(page: number, modulos: number[], ordem?: string, asc?: boolean): Promise<any> {
    let link = this.url + `/obter-agendamentos?`    
    for(var i =0 ; i< modulos.length ; i++){
      link += `modulos=${modulos[i]}&`;
    }
    if(ordem != undefined){
      link += `ordem=${ordem}&asc=${asc}&`
    }
    link += `page=${page}&size=10`
    return this.http.get<any>(link).toPromise();
  }

  obterEmpresas(){
    let link = this.url + `/obter-empresas`;
    return this.http.get<any>(link)
  }

  obterEmpresasGrupos(){
    let link = this.url + `/obter-empresas-grupos`;
    return this.http.get<any>(link)
  }

  obterHaircutPadrao(modulo: number){
    let link = this.url + `/obter-haircut-padrao?modulo=${modulo}`;
    return this.http.get<any>(link)
  }

  obterValoresTrabalhista(){
    let link = this.url + `/obter-valores-trabalhista`;
    return this.http.get<any>(link)
  }

  obterValorPex(){
    let link = this.url + `/obter-valores-pex`;
    return this.http.get<any>(link)
  }

  obterValorOutlier(){
    let link = this.url + `/obter-valor-outlier`;
    return this.http.get<any>(link)
  }

  excluirAgendamento(id: number): Observable<any> {
    let link = this.url + `/excluir-agendamento?id=${id}`
    return this.http.delete<any>(link)
  }

  incluirAgendamento(model: FechamentoDto): Observable<FechamentoDto> {
    let link = this.url + `/incluir-agendamento`
    return this.http.post<FechamentoDto>(link, model);
  }

  editarAgendamento(id: number, model: any): Observable<any> {
    let link = this.url + `/editar-agendamento?id=${id}`
    return this.http.put<any>(link, model);
  }

}
