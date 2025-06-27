import { Observable, of } from 'rxjs';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment'; import { TipoDePendencia } from '@manutencao/models/tipo-de-pendencia';
import { DatePipe } from '@angular/common';
''

const mockList: List<TipoDePendencia> = new List<TipoDePendencia>();

for (let i = 0; i < 20; i++) {
  mockList.Add(
    TipoDePendencia.fromObj({
      codigo: i + 1,
      descricao: 'Tipo de Pendencia ' + (i + 1)
    })
  );
}

export class TipoDePendenciaServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'TipoDePendenciaServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    descricao: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<TipoDePendencia>> {
    let query: List<TipoDePendencia> = mockList.Where(x =>
      x.descricao.toUpperCase().includes(descricao.toUpperCase())
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.id);
    }

    if (sort) {
      let sortExpression: (x: TipoDePendencia) => any = x => x.id;

      if (sort.column === 'descricao') {
        sortExpression = (x: TipoDePendencia) => x.descricao;
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

  async incluir(descricao: string): Promise<void> {
    mockList.Add(
      TipoDePendencia.fromObj({
        codigo: mockList.Count() + 1,
        descricao: descricao
      })
    );
  }

  async alterar(codigo: number, descricao: string): Promise<void> {
    const toChange: TipoDePendencia = mockList.Single(
      x => x.id === codigo
    );
    mockList.Remove(toChange);
    mockList.Add(
      TipoDePendencia.fromObj({
        codigo: codigo,
        descricao: descricao
      })
    );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDePendencia = mockList.Single(
      x => x.id === codigo
    );
    mockList.Remove(toRemove);
  }

  async exportar(search: string): Promise<void> {
    const datePipe = new DatePipe('pt-BR');

    let fileName: string = `Tipo_de_Pendencia_${datePipe.transform(new Date(), "yyyyMMdd_HHmmss")}.csv`
    let data: List<TipoDePendencia> = mockList.Where(x =>
      x.descricao.toUpperCase().includes(search.toUpperCase())
    );
    let heders = ['codigo', 'descricao'];
    let titulos = ['Código', 'Descrição']
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
