import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AcompanhamentoService {
  apiUrl = `${environment.api_v2_url}/api/CargaDeCompromisso`;   
  constructor(private http: HttpClient) {}

  obterCargaDeCompromisso(
    page?: number,
    size?: number,
    filters?: any
  ): Observable<any> {
    let url = `${this.apiUrl}/Obter?page=${page}&size=${size}`;

    if (filters) {
      Object.keys(filters).forEach(key => {
        const value = filters[key];
        if (value !== null && value !== undefined && value !== '') {
          url += `&${key}=${encodeURIComponent(value)}`;
        }
      });
    }

    return this.http.get<any>(url);
  }

  obterClasseCredito(): Observable<any> {
    let url = `${this.apiUrl}/obter-classecredito`; 
    return this.http.get<any>(url);
  }

  obterAgendamentos(
    usuario: string = '',
    solicitacaoDe?: string,
    solicitacaoAte?: string,
    campoPesquisa? : string,
    tipoPesquisa? : string

  ): Observable<any> {
    let url = `${this.apiUrl}/ObterAgendamentosExportacao`;
    
    // Verificar se existe pelo menos um filtro
    const filters = {
      pesquisa: usuario || '',
      solicitacaoDe: solicitacaoDe || '',
      solicitacaoAte: solicitacaoAte || '',
      campoPesquisa : campoPesquisa || '',
      tipoPesquisa : tipoPesquisa || ''
    };
  
    // Iniciar a URL com "?" se houver algum filtro
    const hasFilters = Object.values(filters).some(value => value !== null && value !== undefined && value !== '');
  
    if (hasFilters) {
      url += '?';
      Object.keys(filters).forEach((key, index) => {
        const value = filters[key];
        if (value !== null && value !== undefined && value !== '') {
          // Adicionar os parâmetros com "&" apenas a partir do segundo filtro
          url += `${index > 0 ? '&' : ''}${key}=${encodeURIComponent(value)}`;
        }
      });
    }
  
    return this.http.get<any>(url);
  }
   


  agendarCompromisso( 
    filters?: any
  ): Observable<any> {
    let url = `${this.apiUrl}/Novo?`;

    if (filters) {
      Object.keys(filters).forEach(key => {
        const value = filters[key];
        if (value !== null && value !== undefined && value !== '') {
          url += `&${key}=${encodeURIComponent(value)}`;
        }
      });
    }

    return this.http.get<any>(url);
  }



  // agendarCompromisso(payload: any): Observable<any> {
  //   return this.http.post<any>(`${this.apiUrl}/Novo`, payload);
  // }

  downloadAgendamento(codAgendamento: number): string {
    return `${this.apiUrl}/download/${codAgendamento}`;
  }
  
  
}
