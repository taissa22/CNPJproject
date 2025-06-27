import { Observable, of } from 'rxjs';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment'; 
import { DatePipe } from '@angular/common';

import { Indice } from '@manutencao/models/indice';

const mockList: List<Indice> = new List<Indice>();

for (let i = 0; i < 20; i++) {
  mockList.Add(
    Indice.fromObj({

      id: i + 1,
      descricao: 'string'+ (i + 1),
      codigoTipoIndice: 'string',
      codigoValorIndice: 'string',
      acumulado: false,
      acumuladoAutomatico: false,

      // id:
      // mesAno: 'string',
      // indice: 'indice' + (i + 1),
      // fator: 'fator'

    })
  );
}

export class IndiceServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'IndiceServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    indice: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<Indice>> {
    let query: List<Indice> = mockList.Where(x =>
      x.descricao.toUpperCase().includes(indice.toUpperCase())
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.id);
    }

    if (sort) {
      let sortExpression: (x: Indice) => any = x => x.id;

      if (sort.column === 'indice') {
        sortExpression = (x: Indice) => x.id;
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

  async incluir( id: number, descricao: string, codigoTipoIndice: string, codigoValorIndice: string, acumulado: boolean, acumuladoAutomatico: boolean): Promise<void> {
    mockList.Add(
      Indice.fromObj({
        id: id,
        descricao: descricao,
        codigoTipoIndice: codigoTipoIndice,
        codigoValorIndice: codigoValorIndice,
        acumulado: acumulado,
        acumuladoAutomatico: acumuladoAutomatico
      })
    );
  }

  async alterar( id: number, descricao: string, codigoTipoIndice: string, codigoValorIndice: string, acumulado: boolean, acumuladoAutomatico: boolean): Promise<void> {
    const toChange: Indice = mockList.Single(
      x => x.id === id
    );
    mockList.Remove(toChange);
    mockList.Add(
      Indice.fromObj({
        id: id,
        descricao: descricao,
        codigoTipoIndice: codigoTipoIndice,
        codigoValorIndice: codigoValorIndice,
        acumulado: acumulado,
        acumuladoAutomatico : acumuladoAutomatico
      })
    );
  }

  async excluir(id: number): Promise<void> {
    const toRemove: Indice = mockList.Single(
      x => x.id === id
    );
    mockList.Remove(toRemove);
  }

  async exportar(search: string): Promise<void> {
    const datePipe = new DatePipe('pt-BR');

    let fileName: string = `Indice_${datePipe.transform(new Date(), "yyyyMMdd_HHmmss")}.csv`
    let data: List<Indice> = null;//mockList.Where(x =>
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
