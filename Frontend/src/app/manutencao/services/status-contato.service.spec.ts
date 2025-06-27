/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { StatusContatoService } from './status-contato.service';

describe('Service: StatusContato', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [StatusContatoService]
    });
  });

  it('should ...', inject([StatusContatoService], (service: StatusContatoService) => {
    expect(service).toBeTruthy();
  }));
});
