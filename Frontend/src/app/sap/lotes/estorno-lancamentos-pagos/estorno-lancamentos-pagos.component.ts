import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';
import { ProcessoLocalizado } from '@shared/interfaces/processo-localizado';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { TipoProcesso } from 'src/app/core/models/tipo-processo';
import { role } from '../../sap.constants';
import { EstornoLancamentosPagosService } from './services/estorno-lancamentos-pagos.service';

@Component({
  selector: 'app-estorno-lancamentos-pagos',
  templateUrl: './estorno-lancamentos-pagos.component.html',
  styleUrls: ['./estorno-lancamentos-pagos.component.scss']
})
export class EstornoLancamentosPagosComponent implements OnInit, OnDestroy, AfterViewInit {

  //#region Subscriptions
  comboboxTipoProcessoSubscription: Subscription;
  buscarProcessosSubscription: Subscription;
  //#endregion

  //#region View vars
  // Contém os itens da combo tipo processo
  comboBoxTipoProcesso: TipoProcesso[] = [];

  // Processos Localizados
  processosLocalizados: ProcessoLocalizado[] = [];

  realizouBusca = false;
  //#endregion
  codigoInterno = true;
  numProcesso = false;
  breadcrumb: string;

  constructor(private service: EstornoLancamentosPagosService, private breadcrumbsService: BreadcrumbsService) { }

  //#region Event Handlers
  ngOnInit() {

    this.valorNumero = null;
    this.realizouBusca = false;
    // Prepara a combobox de tipo processo
    this.comboboxTipoProcessoSubscription = this.service.carregaItensComboBoxTipoProcesso()
      .subscribe(itens => this.comboBoxTipoProcesso = itens.filter(id => id.id != TipoProcessoEnum.juizadoEspecial));

      this.buscarProcessosSubscription = this.service.buscarProcessosResponseSubject
      .subscribe(processos => this.processosLocalizados = processos);

    this.service.realizouBusca.subscribe(item =>
      this.realizouBusca = item
    );

    this.service.currentItemComboboxProcesso.subscribe(
      item => {
        this.service.setCurrentIdentificacao(null);
        if (item == 1) {

          this.codigoInterno = true;
          this.numProcesso = false;
        } else {
          this.numProcesso = true;
          this.codigoInterno = false;
        }
      }
    );


  }


  get processo() {
    return this.service.currentItemComboboxProcesso.value;
  }



  async ngAfterViewInit(): Promise<void> {
    this.service.limparResultado()
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(role.menuEstornoLancamento);
  }

  ngOnDestroy() {
    this.comboboxTipoProcessoSubscription.unsubscribe();
    this.buscarProcessosSubscription.unsubscribe();
    this.service.currentItemComboboxProcesso.next(0);
    //this.service.currentItemComboboxTipoProcesso.next(0);
    this.service.buscarProcessosResponseSubject.next([]);
    this.valorNumero = null;
    this.processosLocalizados = [];
  }

  onClickBuscarProcessos() {
    this.service.setCurrentIdentificacao(this.valorNumero);
    this.service.buscarProcessos()
      .pipe(take(1))
      .subscribe(response => { })
  }

  onChangeComboboxProcesso(index) {
    this.service.setCurrentItemComboboxProcesso(parseInt(index));
    this.valorNumero = null;
  }

  onChangeComboboxTipoProcesso(index) {
    this.service.setCurrentItemComboboxTipoProcesso(parseInt(index));
  }

  valorNumero;



  onCardClick(index) {
    this.service.toResultado(this.processosLocalizados[index]);
    this.service.buscarProcessosResponseSubject.next([])
  }
  //#endregion

  get isBuscarEnabled() {

    return this.service.currentItemComboboxProcesso.value &&
      this.service.currentItemComboboxTipoProcesso.value &&
      this.valorNumero;
  }
}
