import { FilterService } from 'src/app/sap/consultaLote/services/filter.service';
import { filter, take } from 'rxjs/operators';
import { CriteriosGeraisService } from './../../services/criterios-gerais.service';
import { Component, OnInit, OnChanges } from '@angular/core';
import { NgbDateParserFormatter, NgbDateStruct, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { LoteService } from 'src/app/core/services/sap/lote.service';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { SapService } from 'src/app/core/services/sap/sap.service';
import { TipoProcessoService } from 'src/app/core/services/sap/tipo-processo.service';
import { PermissoesSapService } from 'src/app/sap/permissoes-sap.service';
import { faCalendarAlt } from '@fortawesome/free-solid-svg-icons';
import { ListaFiltroRadioConsultaLoteEnum } from '@shared/enums/lista-filtro-radio-consulta-lote.enum';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';


@Component({
  // tslint:disable-next-line: component-selector
  selector: 'filtro-data',
  templateUrl: './filtro-data.component.html',
  styleUrls: ['./filtro-data.component.scss'],
  providers: [{ provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})

// tslint:disable-next-line: class-name
export class filtrodataComponent implements OnInit {

  constructor(public loteService: LoteService,
    private sapService: SapService,
    private filterService: FilterService,
    public permissoesSapService: PermissoesSapService,
    public service: CriteriosGeraisService,
    private router: Router) { }

  // formCriterios: FormGroup;
  // faCalendarAlt = faCalendarAlt;
  subscription : Subscription;

  form = new FormGroup({
    contabil: new FormControl(this.service.selectContabil),
    processo: new FormControl(this.service.selectProcesso),

  });




  public tipoSelecionado = false;
  // //public t;

  ngOnInit() {
    this.service.atualizarContador();
    this.form.reset({
      contabil: this.service.selectContabil,
      processo: this.service.selectProcesso
    })

    this.subscription = this.filterService.limparContadores.subscribe( _ => {this.resetarRadioButton()})

    //this.atualizaCount();
    this.verificarTipoProcesso();
    //this.addValuesWhenChanges();
    //this.form.value.contabil = 'inativo'
    this.changeRadioButton('contabil', this.form.value.contabil)
    this.changeRadioButton('processo', this.form.value.processo)
    this.form.value.contabil;
    this.form.valueChanges.subscribe(formResult => {
      this.changeRadioButton('contabil', formResult.contabil)
      this.changeRadioButton('processo', formResult.processo)
    })



  }



  onDestroy() {
    this.tipoSelecionado = false;
  }


  private verificarTipoProcesso() {
    const valorAntigo = this.filterService.tipoProcessoTracker.value;

    this.filterService.tipoProcessoTracker.subscribe(item => {
      if (item) {
        this.tipoSelecionado = true;
      }
      if (item !== valorAntigo) {
        this.form.get('contabil').setValue('3');
        this.form.get('processo').setValue('3');
      }
    });
  }

  public resetarRadioButton() {
    this.form.get('contabil').setValue('3');
    this.form.get('processo').setValue('3');
  }


  /**
  * Envia os valores do data range para o componente e service
  * @param label Label do campo de data utilizado
  * @param isDataInicial true se for data inicial, false se for data final
  * @param data Valor da data
  */
  changeData(label: string, isDataInicial: boolean, data: Date) {

    this.service.setData(label, isDataInicial, data);

  }


  /**
  * Envia os valores do data range para o componente e service
  * @param label Label do campo de data utilizado
  * @param isDataInicial true se for data inicial, false se for data final
  * @returns returna uma data
  */
  recuperarData(label: string, isDataInicial: boolean): Date {

    return this.service.getData(label, isDataInicial);

  }

  onRangeValido(nomeCampo: string, valid: boolean) {
    this.service.validar(nomeCampo, valid)
  }
  // onNumeroValido(valid: boolean){

  // }

  recuperarNumero(isInicio: boolean): string {
    return this.service.getNumero(isInicio);
  }

  salvarNumero(isInicio: boolean, numero: string) {

    this.service.setNumero(isInicio, numero)
  }


  changeRadioButton(nome: string, opcao: string) {
    this.service.setRadioButton(nome, opcao);
  }


}
