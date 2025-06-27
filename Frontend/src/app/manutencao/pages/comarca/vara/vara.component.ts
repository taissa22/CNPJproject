import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { VaraModalComponent } from '@manutencao/modals/vara-modal/vara-modal.component'
import { Comarca } from '@manutencao/models/comarca.model';
import { Vara } from '@manutencao/models/vara.model';
import { VaraService } from '@manutencao/services/vara.service';
import { JurPaginator } from '@shared/components/jur-paginator/jur-paginator.component';
import { JurTable } from '@shared/components/jur-table/jur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';
import { EMPTY, merge } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

@Component({
  selector: 'vara',
  templateUrl: './vara.component.html',
  styleUrls: ['./vara.component.scss']
})
export class VaraComponent {

  textoPesquisadoFormControl: FormControl = new FormControl("");
  varas: Array<Vara> = new Array<Vara>();
  total: number = 0;
  sort: Sort;

  @Input() comarca: Comarca;
  @Output() varasCarregadas: EventEmitter<Vara[]> = new EventEmitter();
  constructor(private dialog: DialogService, private serviceVara: VaraService) { }

  @ViewChild(JurPaginator, { static: false }) paginator: JurPaginator;
  @ViewChild(JurTable, { static: false }) table: JurTable<Vara>;


  buscar() {
    let view = this.iniciaValoresDaView();
    let sortColumn: any = view.sortColumn;
    this.serviceVara.obterPaginado(this.comarca.id, view.pageIndex, view.pageSize, sortColumn, view.sortDirection, this.textoPesquisadoFormControl.value)
      .pipe(
        catchError(() => {
          this.dialog.err(
            'Informações não carregadas',
            'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
          );
          return EMPTY;
        })
      )
      .subscribe(resposta => {
        this.varas = resposta.data;
        this.total = resposta.total;
        this.varasCarregadas.emit(this.varas);
      });
  }

 
  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "Id" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async incluirVara(): Promise<void> {
    const teveAlteracao: boolean = await VaraModalComponent.exibeModalDeIncluir(this.comarca);
    if (teveAlteracao) this.buscar();
  }

  async alterarVara(registro: any): Promise<void> {
    const teveAlteracao: boolean = await VaraModalComponent.exibeModalDeAlterar(registro, this.comarca);
    if (teveAlteracao) this.buscar();
  }

  onClearInputPesquisar(){
    if(!this.textoPesquisadoFormControl.value){
      this.buscar()
    }
 }

  async excluirVara(vara: Vara) {

    const excluir: boolean = await this.dialog.confirm(
      'Excluir Vara',
      `Deseja excluir a vara <br><b>${vara.tipoVara.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {

      await this.serviceVara.remover(vara.varaId, vara.comarcaId, vara.tipoVaraId);
      await this.dialog.alert(`Exclusão realizada com sucesso`, `Vara excluida!`);
      this.buscar();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionada')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));

    }
  }
}
