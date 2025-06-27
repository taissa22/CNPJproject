/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ResultadoNegociacaoService } from './resultado-negociacao.service';

describe('Service: ResultadoNegociacao', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ResultadoNegociacaoService]
    });
  });

  it('should ...', inject([ResultadoNegociacaoService], (service: ResultadoNegociacaoService) => {
    expect(service).toBeTruthy();
  }));
});
