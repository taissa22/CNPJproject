import { DialogService } from '@shared/services/dialog.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Documento } from '../models/documento.model';
import { Component, OnInit } from '@angular/core';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';
import { CargaDeCompromissosService } from '../services/carga-de-compromissos.service';
import { DatePipe } from '@angular/common';
import { AgendamentoCargaCompromissoModalComponent } from './modal/agendamento-carga-compromisso-modal/agendamento-carga-compromisso-modal.component';
import { Compromisso } from '../models/compromisso.model';
import { FormControl, FormGroup } from '@angular/forms';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { TipoDeAudienciaService } from '@manutencao/services/tipo-de-audiencia.service';
import { debug } from 'console';

@Component({
  templateUrl: './carga-de-compromissos.component.html',
  styleUrls: ['./carga-de-compromissos.component.scss']
})
export class CargaDeCompromissosComponent implements OnInit {
  titulo: string = 'Carga de Compromissos RJ';
  breadcrumb: string = '';

  tiposDeProcesso: TiposProcesso[] = [];

  compromissos: Array<Compromisso> = [];
  totalCompromissos: number = 0;
  pageIndex: number = 0;
  pageSize: number = 0;
  dataInicio: Date;
  dataFinal: Date;

  dataInicioAgendamentoFormControl = new FormControl();
  dataFimAgendamentoFormControl = new FormControl();

  buscaFormGroup: FormGroup = new FormGroup({
    dataInicioAgendamento: this.dataInicioAgendamentoFormControl,
    dataFimAgendamento: this.dataFimAgendamentoFormControl
  });

  constructor(
    private service: CargaDeCompromissosService,
    private dialog: DialogService,
    private datePipe: DatePipe,
    private breadcrumbsService: BreadcrumbsService,
    private serviceTipoDeAudiencia: TipoDeAudienciaService
  ) {}

  async ngOnInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(
      Permissoes.ACESSAR_CARGA_COMPROVANTE_PAGAMENTO_RJ
    );
    
    await this.loadTipoProcesso();  
  }

  async loadTipoProcesso()
  {
    const tpsProcessos = await this.serviceTipoDeAudiencia.getTiposDeProcesso();
    
    this.tiposDeProcesso = tpsProcessos.filter((processo: any) => {
      const allowedIds = [
        'Cível Consumidor',
        'Cível Estratégico',
        'Juizado Especial Cível',
        'Pex'
      ];
      
      return allowedIds.includes(processo.descricao);
    });
  }

  async ngAfterViewInit(): Promise<void> {
    await this.loadTipoProcesso();
  }

  async obterPaginado(): Promise<void> {
    try {
      let dtIni = this.datePipe.transform(
        this.dataInicioAgendamentoFormControl.value,
        'yyyy-MM-dd'
      );
      let dtFim = this.datePipe.transform(
        this.dataFimAgendamentoFormControl.value,
        'yyyy-MM-dd'
      );

      const response = await this.service.obterPaginado(
        this.pageIndex,
        this.pageSize,
        dtIni,
        dtFim
      );

      this.totalCompromissos = response.total;
      console.log(this.totalCompromissos);

      // Usando Promise.all para aguardar todas as chamadas assíncronas
      const compromissosPromises = response.data.map(async m => {
        const tipoProcesso = await this.obterTipoProcesso(m.tipoProcesso);
        return Compromisso.fromJson(m, tipoProcesso);
      });

      // Aguarda a resolução de todas as promises
      this.compromissos = await Promise.all(compromissosPromises);
    } catch (error) {
      console.error(error);
      await this.dialog.showErr(
        'Operação não realizada',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  async obterTipoProcesso(tipoProcesso: string): Promise<string> {
    debugger;
    if (this.tiposDeProcesso.length == 0)
    {
      await this.loadTipoProcesso();
    }

    if (tipoProcesso) {
      const tipoProcessoNumero = parseInt(tipoProcesso.trim(), 10);

      const tipo = this.tiposDeProcesso.find(t => t.id === tipoProcessoNumero);

      return tipo ? tipo.descricao : '';
    }

    return '';
  }

  async novoAgendamento(): Promise<void> {
    const changes =
      await AgendamentoCargaCompromissoModalComponent.exibeModal();
    if (changes) this.refresh();
  }

  async refresh(): Promise<void> {
    //this.pageIndex = 0;
    await this.obterPaginado();
  }

  async obterMaisCompromissos(): Promise<void> {
    //this.pageIndex += 1;
    console.log('index: ', this.pageIndex, 'Size: ', this.pageSize);
    await this.obterPaginado();
  }

  setPageIndex(index: number): void {
    this.pageIndex = index;
  }

  setPageSize(size: number): void {
    this.pageSize = size;
  }

  async redefinirListagem(): Promise<void> {
    this.compromissos = [];
    //this.pageIndex = 0;

    await this.obterPaginado();
  }

  async excluir(id: number | string): Promise<void> {
    try {
      await this.service.excluir(id);
    } catch (error) {
      console.error(error);
      await this.dialog.showErr(
        'Operação não realizada',
        (error as HttpErrorResult).messages.join('\n')
      );
      return;
    }
    await this.redefinirListagem();
  }
}
