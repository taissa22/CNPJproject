import { Observable, of } from 'rxjs';

import { TipoDeParticipacao } from '@manutencao/models/tipo-de-participacao';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment';''

const mockList: List<TipoDeParticipacao> = new List<TipoDeParticipacao>();

for (let i = 0; i < 20; i++) {
  mockList.Add(
    TipoDeParticipacao.fromObj({
      codigo: i + 1,
      descricao: 'Tipo de Participacao ' + (i + 1)
    })
  );
}

export class TipoDeParticipacaoServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'TipoDeParticipacaoServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    descricao: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<TipoDeParticipacao>> {
    let query: List<TipoDeParticipacao> = mockList.Where(x =>
      x.descricao.includes(descricao)
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.codigo);
    }

    if (sort) {
      console.log(sort);
      let sortExpression: (x: TipoDeParticipacao) => any = x => x.codigo;

      if (sort.column === 'descricao') {
        sortExpression = (x: TipoDeParticipacao) => x.descricao;
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
      TipoDeParticipacao.fromObj({
        codigo: mockList.Count() + 1,
        descricao: descricao
      })
    );
  }

  async alterar(codigo: number, descricao: string): Promise<void> {
    const toChange: TipoDeParticipacao = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toChange);
    mockList.Add(
      TipoDeParticipacao.fromObj({
        codigo: codigo,
        descricao: descricao
      })
    );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDeParticipacao = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toRemove);
  }
}
