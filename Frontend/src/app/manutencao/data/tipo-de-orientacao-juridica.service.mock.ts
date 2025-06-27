import { TipoDeOrientacaoJuridica, TipoDeOrientacaoJuridicaBack } from './../models/tipo-de-orientacao-juridica';
import { Observable, of } from 'rxjs';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';

import { DatePipe } from '@angular/common';
import { environment } from '@environment';
''

const mockList: List<TipoDeOrientacaoJuridica> = new List<TipoDeOrientacaoJuridica>();

for (let i = 0; i < 20; i++) {
  mockList.Add(
    TipoDeOrientacaoJuridica.fromObj({
      id: i + 1,
      descricao: 'Tipo de Orientação Jurídica ' + (i + 1)
    })
  );
}

export class TipoDeOrientacaoJuridicaServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'TipoDeOrientacaoJuridicaServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    descricao: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<TipoDeOrientacaoJuridica>> {
    let query: List<TipoDeOrientacaoJuridica> = mockList.Where(x =>
      x.descricao.toUpperCase().includes(descricao.toUpperCase())
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.codigo);
    }

    if (sort) {
      let sortExpression: (x: TipoDeOrientacaoJuridica) => any = x => x.codigo;

      if (sort.column === 'descricao') {
        sortExpression = (x: TipoDeOrientacaoJuridica) => x.descricao;
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
      TipoDeOrientacaoJuridica.fromObj({
        id: mockList.Count() + 1,
        descricao: descricao
      })
    );
  }

  async alterar(codigo: number, descricao: string): Promise<void> {
    const toChange: TipoDeOrientacaoJuridica = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toChange);
    mockList.Add(
      TipoDeOrientacaoJuridica.fromObj({
        id: codigo,
        descricao: descricao
      })
    );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDeOrientacaoJuridica = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toRemove);
  }

  async exportar(search: string): Promise<void> {
    const datePipe = new DatePipe('pt-BR');

    let fileName: string = `Tipo_de_Orientacao_Juridica_${datePipe.transform(new Date(), "yyyyMMdd_HHmmss")}.csv`
    let data: List<TipoDeOrientacaoJuridica> = mockList.Where(x =>
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
