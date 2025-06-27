import { Injectable } from '@angular/core';
import { BaseService } from '../base.service';
import { OrgaoBB } from '@shared/interfaces/orgao-bb';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BBOrgaoService extends BaseService<OrgaoBB, number> {

  endpoint = 'BBOrgaos';

  getComboboxTribunalComarca(): Observable<any>{
    return this.http.get('/BBOrgaos/CarregarFiltros');
  }

}
