import { AfterViewInit, Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { EstadoEnum } from '@manutencao/models';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { DialogService } from '@shared/services/dialog.service'; 

import Swal from 'sweetalert2';
import { AtmService } from '@core/services/relatorios/atm/atm.service';
import { FechamentoProps } from '../model/AgendamentoATMCC'; 
import { StaticInjector } from '../../static-injector';

@Component({
  selector: 'app-agendamento-cc-modal',
  templateUrl: './agendamento-cc-modal.component.html',
  styleUrls: ['./agendamento-cc-modal.component.scss']
})
export class AgendamentoCCModalComponent implements AfterViewInit {

  fechamentos: Array<FechamentoProps> = [];
  indices: Array<any> = [];
  private indicesDoFechamento: Array<any> = [];

  indicesFormArray: FormArray = new FormArray([]);

  agendamentoFormGroup: FormGroup = new FormGroup({
    indices: this.indicesFormArray
  });

  dataFechamentoSelecionado: string;
  mesDataSelecionada: string = '';
  anoDataSelecionada: string = '';

  codSolicFechamento: number = null;
  id: number = null;
  dataFechamento: Date;

  constructor(
    private service: AtmService,
    private modal: NgbActiveModal,
    private dialogService: DialogService
  ) {}

  async ngAfterViewInit(): Promise<void> {
    this.obterIndices();
     this.obterFechamentos();
  }

 

  close(): void {
    this.modal.close(false);
  }

  async selecionarFechamento(codSolicFechamento, data, id): Promise<void> {
    this.id = id;
    this.codSolicFechamento = codSolicFechamento;
    this.dataFechamento = data;

    this.indicesDoFechamento = await this.service.indicesDoFechamento(
      this.codSolicFechamento
    );

    this.refreshIndicesTable();
  }

  private async refreshIndicesTable(): Promise<void> {
    this.indicesFormArray.clear();

    EstadoEnum.Todos.forEach(estado => {
      this.indicesFormArray.push(this.getIndiceFormGroup(estado));
    });
  }

  private getIndiceFormGroup(estado: EstadoEnum): FormGroup {
    const indiceFormGroup = new FormGroup({
      estado: new FormControl(estado, [Validators.required]),
      indice: new FormControl(null, [Validators.required])
    });

    if (this.indicesDoFechamento.length === 0) {
      return indiceFormGroup;
    }

    setTimeout(() => {
      const indiceDoFechamento: any = this.indicesDoFechamento.find(x => x.codEstado.trim() == estado.id);

      if (!indiceDoFechamento) {
        return;
      }

      const indice = this.indices.find(
        x => x.codIndice == indiceDoFechamento.codIndice
      );

      if (!indice) {
        return;
      }

      indiceFormGroup.get('indice').setValue(indice);
    }, 0);

    return indiceFormGroup;
  }

  indiceCompareFn(one: any, other: any): boolean {
    return one.codIndice === other.codIndice;
  }
 
  agendar(): void { 
    
    let fech = this.fechamentos.find(x => x.id == this.id);
  
    if (!fech) { 
      this.dialogService.err('Fechamento não encontrado.');
      return;
    }
   
    const novoAgendamento: any = {
      codFechContCcMedia: fech.codSolicFechamentoCont || 0,
      mesAnoContabil: fech.mesAnoFechamento,
      datFechamento: fech.dataFechamento || '',
      mesAnoFechamento: fech.mesAnoFechamento || '',
      IndFechMensal: 'S',
      numeroMeses : fech.numeroMeses,
      empresas: fech.empresas,
      EmpresaCentralizadora: fech.empresaCentralizador || '',
      uFs: this.indicesFormArray.controls.map((indiceFormGroup: FormGroup) => {
        return {
          uf: indiceFormGroup.get('estado').value.id,
          Indice: indiceFormGroup.get('indice').value.codIndice
        };
      })
    };
   
    this.service.agendar(novoAgendamento).subscribe({
      next: (resultado) => { 
        this.dialogService.alert('Agendado com sucesso.');
        this.modal.close(true);
      },
      error: (error) => { 
        if (error && error.error) { 
          this.dialogService.err('Agendamento não realizado.', error.error);
          return;
        }
   
        this.dialogService.err('Agendamento não realizado.');
      }
    });
  }
  
  

  obterIndices() {
    this.service.getObterIndices().subscribe(
      (data: any[]) => {
        this.indices = data;
      },
      error => console.error('Erro ao buscar índices', error)
    );
  }

  obterFechamentos() {
    this.service.getObterFechamentos().subscribe(
      (data: FechamentoProps[]) => {
        
        this.fechamentos = data;
      },
      error => console.error('Erro ao buscar fechamentos', error)
    );
  }

   static exibeModal(fechamentos: Array<FechamentoProps>, indices: Array<any>): Promise<boolean> {
    
    const modalRef = StaticInjector.Instance.get(NgbModal).open(AgendamentoCCModalComponent,
      { centered: true, backdrop: 'static', size: 'lg', windowClass:"modal-agend-movimentacao-cc" }
    );
    modalRef.componentInstance.fechamentos = fechamentos;
    modalRef.componentInstance.indices = indices;
    return modalRef.result;
  }

}



 
