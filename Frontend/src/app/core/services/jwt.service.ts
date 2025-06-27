import { Injectable } from '@angular/core';


@Injectable()
export class JwtService {


  signOut(): void {
    window.localStorage.removeItem('RefreshToken');
    window.localStorage.removeItem('LoginAtual');
    window.localStorage.removeItem('jwtToken');
  }

  getToken(): string {
    return window.localStorage.jwtToken;
  }

  saveToken(token: string) {
    window.localStorage.jwtToken = token;
  }

  destroyToken() {
    window.localStorage.removeItem('jwtToken');
  }

  saveTokenByCrossSite(): boolean {
    if(this.getToken()) {
      this.saveToken(this.getToken());
      return true;
    }

    return false;
  }

  getJwtUsuario(): string {
    return window.localStorage.LoginAtual;
  } 

  saveJwtUsuario(usuario:string): string {
    return window.localStorage.LoginAtual = usuario;
  } 

  saveRefreshToken(token: string): void {
    window.localStorage.removeItem('RefreshToken');
    window.localStorage.RefreshToken = token;
  }

  getRefreshToken(): string | null {
    return window.localStorage.RefreshToken;
  }


}
