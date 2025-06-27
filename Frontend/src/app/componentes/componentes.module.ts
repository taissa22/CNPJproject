import { CriteriosDePesquisaComponent } from './../sap/criterios-de-pesquisa/criterios-de-pesquisa.component';
import { NgModule, LOCALE_ID } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FiltroRelatoriosComponent } from './filtro-relatorios/filtro-relatorios.component';
import { DualListComponent } from './dual-list/dual-list.component';
import { RouterModule } from '@angular/router';
import { ContainerExpansivelComponent } from './container-expansivel/container-expansivel.component';
import { ContainerPaginaComponent } from './container-pagina/container-pagina.component';
import { ContainerExpansivelNewComponent } from './container-expansivel-new/container-expansivel-new.component';
import { BotaoOrdenacaoComponent } from './BotaoOrdenacao/botao-ordenacao.component';
import { TableOptionsComponent } from './table-options/table-options.component';
import { FormsModule } from '@angular/forms';
import { PaginacaoButtonsComponent } from './paginacao-buttons/paginacao-buttons.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CardContainerComponent } from './card-container/card-container.component';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { DualListTituloComponent } from './dual-list-titulo/dual-list-titulo.component';
import { VerticalDualListComponent } from './vertical-dual-list/vertical-dual-list.component';
import { NumberRangeComponent } from './number-range/number-range.component';
import { DataRangeComponent } from './range-datas/data-range.component';
import { BsDatepickerModule, ModalModule } from 'ngx-bootstrap';
import { InputMaskModule } from '@libs/racoon-mask/input-mask.module';
import { CurrencyMaskModule } from 'ng2-currency-mask';
import { SharedModule } from '@shared/shared.module';
import { ContainerItemComponent } from './container-item/container-item.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { ComboBoxComponent } from './combo-box/combo-box.component';
import { OrdenacaoDragDropComponent } from './ordenacao-drag-drop/ordenacao-drag-drop.component';
import { TabelaComponent } from '../sap/criacaoLote/tabela/tabela.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { PedidosModalComponent } from './pedidos-modal/pedidos-modal.component';
import { PartesModalComponent } from './partes-modal/partes-modal.component';
import { FiltroBuscaPadraoComponent } from './filtro-busca-padrao/filtro-busca-padrao.component';
import { DatasComponent } from './datas/datas.component';
import { NumberComponent } from './number/number.component';
import { MesAnoComponent } from './mes-ano/mes-ano.component';
import { MesAnoRangeComponent } from './mes-ano-range/mes-ano-range.component';
import { MesAnoNewComponent } from './mes-ano-new/mes-ano-new.component';
import { PesquisaHeaderComponent } from './pesquisa-header/pesquisa-header.component';

@NgModule({
  imports: [
    CommonModule,
    NgSelectModule,
    RouterModule,
    FormsModule,
    NgbModule,
    ScrollingModule,
    BsDatepickerModule.forRoot(),
    InputMaskModule,
    CurrencyMaskModule,
    SharedModule,
    DragDropModule,
    ModalModule.forRoot()
  ],
  declarations: [
    DualListComponent,
    FiltroRelatoriosComponent,
    ContainerPaginaComponent,
    BotaoOrdenacaoComponent,
    TableOptionsComponent,
    PaginacaoButtonsComponent,
    CardContainerComponent,
    DualListTituloComponent,
    VerticalDualListComponent,
    NumberRangeComponent,
    DataRangeComponent,
    ContainerItemComponent,
    OrdenacaoDragDropComponent,
    ContainerExpansivelComponent,
    ContainerExpansivelNewComponent,
    ComboBoxComponent,
    PartesModalComponent,
    CriteriosDePesquisaComponent,
    PedidosModalComponent,
    TabelaComponent,
    FiltroBuscaPadraoComponent,
    DatasComponent,
    NumberComponent,
    MesAnoComponent,
    MesAnoRangeComponent,
    MesAnoNewComponent
  ],
  exports: [
    DualListComponent,
    FiltroRelatoriosComponent,
    BotaoOrdenacaoComponent,
    TableOptionsComponent,
    PaginacaoButtonsComponent,
    CardContainerComponent,
    DualListTituloComponent,
    VerticalDualListComponent,
    NumberRangeComponent,
    DataRangeComponent,
    ContainerItemComponent,
    ComboBoxComponent,
    OrdenacaoDragDropComponent,
    ContainerExpansivelComponent,
    ContainerExpansivelNewComponent,
    PartesModalComponent,
    CriteriosDePesquisaComponent,
    TabelaComponent,
    FiltroBuscaPadraoComponent,
    DatasComponent,
    NumberComponent,
    ContainerPaginaComponent,
    MesAnoComponent,
    MesAnoRangeComponent,
    MesAnoNewComponent,
  ],
  entryComponents: [
    PartesModalComponent,
    PedidosModalComponent,
    OrdenacaoDragDropComponent
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' }],
  bootstrap: [DataRangeComponent]
})
export class ComponentesModule {}
