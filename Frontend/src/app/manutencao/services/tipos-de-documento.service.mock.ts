import { Combobox } from './../../shared/interfaces/combobox';
import { Observable, of } from 'rxjs';

import { TipoDeDocumento } from '@manutencao/models/tipos-de-documento.model';
import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment';
import { EventEmitter } from '@angular/core';

import { take } from 'rxjs/operators';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { Injectable } from '@angular/core';
''

const mockList: List<TipoDeDocumento> = new List<TipoDeDocumento>();

for (let i = 0; i < 20; i++) {
  // mockList.Add(
  //   TipoDeDocumento.fromObj({
  //     codigo: i + 1,
  //     descricao: 'Documento ' + (i + 1),
  //     ativo: i%2==0 ? 'Sim' : 'Não' ,
  //     tipoDeProcesso: 'Tipo do processo ' + (i + 1),
  //     cadastraProcesso: i%2==0 ? 'Sim' : 'Não',
  //     requerDataAudienciaPrazo: i%2==0 ? 'Sim' : 'Não',
  //     prioritarioFilaCadastroProcesso: i%2==0 ? 'Sim' : 'Não',
  //     utilizadoEmProtocolo: i%2==0 ? 'Sim' : 'Não',
  //     tipoDePrazo: 'Tipo Prazo  ' + (i + 1)
  //   })
  // );
}

@Injectable({
  providedIn: 'root'
})

export class TipoDeDocumentoServiceMock {




  constructor(
    private tipoProcessoService: TipoProcessoService
  ) {
    if (environment.production) {
      throw new Error(
        'TipoDeDocumentoServiceMock is supposed to be used in development environment'
      );
    }
  }

  tipoProcessoCombo = new EventEmitter;

  obter(
    descricao: string,
    tipoDeProcesso: number,
    page: Page,
    sort?: Sort
  ): Observable<QueryResult<TipoDeDocumento>> {
    let query: List<TipoDeDocumento> = mockList.Where(x =>
      x.descricao.includes(descricao)
    );

    // if (sort && sort.direction === 'desc') {
    //   query = query.OrderBy(x => x.codigo);
    // }

    if (sort) {
      console.log(sort);
      
      let sortExpression: (x: TipoDeDocumento) => any = x => x.codigo;

      if (sort.column === 'codigo') {
        sortExpression = (x: TipoDeDocumento) => x.codigo;
      }

      if (sort.column === 'descricao') {
        sortExpression = (x: TipoDeDocumento) => x.descricao;
      }

      if (sort.column === 'ativo') {
        sortExpression = (x: TipoDeDocumento) => x.ativo;
      }

      if (sort.column === 'tipoProcesso') {
        sortExpression = (x: TipoDeDocumento) => x.tipoDeProcesso;
      }

      if (sort.column === 'cadastraProcesso') {
        sortExpression = (x: TipoDeDocumento) => x.cadastraProcesso;
      }

      if (sort.column === 'requerDataAudienciaPrazo') {
        sortExpression = (x: TipoDeDocumento) => x.requerDataAudienciaPrazo;
      }

      if (sort.column === 'prioritarioFilaCadastroProcesso') {
        sortExpression = (x: TipoDeDocumento) => x.prioritarioFilaCadastroProcesso;
      }

      if (sort.column === 'utilizadoEmProtocolo') {
        sortExpression = (x: TipoDeDocumento) => x.utilizadoEmProtocolo;
      }

      if (sort.column === 'tipoDePrazo') {
        sortExpression = (x: TipoDeDocumento) => x.tipoDePrazo;
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

  async incluir(descricao: string, ativo: string, tipoDeProcesso: number, cadastraProcesso: string, requerDataAudienciaPrazo: string, prioritarioFilaCadastroProcesso: string, utilizadoEmProtocolo: string, tipoDePrazo: string): Promise<void> {
    // mockList.Add(
    //   TipoDeDocumento.fromObj({
    //     codigo: mockList.Count() + 1,
    //     descricao: descricao,
    //     ativo: ativo,
    //     tipoDeProcesso: tipoDeProcesso,
    //     cadastraProcesso: cadastraProcesso,
    //     requerDataAudienciaPrazo: requerDataAudienciaPrazo,
    //     prioritarioFilaCadastroProcesso: prioritarioFilaCadastroProcesso,
    //     utilizadoEmProtocolo: utilizadoEmProtocolo,
    //     tipoDePrazo: tipoDePrazo
    //   })
    // );
  }

  async alterar(codigo: number, descricao: string, ativo: string, tipoDeProcesso: number, cadastraProcesso: string, requerDataAudienciaPrazo: string, prioritarioFilaCadastroProcesso: string, utilizadoEmProtocolo: string, tipoDePrazo: string): Promise<void> {
    const toChange: TipoDeDocumento = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toChange);
    // mockList.Add(
    //   TipoDeDocumento.fromObj({
    //     codigo: codigo,
    //     descricao: descricao,
    //     ativo: ativo,
    //     tipoDeProcesso: tipoDeProcesso,
    //     cadastraProcesso: cadastraProcesso,
    //     requerDataAudienciaPrazo: requerDataAudienciaPrazo,
    //     prioritarioFilaCadastroProcesso: prioritarioFilaCadastroProcesso,
    //     utilizadoEmProtocolo: utilizadoEmProtocolo,
    //     tipoDePrazo: tipoDePrazo
    //   })
    // );
  }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDeDocumento = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toRemove);
  }


  getTipoProcesso() {

    let tiposDeProcesso: Combobox[] = [
      {
        id: TipoProcessoEnum.administrativo,
        descricao: "Administrativo"
      }, {
        id: TipoProcessoEnum.civelAdministrativo,
        descricao: "Cível Administrativo"
      }, {
        id: TipoProcessoEnum.civelConsumidor,
        descricao: "Cível Consumidor"
      }, {
        id: TipoProcessoEnum.civelEstrategico,
        descricao: "Cível Estratégico"
      }, {
        id: TipoProcessoEnum.criminalAdministrativo,
        descricao: "Criminal Administrativo"
      }, {
        id: TipoProcessoEnum.criminalJudicial,
        descricao: "Criminal Judicial"
      }, {
        id: TipoProcessoEnum.juizadoEspecial,
        descricao: "Juizado Especial"
      }, {
        id: TipoProcessoEnum.PEX,
        descricao: "Pex"
      }, {
        id: TipoProcessoEnum.procon,
        descricao: "Procon"
      }, {
        id: TipoProcessoEnum.trabalhista,
        descricao: "Trabalhista"
      }, {
        id: TipoProcessoEnum.trabalhistaAdministrativo,
        descricao: "Trabalhista Administrativo"
      }, {
        id: TipoProcessoEnum.tributarioAdministrativo,
        descricao: "Tributário Administrativo"
      }, {
        id: TipoProcessoEnum.tributarioJudicial,
        descricao: "Tributário Judicial"
      }
    ]
    this.tipoProcessoCombo.emit(tiposDeProcesso)

    // this.tipoProcessoService.getTiposProcesso('TipoDeDocumento')
    //   .pipe(take(1))
    //   .subscribe(response => {
    //     this.tipoProcessoCombo.emit(response);

    //   });

    // this.tipoProcessoService.getTodosTiposProcesso()
    //   .pipe(take(1))
    //   .subscribe(response => {
    //     this.tipoProcessoCombo.emit(response);

    //   });
  }
}
