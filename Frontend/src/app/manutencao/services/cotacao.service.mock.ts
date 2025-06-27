import { Observable, of } from 'rxjs';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment'; 
import { DatePipe } from '@angular/common';

import { Cotacao } from '@manutencao/models/cotacao.model';

const mockList: List<Cotacao> = new List<Cotacao>();

for (let i = 0; i < 20; i++) {
  // mockList.Add(
  //   // Cotacao.fromObj({
  //   //   // mesAno: 'string',
  //   //   // indice: 'indice' + (i + 1),
  //   //   // fator: 'fator'

  //   // })
  // );
}

export class CotacaoServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'CotacaoServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    indice: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<Cotacao>> {
    let query: List<Cotacao> = null;//mockList.Where(x =>
      // x.indice.toUpperCase().includes(indice.toUpperCase())
    //);

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.indice);
    }

    if (sort) {
      let sortExpression: (x: Cotacao) => any = x => x.indice;

      if (sort.column === 'indice') {
        sortExpression = (x: Cotacao) => x.indice;
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

  // async incluir(indice: string, mesAno: string, fator: string ): Promise<void> {
  //   mockList.Add(
  //     Cotacao.fromObj({
  //       mesAno: mesAno,
  //       indice: indice,
  //       fator: fator
  //     })
  //   );
  // }

  // async alterar(mesAno: date, codigoIndice: number, valor: valor): Promise<void> {
  //   const toChange: Cotacao = mockList.Single(
  //     x => x.indice === indice
  //   );
  //   mockList.Remove(toChange);
  //   mockList.Add(
  //     Cotacao.fromObj({
  //       mesAno: mesAno,
  //       indice: indice,
  //       fator: fator
  //     })
  //   );
  // }

  async excluir(indice: string): Promise<void> {
    const toRemove: Cotacao = mockList.Single(
      // x => x.indice === indice
    );
    mockList.Remove(toRemove);
  }

  async exportar(search: string): Promise<void> {
    const datePipe = new DatePipe('pt-BR');

    let fileName: string = `Cotacao_${datePipe.transform(new Date(), "yyyyMMdd_HHmmss")}.csv`
    let data: List<Cotacao> = null;//mockList.Where(x =>
      // x.indice.toUpperCase().includes(search.toUpperCase())
    //);
    let heders = ['mesAno', 'fator', 'indice'];
    let titulos = ['Mês/Ano', 'Fator', 'Índice']
    let csvData = this.ConvertToCSV(data.ToArray(), heders, titulos );
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

        line += array[i][head] + ';' ;
      }
      line = line.substring(0, line.length -1)
      str += line + '\r\n';
    }
    return str;
  }

}
