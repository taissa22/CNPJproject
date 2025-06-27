/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ManutencaoContratoEscritorioService } from './manutencao-contrato-escritorio.service';

describe('Service: ManutencaoContratoEscritorio', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ManutencaoContratoEscritorioService]
    });
  });

  it('should ...', inject([ManutencaoContratoEscritorioService], (service: ManutencaoContratoEscritorioService) => {
    expect(service).toBeTruthy();
  }));
});
