import { Injectable } from '@angular/core';
import { TribunalBB } from '@shared/interfaces/tribunalBB';
import { BaseService } from '../base.service';

@Injectable({
  providedIn: 'root'
})
export class BbtribunalService extends BaseService<TribunalBB, number>{

  endpoint = 'BBTribunais';

}
