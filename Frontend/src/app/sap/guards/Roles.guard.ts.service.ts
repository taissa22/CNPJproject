import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from 'src/app/core';


 /**
   * @Descrição
   * Guard que implementa o CanActive e verifica se o usuário possui a função necessária para poder
   * acessar aquela determinada tela.
   * Caso o usuário não a possua, é redirecionado para a tela home da aplicação.
   * Para se usar esse guard, é necessário passar a propriedade role dentro
   * do data no sap-routing.ts.
   *
   * ```typescript
   * {
   * path: 'consulta/consultaSaldoGarantias',
   * component: ConsultaSaldoGarantiaComponent,
   * data: { role: nomeDaRole }
    *}
    * ```
   */
@Injectable()
export class RoleGuardSAPService implements CanActivate {

  constructor(public user: UserService , public router: Router) {}
  canActivate(route: ActivatedRouteSnapshot): boolean {

    const role = route.data.role;

    if (this.user.getCurrentUser().permissoes
      .find(element => element === role) === undefined) {
      this.user.purgeAuth()
      return false;
    }
    return true;
  }
}


