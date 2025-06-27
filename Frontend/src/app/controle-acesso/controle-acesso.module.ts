import { LOCALE_ID, NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { LogOperacoesComponent } from './usuarios/log-operacoes/log-operacoes.component';
import { ControleAcessoRoutingModule } from './controle-acesso-routing.module';
import { ComponentesModule } from '../componentes/componentes.module';
import { ProcessosModule } from '../processos/processos.module';


@NgModule({
  declarations: [
    LogOperacoesComponent
  ],
  imports: [
    CommonModule,
    ComponentesModule,
    ControleAcessoRoutingModule,
    ProcessosModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [
    DatePipe,
    { provide: LOCALE_ID, useValue: 'pt' }
  ]
})
export class ControleAcessoModule { }
