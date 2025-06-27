import { Injectable } from '@angular/core';

import { UserService } from '..';

@Injectable({
  providedIn: 'root'
})
export class PermissoesAlteracaoProcessoBlocoWebService {

  public permissoes: any;

  constructor(
    private userService: UserService
  ) {
    this.permissoes = this.userService.getCurrentUser().permissoes;
  }

  verificarPermissao(permissao: string) {
    return this.userService.getCurrentUser().permissoes.find(item => item === permissao) !== undefined;
  }

  get f_AlteraProcBlocoWebJEC(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebJEC');
  }

  get f_AlteraProcBlocoWebAdm(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebAdm');
  }

  get f_AlteraProcBlocoWebCC(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebCC');
  }

  get f_AlteraProcBlocoWebCE(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebCE');
  }

  get f_AlteraProcBlocoWebPEX(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebPEX');
  }
  get f_AlteraProcBlocoWebTrab(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebTrab');
  }

  get f_AlteraProcBlocoWebTribJud(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebTribJud');
  }

  get f_AlteraProcBlocoWebProcon(): boolean {
    return this.verificarPermissao('f_AlteraProcBlocoWebProcon');
  }
}
