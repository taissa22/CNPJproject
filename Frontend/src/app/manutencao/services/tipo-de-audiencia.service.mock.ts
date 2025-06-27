import { Observable, of } from 'rxjs';

import { TipoDeAudiencia } from '@manutencao/models/tipo-de-audiencia';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment';

const mockList: List<TipoDeAudiencia> = new List<TipoDeAudiencia>();

for (let i = 0; i < 20; i++) {
  mockList.Add(
    TipoDeAudiencia.fromObj({
      codigo: i + 1,
      descricao: 'Tipo de Audiencia ' + (i + 1),
      ativo: true,
      sigla: 'TAUD' + (i + 1),
      tipoDeProcesso: 'Tipo de Processo'
    })
  );
}

export class TipoDeAudienciaServiceMock {
  constructor() {
    if (environment.production) {
      throw new Error(
        'TipoDeAudienciaServiceMock is supposed to be used in development environment'
      );
    }
  }
  obter(
    descricao: string,
    tipoDeProcesso: string,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<TipoDeAudiencia>> {
    let query: List<TipoDeAudiencia> = mockList.Where(x =>
      x.descricao.includes(descricao)
    );

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.codigoTipoAudiencia);
    }

    if (sort) {
      console.log(sort);
      let sortExpression: (x: TipoDeAudiencia) => any = x => x.codigoTipoAudiencia;

      if (sort.column === 'descricao') {
        sortExpression = (x: TipoDeAudiencia) => x.descricao;
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

  async incluir(
    descricao: string,
    ativo: boolean,
    sigla: string,
    tipoDeProcesso: string,
    idEstrategico?:number,
    idConsumidor?:number
  ): Promise<void> {
    const obj = {
      descricao: descricao,
      ativo: ativo,
      sigla: sigla,
      tipoDeProceso: tipoDeProcesso,
      IdEstrategico: idEstrategico,
      IdConsumidor: idConsumidor
    };
    mockList.Add(
      TipoDeAudiencia.fromObj({
        codigo: mockList.Count() + 1,
        descricao: descricao,
        ativo: ativo,
        sigla: sigla,
        tipoDeProcesso: tipoDeProcesso,
        IdEstrategico: idEstrategico,
        IdConsumidor: idConsumidor
      })
    );
  }

  async alterar(
    codigo: number,
    descricao: string,
    ativo: boolean,
    sigla: string,
    tipoDeProcesso: string,
    idEstrategico?:number,
    idConsumidor?:number
  ): Promise<void> {
    const toChange: TipoDeAudiencia = mockList.Single(x => x.codigoTipoAudiencia === codigo);
    mockList.Remove(toChange);
    mockList.Add(
      TipoDeAudiencia.fromObj({
        codigo: codigo,
        descricao: descricao,
        ativo: ativo,
        sigla: sigla,
        tipoDeProcesso: tipoDeProcesso,
        IdEstrategico: idEstrategico,
        IdConsumidor: idConsumidor
      })
    );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDeAudiencia = mockList.Single(x => x.codigoTipoAudiencia === codigo);
    mockList.Remove(toRemove);
  }
}
