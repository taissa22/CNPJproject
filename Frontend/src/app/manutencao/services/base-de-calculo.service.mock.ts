import { Observable, of } from 'rxjs';

import { BaseDeCalculo } from '@manutencao/models/base-de-calculo';

import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment';

const mockList: List<BaseDeCalculo> = new List<BaseDeCalculo>();

mockList.Add(
  BaseDeCalculo.fromObj({
    codigo: 1,
    descricao: 'Base de Cálculo 1',
    indBaseInicial: true
  })
);

for (let i = 1; i < 20; i++) {
  mockList.Add(
    BaseDeCalculo.fromObj({
      codigo: i + 1,
      descricao: 'Base de Cálculo ' + (i + 1),
      indBaseInicial: false
    })
  );
}

export class BaseDeCalculoServiceMock {
  constructor() {
    if (environment.production) {
      // TODO: Uncomment;
      console.error(
        'BaseDeCalculoServiceMock is supposed to be used in development environment'
      );
      // throw new Error(
      //   'BaseDeCalculoServiceMock is supposed to be used in development environment'
      // );
    }
  }
  obter(
    descricao: string,
    page: Page,
    sort?: SortOf<'descricao' | 'codigo'>
  ): Observable<QueryResult<BaseDeCalculo>> {
    let query: List<BaseDeCalculo> = mockList.Where(
      x => x.descricao.includes(descricao) && !x.ehCalculoInicial
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.codigo);
    }

    if (sort) {
      let sortExpression: (x: BaseDeCalculo) => any = x => x.codigo;

      if (sort.column === 'descricao') {
        sortExpression = (x: BaseDeCalculo) => x.descricao;
      }

      if (sort.direction === 'asc') {
        query = query.OrderBy(sortExpression);
      } else {
        query = query.OrderByDescending(sortExpression);
      }
    }

    const resultList: List<BaseDeCalculo> = new List();

    resultList.Add(mockList.Single(x => x.ehCalculoInicial));

    resultList.AddRange(
      query
        .Skip(page.index * page.size - 1)
        .Take(page.size - 1)
        .ToArray()
    );

    return of({
      lista: resultList.ToArray(),
      total: query.Count() + 1
    });
  }

  async incluir(descricao: string): Promise<void> {
    mockList.Add(
      BaseDeCalculo.fromObj({
        codigo: mockList.Count() + 1,
        descricao: descricao,
        indBaseInicial: false
      })
    );
  }

  async alterar(
    codigo: number,
    descricao: string,
    ehCalculoInicial: boolean
  ): Promise<void> {
    const toChange: BaseDeCalculo = mockList.Single(x => x.codigo === codigo);

    if (!toChange.ehCalculoInicial && ehCalculoInicial) {
      mockList.Remove(mockList.Single(x => x.ehCalculoInicial));
    }

    mockList.Remove(toChange);
    mockList.Add(
      BaseDeCalculo.fromObj({
        codigo: codigo,
        descricao: descricao,
        indBaseInicial: ehCalculoInicial
      })
    );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: BaseDeCalculo = mockList.Single(x => x.codigo === codigo);
    mockList.Remove(toRemove);
  }
}
