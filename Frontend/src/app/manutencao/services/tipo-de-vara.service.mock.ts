import { Observable, of } from 'rxjs';

import { TipoDeVara } from '@manutencao/models/tipo-de-vara';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment';import { DatePipe } from '@angular/common';
''



const mockList: List<TipoDeVara> = new List<TipoDeVara>();



for (let i = 0; i < 20; i++) {
  mockList.Add(
    TipoDeVara.fromObj({
      id: i + 1,
      nome: 'Nome da vara ' + (i + 1),
      eh_CivelConsumidor: Math.random() < 0.5,
      eh_CivelEstrategico: Math.random() < 0.5,
      eh_Trabalhista: Math.random() < 0.5,
      eh_Tributaria: Math.random() < 0.5,
      eh_Juizado: Math.random() < 0.5,
      eh_CriminalJudicial: Math.random() < 0.5,
      eh_Procon: Math.random() < 0.5,
    })   
  );
}

export class TipoDeVaraServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'TipoDeVaraServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    nome: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<TipoDeVara>> {
    let query: List<TipoDeVara> = mockList.Where(x =>
      x.nome.includes(nome)
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.codigo);
    }

    if (sort) {
      console.log(sort);
      let sortExpression: (x: TipoDeVara) => any = x => x.codigo;

      if (sort.column === 'nome') {
        sortExpression = (x: TipoDeVara) => x.nome;
      }

      if (sort.direction === 'asc') {
        query = query.OrderBy(sortExpression);
      } else {
        query = query.OrderByDescending(sortExpression);
      }
    }

    return of({
      lista: query
        .Skip(page.index * page.size)
        .Take(page.size)
        .ToArray(),
      total: query.Count()
    });
  }

  async incluir(nome: string, indCivel: boolean, indCivelEstrategico: boolean, 
                indTrabalhista: boolean, indTributaria: boolean, indJuizado: boolean, 
                indCriminalJudicial: boolean, indProcon: boolean): Promise<void> {
    mockList.Add(
      TipoDeVara.fromObj({
        id: mockList.Count() + 1,
        nome: nome,
        eh_CivelConsumidor: indCivel,
        eh_CivelEstrategico: indCivelEstrategico,
        eh_Trabalhista: indTrabalhista,
        eh_Tributaria: indTributaria,
        eh_Juizado: indJuizado,
        eh_CriminalJudicial: indCriminalJudicial,
        eh_Procon: indProcon,
      })
    );
  }

  async alterar(codigo: number, nome: string, indCivel: boolean, indCivelEstrategico: boolean, 
                indTrabalhista: boolean, indTributaria: boolean, indJuizado: boolean, 
                indCriminalJudicial: boolean, indProcon: boolean): Promise<void> {
    const toChange: TipoDeVara = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toChange);
    mockList.Add(
      TipoDeVara.fromObj({
        id: codigo,
        nome: nome,
        eh_CivelConsumidor: indCivel,
        eh_CivelEstrategico: indCivelEstrategico,
        eh_Trabalhista: indTrabalhista,
        eh_Tributaria: indTributaria,
        eh_Juizado: indJuizado,
        eh_CriminalJudicial: indCriminalJudicial,
        eh_Procon: indProcon,
      })
    );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDeVara = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toRemove);
  }

  async exportar(search: string): Promise<void> {
    const datePipe = new DatePipe('pt-BR');

    let fileName: string = `Tipo_Vara_${datePipe.transform(new Date(), "yyyyMMdd_HHmmss")}.csv`
    let data: List<TipoDeVara> = mockList.Where(x =>
      x.nome.toUpperCase().includes(search.toUpperCase())
      );
    let headers = ['codigo', 'nome', 'indCivel', 'indCivelEstrategico', 'indTrabalhista', 'indTributaria', 'indJuizado', 'indCriminalJudicial', 'indProcon'];
    let titulos = ['Código', 'Nome', 'Cível Consumidor','Cível Estratégico', 'Trabalhista', 'Tributária', 'Juizado', 'Criminal Judicial', 'Procon']
    let csvData = this.ConvertToCSV(data.ToArray(), headers, titulos );
    console.log(csvData)
    let blob = new Blob(['\ufeff' + csvData], { type: 'text/csv;charset=utf-8;' });
    let dwldLink = document.createElement("a");
    let url = URL.createObjectURL(blob);
    let isSafariBrowser = navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1;
    if (isSafariBrowser) {  //if Safari open in new window to save file with random filename.
      dwldLink.setAttribute("target", "_blank");
    }
    dwldLink.setAttribute("href", url);
    dwldLink.setAttribute("download", fileName);
    dwldLink.style.visibility = "hidden";
    document.body.appendChild(dwldLink);
    dwldLink.click();
    document.body.removeChild(dwldLink);

  }

  ConvertToCSV(objArray, headerList, titulos) {

    // let array = typeof objArray != 'object' ? JSON.parse(objArray) : objArray;
    let array = objArray;
    let str = '';
    let row = '';

    for (let index in titulos) {
      row += titulos[index] + ';';
    }
    row = row.slice(0, -1);
    str += row + '\r\n';
    for (let i = 0; i < array.length; i++) {
      let line:string = '';
      for (let index in headerList) {
        let head = headerList[index];

        line += array[i][head].toString().replace('true', 'Sim').replace('false', 'Não') + ';' ;
      }
      line = line.substring(0, line.length -1)
      str += line + '\r\n';
    }
    return str;
  }


}

