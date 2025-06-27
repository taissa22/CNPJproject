import { Injectable, Injector, isDevMode } from '@angular/core'; 
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';

import { JwtService, UserService } from '../services';
import { catchError, switchMap } from 'rxjs/operators';

@Injectable()
export class HttpTokenInterceptor implements HttpInterceptor {
  constructor(private jwtService: JwtService, private userService: UserService) { }

  private isRefreshing = false;
  //private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const headersConfig = {};

    headersConfig['Authorization'] = '';

    if (!(req.url.toLocaleLowerCase().search('upload') > 0)) {
      headersConfig['Content-Type'] = 'application/json';
      headersConfig['Accept'] = 'application/json';
    } else {
      headersConfig['Accept'] = '*/*';
    }

    const token = this.jwtService.getToken();

    req = this.addToken(req, token, headersConfig);

    return next.handle(req).pipe(catchError((error: any) => {
      if (error instanceof HttpErrorResponse && error.status === 401 && !this.isRefreshing) {
        return this.handle401Error(req, next);
      } else {
        return throwError(error);
      }
    }));
  }

  private addToken(request: HttpRequest<any>, token: string, headersConfig: any = null) {
    if(headersConfig === null){
      headersConfig['Content-Type'] = 'application/json';
      headersConfig['Accept'] = 'application/json';
    }

    if (token != '') {
      headersConfig.Authorization = `Bearer ${token}`;
      return request.clone({ setHeaders: headersConfig });
    }
    return request.clone({ setHeaders: headersConfig });
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.isRefreshing = true;
    const refreshToken = this.jwtService.getRefreshToken();
    const usuario = this.jwtService.getJwtUsuario();

    if (usuario != undefined) {
      return this.handleRefreshAuth(request, next, refreshToken);
    } else if (this.userService.getCurrentUser().username != undefined) {
      return this.handleRefreshAuth(request, next, refreshToken);
    } else {
      return next.handle(this.addToken(request, ''));
    }
  }

  private handleRefreshAuth(request: HttpRequest<any>, next: HttpHandler, refreshToken: string): Observable<HttpEvent<any>> {
    let usuario = this.jwtService.getJwtUsuario();

    if (usuario == undefined) {
      usuario = this.userService.getCurrentUser().username;
    }

    if (!refreshToken || !usuario) {
      return next.handle(this.addToken(request, ''));
    }

    return this.userService.userAuthenticationRefreshToken(usuario, refreshToken).pipe(
      switchMap((data: any) => {
        this.isRefreshing = false;
        if (data.sucesso) {
          this.jwtService.saveToken(data.data.access_token);
          this.jwtService.saveRefreshToken(data.data.refresh_token);
          return next.handle(this.addToken(request, data.data.access_token));
        } else {
          this.userService.purgeAuth();
        }
      }));
  }
}
