// angular
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { Cotacao } from '@manutencao/models/cotacao.model';
import { CotacaoServiceMock } from '@manutencao/services/cotacao.service.mock';
import { CotacaoService } from '@manutencao/services/cotacao.service';
import { HttpErrorResult } from '@core/http';

@Component({
  selector: 'app-cotacao-modal',
  templateUrl: './cotacao-modal.component.html',
  styleUrls: ['./cotacao-modal.component.scss'],
  providers: [{ provide: CotacaoServiceMock, useClass: CotacaoServiceMock }]

})

export class CotacaoModalComponent implements AfterViewInit, OnInit {
  cotacao: Cotacao;
  dataCotacao: Date;
  indice: { id: number, descricao: string, codigoValorIndice: string };
  indiceColuna: string = '';

  constructor(
    private modal: NgbActiveModal,
    private service: CotacaoService,
    private dialogService: DialogService,
  ) { }

  private today: Date = new Date();

  valorCotacaoFormControl: FormControl = new FormControl(null, [Validators.required, Validators.pattern("-?\\d{1,4}(\\,\\d{1,9})?")]);
  dataSelecionadaFormControl: FormControl = new FormControl(this.today);

  formGroup: FormGroup = new FormGroup({
    valorCotacao: this.valorCotacaoFormControl,
    dataSelecionada: this.dataSelecionadaFormControl
  });

  ngOnInit() {
    switch (this.indice.codigoValorIndice) {
      case 'F': this.indiceColuna = 'Fator'
        break;
      case 'P': this.indiceColuna = 'Percentual'
        break;
      case 'V': this.indiceColuna = 'Valor'
        break;
      default:
        this.indiceColuna = ''
        break;
    }
  }

  ngAfterViewInit(): void {
    
    
    if (this.cotacao) {
      this.dataCotacao = new Date(this.cotacao.dataCotacao);
      this.dataSelecionadaFormControl.setValue(this.dataCotacao);
      this.valorCotacaoFormControl.setValue(this.cotacao.valor.toString().replace('.', ','));
      this.dataSelecionadaFormControl.disable();
    }

  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    let request: any;
    let operacao: string;

    const valorCotacao = this.valorCotacaoFormControl.value.replace(',', '.');
    if (this.cotacao) {
      operacao = 'Alteração';
      const dataCotacao = `${this.dataCotacao.getMonth() + 1}/01/${this.dataCotacao.getFullYear()}`
      request = this.service.alterar(
        this.indice.id,
        dataCotacao,
        valorCotacao,
      );
    } else {
      
      operacao = 'Inclusão';
      const dataCotacao = `${this.dataSelecionadaFormControl.value.getMonth() + 1}/01/${this.dataSelecionadaFormControl.value.getFullYear()}`
      request = this.service.incluir(this.indice.id, dataCotacao, valorCotacao);
    }

    try {
      await request;
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('Já existe')) {
        await this.dialogService.err('Desculpe, não é possível fazer a inclusão', (error as HttpErrorResult).messages.join('\n'));
      } else {
        await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
      }
    }
  }

  //#region MODAL


  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(indice: { id: number, descricao: string, codigoValorIndice: string }): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(CotacaoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.indice = indice;
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(cotacao: Cotacao, indice: { id: number, descricao: string, codigoValorIndice: string }): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(CotacaoModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.cotacao = cotacao;
    modalRef.componentInstance.indice = indice;
    return modalRef.result;
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }

  //#endregion MODAL

}
