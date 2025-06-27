import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { NgbDateAdapter, NgbDateNativeAdapter, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { Permissoes } from '@permissoes';
import { NgbDateCustomParserFormatter } from '@shared/dateformat';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import moment from 'moment';
import { AgendamentoLogProcessoModel } from './models/AgendamentoLogProcesso.model';
import { AgendamentoLogProcessoService } from './services/agendamento-log-processo.service';

@Component({
  selector: 'app-log-de-processos',
  templateUrl: './log-de-processos.component.html',
  styleUrls: ['./log-de-processos.component.scss'],
  providers: [{ provide: NgbDateAdapter, useClass: NgbDateNativeAdapter }, { provide: NgbDateParserFormatter, useClass: NgbDateCustomParserFormatter }]
})
export class LogDeProcessosComponent implements OnInit, AfterViewInit {
  dataIniFormControl: FormControl = new FormControl();
  dataFimFormControl: FormControl = new FormControl();
  total = 0;
  agendamentos = new Array<AgendamentoLogProcessoModel>();
  breadcrumb: string;
  @ViewChild(SisjurPaginator, { static: true }) paginator: SisjurPaginator;
  constructor(public service: AgendamentoLogProcessoService, private dialog: DialogService, private breadcrumbsService: BreadcrumbsService) { }

  ngOnInit() {

  }

  async ngAfterViewInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_RELATORIO_LOG);
  }

  listarAgendamentos(formControl = null) {
    if (formControl != null && formControl.value != null) {
      if (typeof (formControl.value) == "string") return false;
      if (formControl.value.getFullYear() < 1000) return false;
    }
    this.service.obterPaginado(this.paginator.pageIndex + 1, this.paginator.pageSize, this.dataIniFormControl.value ? moment(this.dataIniFormControl.value).format(moment.HTML5_FMT.DATE) : "", this.dataFimFormControl.value ? moment(this.dataFimFormControl.value).format(moment.HTML5_FMT.DATE) : "").subscribe(res => {
      this.agendamentos = res.data;
      this.total = res.total;
    });
  }

  async remover(obj: AgendamentoLogProcessoModel) {

    const excluir: boolean = await this.dialog.confirm(
      'Excluir Agendamento',
      `Deseja excluir este Agendamento?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.deletar(obj.id);
      await this.dialog.alert(`Exclusão realizada com sucesso`, `Agendamento excluido!`);
      this.listarAgendamentos();
    } catch (error) {
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }
  mascaraDataKeyUp(formControl, id: string) {
    if (typeof (formControl.value) != "string") return false;
    let el: any = document.getElementById(id);
    if (formControl.value.length == 2) {
      el.value = formControl.value + "/";
    }
    else if (formControl.value.length == 5) {
      el.value = formControl.value + "/";
    }
  }

  resetarFormControlInvalido(formControl) {
    if (formControl.invalid) {
      formControl.reset();
    }
  }
}
