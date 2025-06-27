import { Injectable } from '@angular/core';
import { NaturezaBB } from '@shared/interfaces/natureza-BB';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class BbNaturezaService extends BaseService<NaturezaBB, number> {

  endpoint = 'BBNaturezasAcoes';

}
