import { DatePipe } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { InfoInterm } from '@esocial/models/subgrupos/v1_2/InfoInterm';
import { InfoIntermService } from '@esocial/services/formulario/subgrupos/infointerm.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { PermissoesService } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { DialogService } from '@shared/services/dialog.service';
import { InfoIntermModalComponent } from './info-interm-modal/info-interm-modal.component';

@Component({
  selector: 'app-info-interm',
  templateUrl: './info-interm.component.html',
  styleUrls: ['./info-interm.component.scss']
})
export class InfoIntermComponent implements OnInit {

  @Input() codF2500: number;
  @Input() contratoId: number;
  @Input() codIdePeriodo: number;
  @Input() consulta: boolean = false;
  @Input() periodo: Date;

  @Output() carregado = new EventEmitter();
  
  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<InfoInterm>;

  constructor(
    private service : InfoIntermService,
    private breadcrumbsService: BreadcrumbsService,
    private dialog : DialogService,
    private elementRef: ElementRef,
    private permissaoService: PermissoesService,
    private datePipe: DatePipe
  ) { }

  ngOnInit() {
  }

  async incluir(): Promise<void> {
    if (!this.codIdePeriodo) {
      await this.dialog.err('Inclusão Trabalho Intermitente', 'Só é possível incluir Trabalho Intermitente após incluir o Período!');
      return;
    }
    const teveAlteracao: boolean = await InfoIntermModalComponent.exibirModalIncluir(this.codF2500, this.contratoId, this.codIdePeriodo, this.periodo);

    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async alterar(item: InfoInterm): Promise<void> {
    const teveAlteracao: boolean = await InfoIntermModalComponent.exibirModalAlterar(this.codF2500, this.contratoId, item,this.codIdePeriodo, this.consulta, this.periodo);
    if (teveAlteracao) {
      await this.buscarTabela();
      this.carregado.emit(true);
    }
  }

  async excluir(codigoInfoInterm: number, dia: number): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Trabalho Intermitente',
      `Deseja excluir o dia<br><b>${dia}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.codF2500, this.contratoId, this.codIdePeriodo, codigoInfoInterm);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Trabalho Intermitente excluído!'
      );
      await this.buscarTabela();
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      if (mensagem.includes('registrado')) {
        await this.dialog.info(
          `Exclusão não permitida`,
          mensagem
        );
        return;
      }
      await this.dialog.err(
        `Exclusão não realizada`,
        mensagem
      );
    }
  }

  async buscarTabela() {
    let view = this.iniciaValoresDaView();
    try {
      this.service.obterPaginado(this.codIdePeriodo, view.pageIndex).then(x => {
        this.dataSource = x.lista;
        this.total = x.total
      });
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialog.err(
        'Informações não carregadas',
        mensagem
      );
      this.dataSource = []; // ou atribua o valor desejado em caso de erro
    }
}

iniciaValoresDaView() {
  let sort: any =
    this.table === undefined || !this.table.sortColumn
      ? 'id'
      : this.table.sortColumn;
  return {
    sortColumn: sort,
    sortDirection:
      this.table === undefined || !this.table.sortDirection
        ? 'asc'
        : this.table.sortDirection,
    pageIndex:
      this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
    pageSize: 10
  };
}

}
