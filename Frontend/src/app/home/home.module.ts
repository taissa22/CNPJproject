import { ModuleWithProviders, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { HomeComponent } from './home.component';
import { HomeAuthResolver } from './home-auth-resolver.service';
import { SharedModule } from '../shared';
import { HomeRoutingModule } from './home-routing.module';
import { MenuNovoComponent } from '@shared/layout/menu-novo/menu-novo.component';
import { NgbActiveModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    SharedModule,
    HomeRoutingModule
  ],
  declarations: [
    HomeComponent,
    MenuNovoComponent
  ],
  providers: [
    HomeAuthResolver,
    MenuNovoComponent,
    NgbActiveModal
  ]
})
export class HomeModule {}
