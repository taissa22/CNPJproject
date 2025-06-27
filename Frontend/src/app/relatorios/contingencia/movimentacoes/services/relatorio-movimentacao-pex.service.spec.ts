import { TestBed } from '@angular/core/testing';

import { RelatorioMovimentacaoPexService } from './relatorio-movimentacao-pex.service';

describe('RelatorioMovimentacaoPexService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: RelatorioMovimentacaoPexService = TestBed.get(RelatorioMovimentacaoPexService);
    expect(service).toBeTruthy();
  });
});
