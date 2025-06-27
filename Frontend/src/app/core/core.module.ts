import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpTokenInterceptor, LoadingHttpInterceptor } from './interceptors';

import {
  ApiService,
  AuthGuard,
  JwtService,
  UserService
} from './services';
import { CacheHttpInterceptor } from './interceptors/http.cache.interceptor';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: CacheHttpInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpTokenInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingHttpInterceptor, multi: true },
    ApiService,
    AuthGuard,
    JwtService,
    UserService
  ],
  declarations: []
})
export class CoreModule { }
