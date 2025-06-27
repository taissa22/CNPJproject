import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanDeactivate
} from '@angular/router';
import { Observable } from 'rxjs';
import { IFormCanDeactivate } from '@shared/formulario/form-deactive/form-deactive';


@Injectable()
export class DeactivateGuard
  implements CanDeactivate<IFormCanDeactivate> {
  canDeactivate(
    component: IFormCanDeactivate,
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {

    return component.podeDesativar();
  }
}
