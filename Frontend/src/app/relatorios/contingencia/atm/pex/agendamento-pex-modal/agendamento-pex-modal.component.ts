import { AfterViewInit, Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { EstadoEnum } from '@manutencao/models';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { DialogService } from '@shared/services/dialog.service';
import { RelatorioAtmPexService } from '../../services/relatorio-atm-pex.service';
import { StaticInjector } from '../../static-injector';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-agendamento-pex-modal',
  templateUrl: './agendamento-pex-modal.component.html',
  styleUrls: ['./agendamento-pex-modal.component.scss']
})
export class AgendamentoPexModalComponent implements AfterViewInit {

  fechamentos: Array<Fechamento> = [];
  indices: Array<any> = [];
  private indicesDoFechamento: Array<IndiceDoFechamento> = [];

  indicesFormArray: FormArray = new FormArray([]);

  agendamentoFormGroup: FormGroup = new FormGroup({
    indices: this.indicesFormArray
  });

  dataFechamentoSelecionado: string;
  mesDataSelecionada: string = '';
  anoDataSelecionada: string = '';

  codSolicFechamento: number = null;
  dataFechamento: Date;

  constructor(
    private service: RelatorioAtmPexService,
    private modal: NgbActiveModal,
    private dialogService: DialogService,
    private messageService: HelperAngular
  ) {}

  async ngAfterViewInit(): Promise<void> {

  }

  static exibeModal(fechamentos: Array<Fechamento>, indices: Array<any>): Promise<boolean> {
    const modalRef = StaticInjector.Instance.get(NgbModal).open(
      AgendamentoPexModalComponent,
      { centered: true, backdrop: 'static', size: 'lg', windowClass:"modal-agend-movimentacao-pex" }
    );
    modalRef.componentInstance.fechamentos = fechamentos;
    modalRef.componentInstance.indices = indices;
    return modalRef.result;
  }

  close(): void {
    this.modal.close(false);
  }

  async selecionarFechamento(codSolicFechamento, data): Promise<void> {
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
      const indiceDoFechamento: IndiceDoFechamento =
        this.indicesDoFechamento.find(x => x.codEstado.trim() == estado.id);

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

  async agendar(bloqueiaValidacaoIndice): Promise<void> {
    try {
      await this.service.agendar(
        this.codSolicFechamento,
        this.dataFechamento,
        bloqueiaValidacaoIndice
        ,this.indicesFormArray.controls.map((indiceFormGroup: FormGroup) => {
          return {
            codEstado: indiceFormGroup.get('estado').value.id,
            codIndice: indiceFormGroup.get('indice').value.codIndice
          };
        })
      );
      await this.dialogService.alert('Agendado com sucesso.');
      this.modal.close(true);
    } catch (error) {
      if (error && error.error) {
        if(typeof error.error == 'object') {
          this.confirmaAgendamento(error.error);
          return;
        }

        await this.dialogService.err('Agendamento não realizado.', error.error);
        return;
      }

      await this.dialogService.err('Agendamento não realizado.');
    }
  }

  async confirmaAgendamento(listaIndices: number[]): Promise<void> {
    let nomesIndice: string = '';
    let cont: number = 0;
    listaIndices.map(id => {
      const indice = this.indices.find(
        x => x.codIndice == id
      );
      nomesIndice += indice.descricao;
      cont++;
      if (cont === listaIndices.length) {
        nomesIndice += '. ';
      } else {
        nomesIndice += ', ';
      }
    });

    for (const fechamento of this.fechamentos){
      if (fechamento.codSolicFechamentoCont === this.codSolicFechamento) {
        this.dataFechamentoSelecionado = fechamento.dataFechamento.toString();
      }
    }
    this.anoDataSelecionada = this.dataFechamentoSelecionado.split(' ')[3];
    this.mesDataSelecionada = this.dataFechamentoSelecionado.split(' ')[1].replace('Jan', '01')
      .replace('Feb', '02')
      .replace('Mar', '03')
      .replace('Apr', '04')
      .replace('May', '05')
      .replace('Jun', '06')
      .replace('Jul', '07')
      .replace('Aug', '08')
      .replace('Sep', '09')
      .replace('Oct', '10')
      .replace('Nov', '11')
      .replace('Dec', '12');

    try
    {
      Swal.fire({
        html: "<p style='text-align: left;'>Atenção! Não existe cotação registrada no sistema para o mês " + this.mesDataSelecionada + "/" + this.anoDataSelecionada + " para o(s) seguintes índices: " + nomesIndice + " <br/><br/>O resultado do cálculo do ATM pode ser impactado por conta dessa situação. <br/><br/>Deseja prosseguir com o agendamento?</p>",
        cancelButtonText: "Sim",
        showCancelButton:   true ,
        cancelButtonColor: '#aaa',
        confirmButtonText: "Não",
        showConfirmButton:  true ,
        confirmButtonColor: '#6F62B2',
        reverseButtons: true
      }).then(resposta =>
        {
          if (!resposta.value)
          {
            this.agendar(true);
          }
        });

    }
    catch (error)
    {
      if (error && error.error)
      {
        await this.dialogService.err('Erro.', error.error);
        return;
      }
    }
  }

}

declare interface Fechamento {
  id: number;
  codSolicFechamentoCont: number;
  multDesvioPadrao: number;
  dataFechamento: Date;
  numeroMeses: number;
  dataAgendamento: Date;
  dataExecucao: Date;
  empresas: string;
  indAplicarHaircut: string;
  nomeUsuario: string;
  percentualHaircut: number;
}

declare interface Indice {
  id: number;
  descricao: string;
  codigoTipoIndice: string;
  codigoValorIndice?: string;
  acumulado: boolean;
}

declare interface IndiceDoFechamento {
  codIndice: number;
  codEstado: string;
}
