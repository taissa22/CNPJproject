/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { EmpresaContratadaService } from './empresa-contratada.service';

describe('Service: EmpresaContratada', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EmpresaContratadaService]
    });
  });

  it('should ...', inject([EmpresaContratadaService], (service: EmpresaContratadaService) => {
    expect(service).toBeTruthy();
  }));
});
