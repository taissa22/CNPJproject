import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { UserService } from '@core/services';
import { User } from '@core/models';

@Injectable({
  providedIn: 'root'
})
export class PermissoesService {

  constructor(
    private userService: UserService
  ) {
    this.userService.currentUser.subscribe(
      (userData) => {
        this.currentUserPermissions = this.userService.getCurrentUser().permissoes;
      }
    );
  }

  private currentUserPermissions: Array<string> = [];

  temPermissaoPara(permissao: string): boolean {
    return this.currentUserPermissions.indexOf(permissao) >= 0;
  }
}
