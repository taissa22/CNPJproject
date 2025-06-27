// angular
import { AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { DialogService } from '@shared/services/dialog.service';

// local
import { TipoDeVara } from '@manutencao/models/tipo-de-vara';
// import { TipoDeParticipacaoService } from '@manutencao/services/tipo-de-participacao.service';
import { TipoDeVaraServiceMock } from '@manutencao/services/tipo-de-vara.service.mock';
import { TipoDeVaraComponent } from '@manutencao/pages/tipo-de-vara/tipo-de-vara.component'
import {HttpClientModule} from '@angular/common/http';
import { StaticInjector } from '@manutencao/static-injector';
import { TipoDeVaraService } from '@manutencao/services/tipo-de-vara.service';
import { HttpErrorResult } from '@core/http';
import { listenToTriggers } from 'ngx-bootstrap/utils';

@Component({
  selector: 'app-tipo-de-vara-modal',
  templateUrl: './tipo-de-vara-modal.component.html',
  styleUrls: ['./tipo-de-vara-modal.component.scss'],
  // providers: [
  //   {
  //     provide: TipoDeVaraService,
  //     useClass: TipoDeVaraServiceMock
  //   }
  // ]
})
export class TipoDeVaraModalComponent implements AfterViewInit {
  tipoDeVara: TipoDeVara;
  // codigo: number;
  // nome: string;
  titulo: string;

  constructor(
    private modal: NgbActiveModal,
    private service: TipoDeVaraService,
    private dialogService: DialogService
  ) {}


  nomeFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(30)
  ]);

  indCivelFormControl: FormControl = new FormControl(false);
  indCivelEstrategicoFormControl: FormControl = new FormControl(false);
  indTrabalhistaFormControl: FormControl = new FormControl(false);
  indTributariaFormControl: FormControl = new FormControl(false);
  indJuizadoFormControl: FormControl = new FormControl(false);
  indCriminalJudicialFormControl: FormControl = new FormControl(false);
  indProconFormControl: FormControl = new FormControl(false);

  formGroup: FormGroup = new FormGroup({
    nome: this.nomeFormControl,
    indCivel: this.indCivelFormControl,
    indCivelEstrategico: this.indCivelEstrategicoFormControl,
    indTrabalhista: this.indTrabalhistaFormControl,
    indTributaria: this.indTributariaFormControl,
    indJuizado: this.indJuizadoFormControl,
    indCriminalJudicial: this.indCriminalJudicialFormControl,
    indProcon: this.indProconFormControl
  });


  async ngAfterViewInit(): Promise<void> {
    
    await this.podeAlterarTipoProcesso();

    if (this.tipoDeVara) {      
      this.nomeFormControl.setValue(this.tipoDeVara.nome);
      this.indCivelFormControl.setValue(this.tipoDeVara.indCivel);
      this.indCivelEstrategicoFormControl.setValue(this.tipoDeVara.indCivelEstrategico);
      this.indTrabalhistaFormControl.setValue(this.tipoDeVara.indTrabalhista);
      this.indTributariaFormControl.setValue(this.tipoDeVara.indTributaria);
      this.indJuizadoFormControl.setValue(this.tipoDeVara.indJuizado);
      this.indCriminalJudicialFormControl.setValue(this.tipoDeVara.indCriminalJudicial);
      this.indProconFormControl.setValue(this.tipoDeVara.indProcon);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    let operacao = this.tipoDeVara? 'Alteração' : 'Inclusão';

    if (!this.nomeFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(`${operacao} não realizada`, `O campo nome não pode contar apenas espaços.`);
      return;
    }

    try {
      if (this.tipoDeVara) {
        await this.service.alterar(
          this.tipoDeVara.codigo,
          this.nomeFormControl.value,
          this.indCivelFormControl.value,
          this.indCivelEstrategicoFormControl.value,
          this.indTrabalhistaFormControl.value,
          this.indTributariaFormControl.value,
          this.indJuizadoFormControl.value,
          this.indCriminalJudicialFormControl.value,
          this.indProconFormControl.value
        );
      } else {
        
        await this.service.incluir(
          this.nomeFormControl.value,
          this.indCivelFormControl.value,
          this.indCivelEstrategicoFormControl.value,
          this.indTrabalhistaFormControl.value,
          this.indTributariaFormControl.value,
          this.indJuizadoFormControl.value,
          this.indCriminalJudicialFormControl.value,
          this.indProconFormControl.value
          );
      }

      await this.dialogService.alert(`${operacao} realizada com sucesso.`);
      this.modal.close(true);
    } catch (error) {     
      await this.dialogService.err(`${operacao} não realizada` , (error as HttpErrorResult).messages.join('\n'));
    }
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeIncluir(): Promise<boolean> {
    
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeVaraModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Incluir Tipo de Vara';
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  static exibeModalDeAlterar(
    tipoDeVara: TipoDeVara
  ): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(TipoDeVaraModalComponent, { centered: true, size: 'sm', backdrop: 'static' });
    modalRef.componentInstance.tipoDeVara = tipoDeVara;
    modalRef.componentInstance.titulo = 'Alterar Tipo de Vara';
    return modalRef.result;
  }

  podeGravar(): boolean {
    return (this.indCivelFormControl.value || this.indCivelEstrategicoFormControl.value
            || this.indTrabalhistaFormControl.value || this.indTributariaFormControl.value
            || this.indJuizadoFormControl.value || this.indCriminalJudicialFormControl.value
            || this.indProconFormControl.value );
  }

  async podeAlterarTipoProcesso(): Promise<void> {
    await this.utilizadoEmProcesso(1) ? this.indCivelFormControl.disable() : this.indCivelFormControl.enable();
    await this.utilizadoEmProcesso(9) ? this.indCivelEstrategicoFormControl.disable() : this.indCivelEstrategicoFormControl.enable();
    await this.utilizadoEmProcesso(2) ? this.indTrabalhistaFormControl.disable() : this.indTrabalhistaFormControl.enable();
    await this.utilizadoEmProcesso(5) ? this.indTributariaFormControl.disable() : this.indTributariaFormControl.enable();
    await this.utilizadoEmProcesso(7) ? this.indJuizadoFormControl.disable() : this.indJuizadoFormControl.enable();
    await this.utilizadoEmProcesso(15) ? this.indCriminalJudicialFormControl.disable() : this.indCriminalJudicialFormControl.enable();
    await this.utilizadoEmProcesso(17) ? this.indProconFormControl.disable() : this.indProconFormControl.enable();
  }

  async utilizadoEmProcesso(codTipoProcesso: number): Promise<boolean> {
    let utilizadoEmProcesso: boolean = false;
    if (this.tipoDeVara) {
      utilizadoEmProcesso = await this.service.utilizadoEmProcesso(this.tipoDeVara.codigo, codTipoProcesso);  
    }
    return utilizadoEmProcesso;
  }
}

