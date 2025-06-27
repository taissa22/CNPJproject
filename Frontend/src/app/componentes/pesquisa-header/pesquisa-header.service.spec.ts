import { TestBed } from '@angular/core/testing';

import { PesquisaHeaderService } from './pesquisa-header.service';

describe('PesquisaHeaderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PesquisaHeaderService = TestBed.get(PesquisaHeaderService);
    expect(service).toBeTruthy();
  });
});
