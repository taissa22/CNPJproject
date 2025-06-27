import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { NoAuthGuard } from './no-auth-guard.service';
import { SharedModule } from '../shared';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [],
  providers: [
    NoAuthGuard
  ]
})
export class AuthModule {}
