import { TestBed } from '@angular/core/testing';

import { RelatorioAtmPexService } from './relatorio-atm-pex.service';

describe('RelatorioAtmPexService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RelatorioAtmPexService = TestBed.get(RelatorioAtmPexService);
    expect(service).toBeTruthy();
  });
});
