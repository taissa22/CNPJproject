import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable} from 'rxjs';
import { BBOrgaoService } from 'src/app/core/services/sap/bborgao.service';
import { pluck } from 'rxjs/operators';

@Injectable()
export class ComarcaTribunaisResolverGuard implements Resolve<any> {

  constructor(private router: Router, private orgaobbService: BBOrgaoService) {
  }

  resolve(): Observable<any> {


    return this.orgaobbService.getComboboxTribunalComarca().pipe(pluck('data'));
  }
}
