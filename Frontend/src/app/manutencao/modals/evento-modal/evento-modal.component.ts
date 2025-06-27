import { AfterContentChecked, AfterViewInit, Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

// 3rd party
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

// core & shared
import { StaticInjector } from '@manutencao/static-injector';
import { DialogService } from '@shared/services/dialog.service';

// local
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { HttpErrorResult } from '@core/http';
import { EventoService } from '@manutencao/services/evento.service';
import { Evento } from '@manutencao/models/evento.model';
import { TipoProcessoAlteracaoProcessoBloco } from '@relatorios/models/tipo-processo-alteracao-processo-bloco';
import { TipoProcesso } from '@core/models/tipo-processo';
import { TipoProcessoEnum } from '@shared/enums/tipo-processoEnum.enum';

@Component({
  selector: 'app-evento-modal',
  templateUrl: './evento-modal.component.html',
  styleUrls: ['./evento-modal.component.scss']
})
export class EventoModalComponent implements  OnInit {
  evento: Evento;
  tipoProcesso: TiposProcesso;

  instancia: any = [ {id: 1, descricao: "Primeira"},{id: 2, descricao: "Segunda"},{id: 3, descricao: "Terceira"}];

  constructor(
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private serviceEvento: EventoService
  ) {}

eventosEstrategico: any;
eventosConsumidor: any;

  descricaoFormControl: FormControl = new FormControl('', [
    Validators.required,
    Validators.maxLength(50)
  ]);

  possuiDecisaoFormControl: FormControl = new FormControl(false);
  cumprimentodePrazoFormControl : FormControl = new FormControl(false);
  notificarViaEmailFormControl : FormControl = new FormControl(false);
  ativoFormControl : FormControl = new FormControl(true);
  instanciaIdFormControl : FormControl = new FormControl(null);
  preencheMultaFormControl : FormControl = new FormControl(false);
  reverCalculoFormControl : FormControl = new FormControl(false);
  finalizacaoContabilFormControl : FormControl = new FormControl(false);
  finalizacaoEscritorioFormControl : FormControl = new FormControl(false);
  alterarExcluirFormControl : FormControl = new FormControl(true);

  ehTrabalhistaAdmFormControl : FormControl = new FormControl(false);
  ehCivelFormControl : FormControl = new FormControl(false);
  ehCivelEstrategicoFormControl : FormControl = new FormControl(false);
  ehRegulatorioFormControl : FormControl = new FormControl(false);
  ehTrabalhistaFormControl : FormControl = new FormControl(false);
  ehTributarioAdministrativoFormControl : FormControl = new FormControl(false);
  ehTributarioJudicialFormControl : FormControl = new FormControl(false);
  comboEstrategicoFormControl : FormControl = new FormControl();
  comboConsumidorFormControl : FormControl = new FormControl();
  atualizaEscritorioFormControl : FormControl = new FormControl(false);


  formGroup: FormGroup = new FormGroup({
    nome: this.descricaoFormControl,
    possuiDecisao: this.possuiDecisaoFormControl,
    ehPrazo: this.cumprimentodePrazoFormControl,
    notificarViaEmail : this.notificarViaEmailFormControl,
    ativo : this.ativoFormControl,
    instanciaId : this.instanciaIdFormControl,
    preencheMulta : this.preencheMultaFormControl,
    alterarExcluir :this.alterarExcluirFormControl,
    finalizacaoContabil : this.finalizacaoContabilFormControl,
    finalizacaoEscritorio : this.finalizacaoEscritorioFormControl,
    reverCalculo : this.reverCalculoFormControl,
    ehTrabalhistaAdm :  this.ehTrabalhistaAdmFormControl,
    ehCivel :  this.ehCivelFormControl,
    ehCivelEstrategico :  this.ehCivelEstrategicoFormControl,
    ehRegulatorio :  this.ehRegulatorioFormControl,
    ehTrabalhista :  this.ehTrabalhistaFormControl,
    idEstrategico: this.comboEstrategicoFormControl,
    idConsumidor: this.comboConsumidorFormControl,
    atualizaEscritorio : this.atualizaEscritorioFormControl,
    ehTributarioAdm : this.ehTributarioAdministrativoFormControl,
    ehTributarioJudicial : this.ehTributarioJudicialFormControl
  });

  ngOnInit(): void {
    if(TipoProcessoEnum.administrativo == this.tipoProcesso.id)  {
      this.ehRegulatorioFormControl.setValue(TipoProcessoEnum.administrativo);
    }
    else
    if (TipoProcessoEnum.trabalhistaAdministrativo == this.tipoProcesso.id ) {
      this.ehTrabalhistaAdmFormControl.setValue(TipoProcessoEnum.trabalhistaAdministrativo);
    }
    else
    if (TipoProcessoEnum.civelConsumidor == this.tipoProcesso.id ) {
      this.ehCivelFormControl.setValue(TipoProcessoEnum.civelConsumidor);
      this.serviceEvento.obterDescricaoEstrategico().then(descricaoEstrategico=> this.eventosEstrategico = descricaoEstrategico.map(r => ({ id: r.id, nome: r.nome + (r.ativo ? '' : ' [INATIVO]') })));
    }
    else
    if (TipoProcessoEnum.civelEstrategico == this.tipoProcesso.id ) {
      this.ehCivelEstrategicoFormControl.setValue(TipoProcessoEnum.civelEstrategico);
      this.serviceEvento.obterDescricaoConsumidor().then(descricaoConsumidor => this.eventosConsumidor = descricaoConsumidor.map(r => ({ id: r.id, nome: r.nome + (r.ativo ? '' : ' [INATIVO]') })));
    }
    else
    if (TipoProcessoEnum.trabalhista == this.tipoProcesso.id ) {
      this.ehTrabalhistaFormControl.setValue(TipoProcessoEnum.trabalhista);
    }
    else
    if (TipoProcessoEnum.tributarioAdministrativo == this.tipoProcesso.id ) {
      this.ehTributarioAdministrativoFormControl.setValue(TipoProcessoEnum.tributarioAdministrativo);
    }
    else
    if (TipoProcessoEnum.tributarioJudicial == this.tipoProcesso.id ) {
      this.ehTributarioJudicialFormControl.setValue(TipoProcessoEnum.tributarioJudicial);
    }


    if (this.evento) {
      this.descricaoFormControl.setValue(this.evento.nome);
      this.possuiDecisaoFormControl.setValue(this.evento.possuiDecisao);
      this.cumprimentodePrazoFormControl.setValue(this.evento.ehPrazo);
      this.notificarViaEmailFormControl.setValue(this.evento.notificarViaEmail);
      this.ativoFormControl.setValue(this.evento.ativo);
      this.instanciaIdFormControl.setValue(this.evento.instanciaId);
      this.preencheMultaFormControl.setValue(this.evento.preencheMulta);
      this.alterarExcluirFormControl.setValue(this.evento.alterarExcluir);
      this.finalizacaoContabilFormControl.setValue(this.evento.finalizacaoContabil);
      this.finalizacaoEscritorioFormControl.setValue(this.evento.finalizacaoEscritorio);
      this.reverCalculoFormControl.setValue(this.evento.reverCalculo);
      this.ehCivelEstrategicoFormControl.setValue(this.evento.ehCivelEstrategico);
      this.ehTrabalhistaFormControl.setValue(this.evento.ehTrabalhista);
      this.ehTrabalhistaAdmFormControl.setValue(this.evento.ehTrabalhistaAdm);
      this.ehRegulatorioFormControl.setValue(this.evento.ehRegulatorio);
      this.ehCivelFormControl.setValue(this.evento.ehCivel);
      this.comboEstrategicoFormControl.setValue(this.evento.idDescricaoEstrategico);
      this.comboConsumidorFormControl.setValue(this.evento.idDescricaoConsumidor);
      this.atualizaEscritorioFormControl.setValue(this.evento.atualizaEscritorio);
    }
  }

  close(): void {
    this.modal.close(false);
  }

  async save(): Promise<void> {
    const operacao = this.evento ? 'Alteração' : 'Inclusão';

    if (!this.descricaoFormControl.value.replace(/\s/g, '').length) {
      await this.dialogService.err(
        `${operacao} não realizada`,
        `O campo nome não pode conter apenas espaços.`
      );
      return;
    }

    try {
      if (this.evento) {
        let obj :any =  this.formGroup.value;
        obj.id = this.evento.id;
        obj.codTipoProcesso = this.tipoProcesso.id;
        await this.serviceEvento.alterar(obj);
      } else {
        await this.serviceEvento.incluir(this.formGroup.value);
      }
      await this.dialogService.alert(`${operacao} realizada com sucesso`);
      this.modal.close(true);
    }  catch (error) {
      console.log(error);
      await this.dialogService.err(`${operacao} não realizada`, (error as HttpErrorResult).messages.join('\n'));
    };
  }
  // tslint:disable-next-line: member-ordering
  public static exibeModalDeIncluir(tipoProcesso: any): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EventoModalComponent, {windowClass: 'evento-modal', centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.tipoProcesso = TiposProcesso.porId(tipoProcesso);
    return modalRef.result;
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(tipoProcesso: any, evento: Evento): Promise<boolean> {
    // prettier-ignore
    const modalRef = StaticInjector.Instance.get(NgbModal)
      .open(EventoModalComponent, {windowClass: 'evento-modal', centered: true, size: 'lg', backdrop: 'static' });
      modalRef.componentInstance.tipoProcesso = TiposProcesso.porId(tipoProcesso);
      modalRef.componentInstance.evento = evento;
    return modalRef.result;
  }
}
