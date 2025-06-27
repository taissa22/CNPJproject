import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {
  NgbDateAdapter,
  NgbDateNativeAdapter,
  NgbDateParserFormatter,
  NgbModalRef
} from '@ng-bootstrap/ng-bootstrap';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import {
  AgendamentoATMCCModel,
  AgendamentosResponse,
  FechamentoProps,
  StatusDescription
} from './model/AgendamentoATMCC';
import { Permissoes } from '@permissoes';
import { AtmService } from '@core/services/relatorios/atm/atm.service';
import { RelatorioAtmProps } from '@relatorios/models/relatorio-atm';
import { StatusAgendamentoATMCC } from './status-agendamento.enum';
import { AgendamentoCCModalComponent } from './agendamento-cc-modal/agendamento-cc-modal.component';

@Component({
  selector: 'app-relatorio-atm-cc',
  templateUrl: './relatorio-atm-cc.component.html',
  styleUrls: ['./relatorio-atm-cc.component.scss'],
  providers: [
    { provide: NgbDateAdapter, useClass: NgbDateNativeAdapter },
    { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }
  ]
})
export class RelatorioATMCCComponent implements OnInit {
  @ViewChild('novoRelatorioModal', { static: false }) modal: any;

  modalRef: NgbModalRef;
  public tituloPagina = 'Relatório ATM Cível Consumidor';
  public caminhoPagina: string;
  agendamentos: AgendamentoATMCCModel[] = [];
  totalAgendamentos: number = 0;
  breadcrumb: string;
  estados: RelatorioAtmProps[] = [];
  itemsToShow = 5;
  hasMoreItems = false;
  visibleAgendamentos: AgendamentoATMCCModel[] = [];

  @ViewChild(SisjurPaginator, { static: true }) paginator: SisjurPaginator;

  novoAgendamentoForm: FormGroup;
  fechamentos: FechamentoProps[] = [];
  indices: any[] = [];
  indicesPorEstado: any[] = [];

  constructor(
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService,
    private fb: FormBuilder,
    private atmService: AtmService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.inicializarFormulario();
    this.obterEstados();
    this.obterAgendamentos(1);
    this.getBreadcumb();

    this.novoAgendamentoForm.get('fechamento').valueChanges.subscribe();
  }

  async getBreadcumb() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_ATM_CC
    );
  }

  obterAgendamentos(pagina: number): void {
    this.atmService.getObterAgendamentos(pagina).subscribe(
      (response: AgendamentosResponse) => {
        this.agendamentos = response.data;
        this.totalAgendamentos = response.total;

        this.agendamentos.sort((a, b) => {
          return (
            new Date(b.datSolicitacao).getTime() -
            new Date(a.datSolicitacao).getTime()
          );
        });
        this.updateVisibleAgendamentos();
      },
      error => console.error('Erro ao buscar agendamentos', error)
    );
  }

  getStatusDescription(statusId: number): string {
    const description = StatusAgendamentoATMCC.obterTexto(statusId);
    if (description === undefined) {
      console.warn('Descrição não encontrada para o status ID:', statusId);
      return 'Desconhecido';
    }
    return description;
  }

  getStatusClass(statusId: number): string {
    switch (statusId) {
      case StatusAgendamentoATMCC.AGENDADO.id:
        return 'status-agendado';
      case StatusAgendamentoATMCC.PROCESSANDO.id:
        return 'status-processando';
      case StatusAgendamentoATMCC.FINALIZADO.id:
        return 'status-finalizado';
      case StatusAgendamentoATMCC.ERRO.id:
        return 'status-erro';
      default:
        return '';
    }
  }

  inicializarFormulario() {
    this.novoAgendamentoForm = this.fb.group({
      fechamento: [null],
      indices: this.fb.group({})
    });
  }

  updateVisibleAgendamentos() {
    this.visibleAgendamentos = this.agendamentos.slice(0, this.itemsToShow);
    this.hasMoreItems =
      this.fechamentos.length > this.visibleAgendamentos.length;
  }

  obterEstados() {
    this.atmService.getObterEstados().subscribe(
      (data: RelatorioAtmProps[]) => {
        this.estados = data;
        this.inicializarIndicesForm();
      },
      error => console.error('Erro ao buscar estados', error)
    );
  }

  mostrarMais() {
    this.itemsToShow += 5;
    this.updateVisibleAgendamentos();
  }

  inicializarIndicesForm() {
    if (this.estados.length && this.indices.length) {
      const indicesGroup = this.estados.reduce((group, estado) => {
        group[estado.codEstado] = [''];
        return group;
      }, {});

      this.novoAgendamentoForm.setControl(
        'indices',
        this.fb.group(indicesGroup)
      );
      this.indicesPorEstado = this.estados.map(estado => ({
        uf: estado.codEstado,
        opcoes: this.indices.map(indice => ({
          id: indice.codIndice,
          descricao: `${indice.nomIndice} (${
            indice.indAcumulado == 'S' ? 'Acumulado' : 'Mensal'
          })`
        }))
      }));

      this.cdr.detectChanges();
    }
  }

  resetForm() {
    const formValues = this.novoAgendamentoForm.getRawValue().indices;

    Object.keys(formValues).forEach(uf => {
      formValues[uf] = '';
    });

    this.novoAgendamentoForm.patchValue({
      indices: formValues
    });
  }

  fechar() {
    this.resetForm();
  }

  onDelete(codAgendRelatorioAtm: number) {
    if (confirm('Você tem certeza que deseja excluir este agendamento?')) {
      this.atmService.deletarAgendamento(codAgendRelatorioAtm).subscribe(
        response => {
          this.obterAgendamentos(1); // Recarregar a lista de agendamentos
        },
        error => console.error('Erro ao excluir agendamento', error)
      );
    }
  }

  async exportarBase(codAgendExecRelNegociacao: number) {
    try {
      return await this.atmService.downloadBase(codAgendExecRelNegociacao);
    } catch (error) {
      await this.dialog.err(
        `Erro ao baixar o agendamento ${codAgendExecRelNegociacao}`,
        error
      );
    }
  }

  async exportarRelatorio(codAgendExecRelNegociacao: number) {
    try {
      return await this.atmService.downloadRelatorio(codAgendExecRelNegociacao);
    } catch (error) {
      await this.dialog.err(
        `Erro ao baixar o agendamento ${codAgendExecRelNegociacao}`,
        error
      );
    }
  }

  async novoAgendamento() {
    
    const teveAlteracao: boolean = await AgendamentoCCModalComponent.exibeModal(
      this.fechamentos,
      this.indices
    );
    if (teveAlteracao) {
      
      this.obterAgendamentos(1);
    }
  }
}
