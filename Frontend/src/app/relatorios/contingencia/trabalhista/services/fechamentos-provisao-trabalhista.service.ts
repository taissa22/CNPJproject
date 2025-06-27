import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { environment } from '@environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FechamentosProvisaoTrabalhistaService {
  private readonly href: string = `${environment.api_url}/contingencia/fechamento/provisao-trabalhista`;

  constructor(private http: HttpClient) {}

  obterTodos(
    semExclusao: boolean,
    percentual: boolean,
    desvioPadrao: boolean
  ): Observable<Fechamento[]> {
    const url: string = `${this.href}?semExclusao=${semExclusao}&percentual=${percentual}&desvioPadrao=${desvioPadrao}`;
    return this.http.get<Fechamento[]>(url).pipe(
      map(x => {
        x.forEach(f => (f.dataFechamento = new Date(f.dataFechamento)));
        return x;
      })
    );
  }

  obterFechamentoDetalhado(fechamento: number): Promise<FechamentoDetalhado> {
    const url: string = `${this.href}/${fechamento}`;
    return this.http
      .get<FechamentoDetalhado>(url)
      .pipe(
        map(x => {
          x.dataFechamento = new Date(x.dataFechamento);
          return x;
        })
      )
      .toPromise();
  }

  obterEmpresasDaCentralizadora(centralizadora: number): Promise<Array<EmpresaDoGrupo>> {
    const url: string = `${this.href}/empresas-da-centralizadora/${centralizadora}`;
    return this.http
      .get<Array<EmpresaDoGrupo>>(url)
      .toPromise();
  }

  obterAnteriores(
    fechamento: number,
    centralizadoras: Array<number>,
    semExclusao: boolean,
    percentual: boolean,
    desvioPadrao: boolean
  ): Observable<Fechamento[]> {
    const url: string = `${this.href}/anteriores/${fechamento}?centralizadoras=${centralizadoras.join('&centralizadoras=')}&semExclusao=${semExclusao}&percentual=${percentual}&desvioPadrao=${desvioPadrao}`;
    return this.http.get<Fechamento[]>(url).pipe(
      map(x => {
        x.forEach(f => (f.dataFechamento = new Date(f.dataFechamento)));
        return x;
      })
    );
  }
}

declare interface Fechamento {
  id: number;
  dataFechamento: Date;
  numeroDeMeses: number;
  tipoDeOutliers: {
    id: number;
    descricao: string;
  };
}

declare interface FechamentoDetalhado extends Fechamento {
  centralizadoras: Array<{ codigo: number; nome: string }>;
}

declare interface EmpresaDoGrupo {
  id: number;
  nome: string;
}
