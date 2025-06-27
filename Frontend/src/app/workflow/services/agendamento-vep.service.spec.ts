/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AgendamentoVepService } from './agendamento-vep.service';

describe('Service: AgendamentoVep', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AgendamentoVepService]
    });
  });

  it('should ...', inject([AgendamentoVepService], (service: AgendamentoVepService) => {
    expect(service).toBeTruthy();
  }));
});
