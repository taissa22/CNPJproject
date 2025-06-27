import { DrvDataDirective } from "./shared/directive/drv-data.directive";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { LOCALE_ID } from '@angular/core';
import { registerLocaleData, DatePipe, CommonModule } from '@angular/common';
import { ScrollingModule } from '@angular/cdk/scrolling';
import {
  NgbModule,
  NgbDatepickerModule,
  NgbTimepickerModule,
  NgbDateParserFormatter,
  NgbDate,
} from "@ng-bootstrap/ng-bootstrap";
import { NgxSpinnerModule } from "ngx-spinner";
import { AppComponent } from "./app.component";
import { AuthModule } from "./auth/auth.module";
import { HomeModule } from "./home/home.module";
import { MenuNovoComponent } from "@shared/layout/menu-novo/menu-novo.component";
import { HeaderComponent, SharedModule, MenuComponent } from "./shared";
import { AppRoutingModule } from "./app-routing.module";
import { CoreModule } from "./core/core.module";
import localePt from "@angular/common/locales/pt";
import { InputsModule } from "./shared/formulario/inputs/inputs.module";
import { AuthGuard } from "./core";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { PagamentosModule } from "./pagamentos/pagamentos.module";
import { AgendasModule } from "./processos/agendaDeAudienciasDoCivelEstrategico/agendas.module";
import { TransferenciaArquivos } from "@shared/services/transferencia-arquivos.service";
import { ProcessosModule } from './processos/processos.module';
import { RoleGuardService } from "@core/services/Roles.guard.ts.service";
//import { TabsModule } from "ngx-bootstrap";
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ModalMenuNovoComponent } from "@shared/layout/modal-menu-novo/modal-menu-novo.component";
import { InputMaskModule } from "@libs/racoon-mask/input-mask.module";
import { NgSelectModule } from "@ng-select/ng-select";
//import { CivelEstrategicoComponent } from './civel-estrategico/civel-estrategico.component';


registerLocaleData(localePt);


@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    MenuComponent,
    //MenuNovoComponent,
    ModalMenuNovoComponent,
    DrvDataDirective, 
    //CivelEstrategicoComponent,
  ],
  imports: [
    BrowserModule,
    NgbModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    CoreModule,
    SharedModule,
    HomeModule,
    AuthModule,
    AppRoutingModule,
    NgxSpinnerModule,
    InputsModule,
    BrowserAnimationsModule,
    ScrollingModule,
    BsDatepickerModule.forRoot(),
    PagamentosModule,
    AgendasModule,
    ProcessosModule,
    TabsModule.forRoot(),
    ProcessosModule,
    InputMaskModule,
  ],
  providers: [
    TransferenciaArquivos,
    AuthGuard,
    RoleGuardService,
    { provide: LOCALE_ID, useValue: 'pt-BR' },
    [DatePipe],

  ],
  entryComponents: [
    MenuNovoComponent
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
