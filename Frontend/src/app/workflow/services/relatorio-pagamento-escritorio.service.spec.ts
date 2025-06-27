/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { RelatorioPagamentoEscritorioService } from './relatorio-pagamento-escritorio.service';

describe('Service: RelatorioPagamentoEscritorio', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RelatorioPagamentoEscritorioService]
    });
  });

  it('should ...', inject([RelatorioPagamentoEscritorioService], (service: RelatorioPagamentoEscritorioService) => {
    expect(service).toBeTruthy();
  }));
});
