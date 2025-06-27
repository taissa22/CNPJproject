import { Combobox } from './../../shared/interfaces/combobox';
import { Observable, of } from 'rxjs';

import { TipoProcessoService } from './../../core/services/sap/tipo-processo.service';
import { TipoDeProcedimento } from '@manutencao/models/tipo-de-procedimento';

import { Page } from '@shared/types/page';
import { Sort } from '@shared/types/sort';
import { QueryResult } from '@shared/types/query-result';
import { List } from 'linqts';
import { environment } from '@environment';
import { EventEmitter } from '@angular/core';

import { take } from 'rxjs/operators';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { Injectable } from '@angular/core';
import { TipoProcesso } from '@core/models/tipo-processo';



const mockList: List<TipoDeProcedimento> = new List<TipoDeProcedimento>();
const codProcessoList: Array<number> = [3,4,6,12,14];
const nomeProcessoList: Array<string> = ['Administrativo', 'Tributário Administrativo', 'Trabalhista Administrativo', 'Cível Administrativo', 'Criminal Administrativo'];

const codTipoParticipacaoList: Array<number> = [1, 5, 6, 9, 10, 18, 22];
const nomeTipoParticipacaoList: Array<string> = ['AUTOR', 'AUTUANTE', 'AUTUADO', 'DENUNCIANTE', 'DENUNCIADO', 'IMPUGNADO', 'DESTINATARIO' ];

for (let i = 0; i < 50; i++) {
  const processoId = Math.round(Math.random() * 4);
  const tipoParticipacaoId = Math.round(Math.random() * 6);
  const tipoParticipacaoId2 = Math.round(Math.random() * 6);
  const ehOrgao1 = Math.random() < 0.5;
  const ehOrgao2 = ehOrgao1? false : Math.random() < 0.5;
  const ehPoloPassivoUnico = ehOrgao2 ? false : Math.random() < 0.5;
  const indAtivo = Math.random() < 0.8;
  mockList.Add(
    TipoDeProcedimento.fromObj({
      codigo: i + 1,
      descricao: 'Tipo de Procedimento ' + (i + 1),
      indAtivo: indAtivo,
      tipoDeParticipacao1: {codigo: codTipoParticipacaoList[tipoParticipacaoId], descricao: nomeTipoParticipacaoList[tipoParticipacaoId]},
      indOrgao1: ehOrgao1,
      tipoDeParticipacao2: {codigo: codTipoParticipacaoList[tipoParticipacaoId2], descricao: nomeTipoParticipacaoList[tipoParticipacaoId2]},
      indOrgao2: ehOrgao2,
      tipoProcesso:  {id: codProcessoList[processoId], nome: nomeProcessoList[processoId]},
      indAdministrativo: processoId === 3,
      indCivelAdministrativo: processoId === 12,
      indCriminalAdministrativo: processoId === 14,
      indTributario: processoId === 4,
      indTrabalhistaAdm: processoId === 6,
      indPoloPassivoUnico: ehPoloPassivoUnico,
      indProvisionado: Math.random() < 0.5
    })
  );
}

console.log(mockList);

@Injectable({
  providedIn: 'root'
})

export class TipoDeProcedimentoServiceMock {




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
  ): Observable<QueryResult<TipoDeProcedimento>> {
    let query: List<TipoDeProcedimento> = mockList.Where(x =>
      x.descricao.includes(descricao)
    );

    query = query.Where(x => x.tipoDeProcesso.id === tipoDeProcesso);

    if (sort && sort.direction === 'desc') {
      query = query.OrderBy(x => x.codigo);
    }

    if (sort) {
      console.log(sort);
      
      let sortExpression: (x: TipoDeProcedimento) => any = x => x.codigo;

      if (sort.column === 'codigo') {
        sortExpression = (x: TipoDeProcedimento) => x.codigo;
      }

      if (sort.column === 'descricao') {
        sortExpression = (x: TipoDeProcedimento) => x.descricao;
      }

      if (sort.column === 'provisionado') {
        sortExpression = (x: TipoDeProcedimento) => x.indProvisionado;
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

  // async incluir(descricao: string, tipoParticipacao1: {id: number, descricao: string}, 
  //   indAtivo: boolean, indOrgao1: boolean, tipoParticipacao2: {id: number, descricao: string}, 
  //   indOrgao2: boolean, codTipoProcesso: number, indPoloPassivoUnico: boolean, 
  //   indProvisionado: boolean): Promise<void> {
  //   mockList.Add(
  //     TipoDeProcedimento.fromObj({
  //       codigo: mockList.Count() + 1, 
  //       descricao: descricao,
  //       indAtivo: indAtivo,
  //       tipoDeParticipacao1: tipoParticipacao1,
  //       indOrgao1: indOrgao1,
  //       tipoDeParticipacao2: tipoParticipacao2,
  //       indOrgao2: indOrgao2,             
  //       indPoloPassivoUnico: indPoloPassivoUnico,
  //       indProvisionado: indProvisionado  
  //     })
  //   );
  // }

  // async alterar(codigo: number, descricao: string, tipoParticipacao1: {id: number, descricao: string}, indAtivo: boolean,
  //   indOrgao1: boolean, tipoParticipacao2: {id: number, descricao: string}, indOrgao2: boolean, 
  //   indPoloPassivoUnico: boolean, indProvisionado: boolean): Promise<void> {
  //   const toChange: TipoDeProcedimento = mockList.Single(
  //     x => x.codigo === codigo
  //   );
  //   mockList.Remove(toChange);
  //   mockList.Add(
  //     TipoDeProcedimento.fromObj({
  //       codigo: codigo,
  //       descricao: descricao,
  //       indAtivo: indAtivo,
  //       tipoDeParticipacao1: tipoParticipacao1,
  //       indOrgao1: indOrgao1,
  //       tipoDeParticipacao2: tipoParticipacao2,
  //       indOrgao2: indOrgao2,
  //       indPoloPassivoUnico: indPoloPassivoUnico,
  //       indProvisionado: indProvisionado
  //     })
  //   );
  // }

  async excluir(codigo: number): Promise<void> {
    const toRemove: TipoDeProcedimento = mockList.Single(
      x => x.codigo === codigo
    );
    mockList.Remove(toRemove);
  }


  getTipoProcesso(): Array<TipoProcesso> {

    let tiposDeProcesso: Combobox[] = [
      {
        id: TipoProcessoEnum.administrativo,
        descricao: "Administrativo"
      }, {
        id: TipoProcessoEnum.civelAdministrativo,
        descricao: "Cível Administrativo"
      }, {
        id: TipoProcessoEnum.criminalAdministrativo,
        descricao: "Criminal Administrativo"
      }, {
        id: TipoProcessoEnum.trabalhistaAdministrativo,
        descricao: "Trabalhista Administrativo"
      }, {
        id: TipoProcessoEnum.tributarioAdministrativo,
        descricao: "Tributário Administrativo"
      }
    ]
    this.tipoProcessoCombo.emit(tiposDeProcesso)
    return(tiposDeProcesso);

    // this.tipoProcessoService.getTiposProcesso('t')
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
