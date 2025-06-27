import { AfterViewInit, Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { EstadoEnum } from '@manutencao/models';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DialogService } from '@shared/services/dialog.service';
import { FechamentoAtmJecService } from '../../services/fechamento-atm-jec.service';
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-agendamento-modal',
  templateUrl: './agendamento-modal.component.html',
  styleUrls: ['./agendamento-modal.component.scss']
})
export class AgendamentoModalComponent implements AfterViewInit {
  constructor(
    private service: FechamentoAtmJecService,
    private modal: NgbActiveModal,
    private dialogService: DialogService
  ) {}

  fechamentos: Array<Fechamento> = [];
  indices: Array<Indice> = [];
  private indicesDoFechamento: Array<IndiceDoFechamento> = [];

  fechamentoFormControl: FormControl = new FormControl(null, [
    Validators.required
  ]);

  indicesFormArray: FormArray = new FormArray([]);

  private getIndiceFormGroup(estado: EstadoEnum): FormGroup {
    const indiceFormGroup = new FormGroup({
      estado: new FormControl(estado, [Validators.required]),
      indice: new FormControl(null, [Validators.required])
    });

    if (this.indicesDoFechamento.length === 0) {
      return indiceFormGroup;
    }

    setTimeout(() => {
      const indiceDoFechamento: IndiceDoFechamento =
        this.indicesDoFechamento.find(x => x.estado == estado.id);

      if (!indiceDoFechamento) {
        return;
      }

      const indice: Indice = this.indices.find(
        x => x.id == indiceDoFechamento.indiceId
      );

      if (!indice) {
        return;
      }

      indiceFormGroup.get('indice').setValue(indice);
    }, 0);

    return indiceFormGroup;
  }

  indiceCompareFn(one: Indice, other: Indice): boolean {
    return one.id === other.id;
  }

  agendamentoFormGroup: FormGroup = new FormGroup({
    fechamento: this.fechamentoFormControl,
    indices: this.indicesFormArray
  });

  async ngAfterViewInit(): Promise<void> {
    this.fechamentoFormControl.valueChanges.subscribe(async _ => {
      this.indicesDoFechamento = await this.service.indicesDoFechamento(
        this.fechamentoFormControl.value.id
      );
      this.refreshIndicesTable();
    });

    this.fechamentos = await this.service.obterFechamentos();
    this.indices = await this.service.obterIndices();
  }

  private async refreshIndicesTable(): Promise<void> {
    this.indicesFormArray.clear();

    EstadoEnum.Todos.forEach(estado => {
      this.indicesFormArray.push(this.getIndiceFormGroup(estado));
    });
  }

  close(): void {
    this.modal.close(false);
  }

  async agendar(): Promise<void> {
    try {
      await this.service.agendar(
        this.fechamentoFormControl.value.id,
        this.indicesFormArray.controls.map((indiceFormGroup: FormGroup) => {
          return {
            estado: indiceFormGroup.get('estado').value.id,
            indice: indiceFormGroup.get('indice').value.id
          };
        })
      );
      await this.dialogService.alert('Agendado com sucesso.');
      this.modal.close(true);
    } catch (error) {
      if (error && error.error) {
        await this.dialogService.err('Agendamento não realizado.', error.error);
        return;
      }

      await this.dialogService.err('Agendamento não realizado.');
    }
  }

  static exibeModal(): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoModalComponent,
      { centered: true, backdrop: 'static' }
    );
    return modalRef.result;
  }
}

declare interface Fechamento {
  id: number;
  mesAnoFechamento: Date;
  dataFechamento: Date;
  numeroDeMeses: number;
  mensal: boolean;
}

declare interface Indice {
  id: number;
  descricao: string;
  codigoTipoIndice: string;
  codigoValorIndice?: string;
  acumulado: boolean;
}

declare interface IndiceDoFechamento {
  indiceId: number;
  estado: string;
}
