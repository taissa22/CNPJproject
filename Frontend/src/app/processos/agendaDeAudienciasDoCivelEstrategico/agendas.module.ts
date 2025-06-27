import { OrdenacaoDragDropComponent } from './../../componentes/ordenacao-drag-drop/ordenacao-drag-drop.component';
import { ModalOrdenacaoMultiColunasComponent } from './components/modal-ordenacao-multi-colunas/modal-ordenacao-multi-colunas.component';
import { ComponentesModule } from 'src/app/componentes/componentes.module';
import { TabelaOrdenacaoMultiColunasComponent } from './components/tabela-ordenacao-multi-colunas/tabela-ordenacao-multi-colunas.component';
import { InputMaskModule } from '@libs/racoon-mask/input-mask.module';;
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { TextMaskModule } from 'angular2-text-mask';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { NgxSelectModule, INgxSelectOptions } from 'ngx-select-ex';
import { NgbModule, NgbDatepickerModule, NgbTimepickerModule, NgbModalModule, NgbTabsetModule } from '@ng-bootstrap/ng-bootstrap';
import { InputsModule } from '@shared/formulario/inputs/inputs.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { SharedModule } from './../../shared/shared.module';
import { AgendaAudienciasComponent } from './pages/agenda-audiencias/agenda-audiencias.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AgendasRoutingModule } from './agendas-routing.module';
import { ModalPartesAgendaComponent } from './pages/agenda-audiencias/modal-partes/modal-partes.component';
import { ModalPedidosComponent } from './pages/agenda-audiencias/modal-pedidos/modal-pedidos.component';
import { ModalEditarAudienciaComponent } from './pages/agenda-audiencias/modal-editar-audiencia/modal-editar-audiencia.component';
import { FiltrosAgendaDeAudienciaComponent } from './pages/agenda-audiencias/filtros/filtros.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';

const opcoesSelect: INgxSelectOptions = {
  optionValueField: 'key',
  optionTextField: 'value',
  keepSelectedItems: false,
  autoSelectSingleOption: false,
};

defineLocale('pt-br', ptBrLocale);

@NgModule({
  declarations: [
    AgendaAudienciasComponent,
    ModalPartesAgendaComponent,
    ModalPedidosComponent,
    ModalEditarAudienciaComponent,
    FiltrosAgendaDeAudienciaComponent,
    TabelaOrdenacaoMultiColunasComponent,
    ModalOrdenacaoMultiColunasComponent
  ],
  imports: [
    CommonModule,
    AgendasRoutingModule,
    SharedModule,
    ReactiveFormsModule,
    InputsModule,
    FormsModule,
    NgbModule,
    NgbDatepickerModule,
    NgbTimepickerModule,
    NgbModalModule,
    NgbTabsetModule,
    BsDatepickerModule.forRoot(),
    TypeaheadModule.forRoot(),
    NgxSelectModule.forRoot(opcoesSelect),
    TextMaskModule,
    CurrencyMaskModule,
    InputsModule,
    InputMaskModule,
    ComponentesModule,
    NgSelectModule,
    ModalModule
  ],
  entryComponents: [
    ModalPartesAgendaComponent,
    ModalPedidosComponent,
    ModalEditarAudienciaComponent,
    ModalOrdenacaoMultiColunasComponent,
    OrdenacaoDragDropComponent
  ]
})
export class AgendasModule { }
