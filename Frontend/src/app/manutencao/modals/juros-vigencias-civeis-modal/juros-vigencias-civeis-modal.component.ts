//import { TipoDePrazo } from './../../models/tipo-de-prazo';
import { JurosVigenciasCiveis } from '@manutencao/models/juros-vigencias-civeis.model';
//import { TipoDePrazoService } from '@manutencao/services/tipo-de-prazo.service';
import { TipoProcesso } from './../../../core/models/tipo-processo';
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { StaticInjector } from '@manutencao/static-injector';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { DialogService } from '@shared/services/dialog.service';
import { JurosVigenciasCiveisService } from '@manutencao/services/juros-vigencias-civeis.service';
import { BsLocaleService } from 'ngx-bootstrap';

@Component({
  selector: 'app-juros-vigencias-civeis-modal',
  templateUrl: './juros-vigencias-civeis-modal.component.html',
  styleUrls: ['./juros-vigencias-civeis-modal.component.scss']
})
export class JurosVigenciasCiveisModalComponent implements AfterViewInit {

  constructor(
    private service: JurosVigenciasCiveisService,
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private configLocalizacao: BsLocaleService,) 
    {
      this.configLocalizacao.use('pt-BR');
    }

  tiposDeProcesso: TiposProcesso[];  
  ativarPrazoServico: boolean = false;
  ativarPrazoDocumento: boolean = false;
  jurosVigenciasCiveis: JurosVigenciasCiveis; 

  titulo: string ;

  async ngAfterViewInit() {
    if (this.jurosVigenciasCiveis) {
        this.tipoProcessoFormControl.setValue(this.jurosVigenciasCiveis.tipoDeProcesso.id);
        this.valorFormControl.setValue(this.jurosVigenciasCiveis.valorJuros);
        this.dataFormControl.setValue(new Date(this.jurosVigenciasCiveis.dataVigencia));
        this.tipoProcessoFormControl.disable();
        this.dataFormControl.disable();
    }

    if (this.tiposDeProcesso.length === 1) {
      this.tipoProcessoFormControl.setValue(this.tiposDeProcesso[0].id);
    }
    
  }

  tipoProcessoFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);
  valorFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.pattern("\\d{1,3}(\\,\\d{1,2})?")
  ]);
  dataFormControl: FormControl = new FormControl('', [
    Validators.required
  ]);


  formGroup: FormGroup = new FormGroup({
    tipoProcesso: this.tipoProcessoFormControl,
    descricao: this.valorFormControl,
    data: this.dataFormControl
  });


  static exibeModalDeIncluir(tiposDeProcesso: Array<TiposProcesso>): Promise<boolean> {
    // prettier-ignore

    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(JurosVigenciasCiveisModalComponent, { centered: true, windowClass: 'modalSize', backdrop: 'static' });
    modalRef.componentInstance.tiposDeProcesso = tiposDeProcesso;
    modalRef.componentInstance.titulo = 'Inclusão de vigência de taxa de juros' ;
    
    return modalRef.result;
  }

  static exibeModalDeAlterar(
    jurosVigenciasCiveis: JurosVigenciasCiveis,
    tiposDeProcesso: Array<TiposProcesso>
  ): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(JurosVigenciasCiveisModalComponent, { centered: true, windowClass: 'modalSize', backdrop: 'static' });
      
    modalRef.componentInstance.titulo = 'Alteração de vigência de taxa de juros' ;
    modalRef.componentInstance.jurosVigenciasCiveis = jurosVigenciasCiveis 
    modalRef.componentInstance.tiposDeProcesso = tiposDeProcesso;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  selecionarTipoProcesso(processoSelecionado) {
    this.tipoProcessoFormControl.setValue(processoSelecionado)
  }

  setData(data){
    data ? this.dataFormControl.setValue(data) : this.dataFormControl.setValue('')
  }

  desabilitaTooltip(formControl: FormControl) {
    return formControl.untouched || formControl.valid;
  }


  async salvar(): Promise<void> {

    
    let request: any;
    let operacao = '';
    if (this.jurosVigenciasCiveis) {
      operacao = 'Alteração';
      request = this.service.alterar(
        this.tipoProcessoFormControl.value,
        this.dataFormControl.value,
        this.valorFormControl.value.replace(',','.')
      );
    } else {
      operacao = 'Inclusão';
      request = this.service.incluir(
        this.tipoProcessoFormControl.value,
        this.dataFormControl.value,
        this.valorFormControl.value.replace(',','.'));
    }

    try {
      await request;
      await this.dialogService.alert( `${operacao} realizada com sucesso`);
      this.modal.close(true);
    } catch (error) {
      if ((error as HttpErrorResult).messages.join('\n').includes('Não é permitido') && operacao == 'Inclusão') {
        this.dialogService.info('Desculpe, não é possível fazer a inclusão', (error as HttpErrorResult).messages.join('\n'));
      } else {
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
      }
    }


  }
}

