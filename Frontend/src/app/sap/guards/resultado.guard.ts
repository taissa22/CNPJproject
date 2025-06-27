import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';

export class ResultadoGuard implements CanActivate {

  constructor(private router: Router,
              public service: any) {

  }

  public rotaRetorno = '..';

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    const isAllowed = this.isRouteAllowed();

    if (isAllowed) {
      return true;
    } else {
      this.router.navigate([this.rotaRetorno]);
      return false;
    }
  }

  isRouteAllowed() : boolean {
    throw Error('Função não implementada.');
  }
}
