import { Audiencia } from './../model/audiencia.model';
import { isNullOrUndefined } from 'util';
import { DialogService } from './../../../../../shared/services/dialog.service';
import { FormControl, FormGroup } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { Component, OnInit, Input } from '@angular/core';
import { EscritorioService } from './../services/escritorio.service';
import { PrepostoService } from '../services/preposto.service';
import { AdvogadoDoEscritorioService } from './../services/advogado-do-escritorio.service';
import { AudienciaService } from '../services/audiencia.service';
import { HttpErrorResult } from '@core/http';
import { StaticInjector } from '@shared/static-injector';
import { Preposto } from '../model/preposto.model';
import { Escritorio } from '../model/escritorio.model';

@Component({
  templateUrl: './modal-editar-audiencia.component.html',
  styleUrls: ['./modal-editar-audiencia.component.scss']
})
export class ModalEditarAudienciaComponent implements OnInit {

  constructor(
    public activeModal: NgbActiveModal,
    private audienciaService: AudienciaService,
    private prepostosService: PrepostoService,
    private escritoriosService: EscritorioService,
    private advogadoEscritorioService: AdvogadoDoEscritorioService,
    private dialog: DialogService) { }

  @Input() titulo: string;
  @Input() entidade: Audiencia;

  @Input() prepostos: Array<Preposto> = [];
  @Input() escritorios: Array<Escritorio> = [];

  resumoDadosDoProcesso: string;
  resumoDadosDaAudiencia: string;

  prepostoFormControl: FormControl = new FormControl(null);
  escritorioFormControl: FormControl = new FormControl(null);
  advogadoEscritorioFormControl: FormControl = new FormControl(null);

  formulario: FormGroup = new FormGroup({
    preposto: this.prepostoFormControl,
    escritorio: this.escritorioFormControl,
    advogadoEscritorio: this.advogadoEscritorioFormControl
  });

  advogadosEscritorio: Array<any> = [];

  ngOnInit() {
    if (!isNullOrUndefined(this.entidade.preposto)) {
      this.prepostos.forEach(p => {
        if (p.id === this.entidade.preposto.id) {
          this.prepostoFormControl.setValue(this.entidade.preposto.id);
        }
      });
    }
    if (!isNullOrUndefined(this.entidade.escritorio)) {
      this.escritorios.forEach(e => {
        if (e.id === this.entidade.escritorio.id) {
          this.escritorioFormControl.setValue(this.entidade.escritorio.id);
          this.obterAdvogadosDoEscritorio(this.entidade.escritorio);
          this.advogadoEscritorioFormControl.setValue(this.entidade.advogadoEscritorio.id);
        }
      });
    }
  }

  obterAdvogadosDoEscritorio(escritorio: any): void {
    try {
      this.advogadosEscritorio = null;
      this.advogadoEscritorioService.obterPorEscritorio(escritorio.id).then(adv => {
        this.advogadosEscritorio = adv.map(a => ({ id: a.id, nome: a.nome }));
      });
    } catch (error) {
      console.log(error);
    }
  }

  // tslint:disable-next-line: member-ordering
  public static exibeModalDeAlterar(entidade: Audiencia, escritorios: Array<any>, preposto: Array<any>): any {
    const modalService: NgbModal = StaticInjector.Instance.get(NgbModal);
    const modalRef = modalService.open(ModalEditarAudienciaComponent, { centered: true, size: 'lg', backdrop: 'static' });
    modalRef.componentInstance.titulo = 'Edição de Audiência';
    modalRef.componentInstance.entidade = entidade;
    modalRef.componentInstance.prepostos = preposto;
    modalRef.componentInstance.escritorios = escritorios;
    return modalRef.result;
  }

  async atualizar(): Promise<void> {
    const prepostoSelecionado = this.prepostos.find(x => x.id === this.prepostoFormControl.value);
    if (!isNullOrUndefined(prepostoSelecionado) && !prepostoSelecionado.ativo) {
      this.dialog.showErr('Operação não permitida', `O Preposto '${prepostoSelecionado.nome}' encontra-se inativo.`);
      return ;
    }

    const escritorioSelecionado = this.escritorios.find(x => x.id === this.escritorioFormControl.value);
    if (!isNullOrUndefined(escritorioSelecionado) && !escritorioSelecionado.ativo) {
      this.dialog.showErr('Operação não permitida', `O Escritório '${escritorioSelecionado.nome}' encontra-se inativo.`);
      return;
    }

    try {
      await this.audienciaService.atualizar({
        id: this.entidade.sequencial,
        processoId: this.entidade.processo.id,
        escritorioId: this.escritorioFormControl.value,
        advogadoId: this.advogadoEscritorioFormControl.value,
        prepostoId: this.prepostoFormControl.value,
        tipoAudienciaId: this.entidade.tipoAudiencia.id
      });
      this.activeModal.close();
      await this.dialog.showAlert('Alteração realizada com sucesso', 'A audiência foi alterada no sistema.');
    } catch (error) {
      await this.dialog.showErr('Operação não realizada', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
