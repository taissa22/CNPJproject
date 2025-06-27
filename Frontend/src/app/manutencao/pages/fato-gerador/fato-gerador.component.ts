import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { FatoGeradorModalComponent } from '@manutencao/modals/fato-gerador-modal/fato-gerador-modal.component';
import { FatoGerador } from '@manutencao/models/fato-gerador.model';
import { FatoGeradorService } from '@manutencao/services/fato-gerador.service';
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';
import { EMPTY } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Component({
  selector: 'app-fato-gerador',
  templateUrl: './fato-gerador.component.html',
  styleUrls: ['./fato-gerador.component.scss']
})
export class FatoGeradorComponent implements AfterViewInit{
  dataSource: Array<FatoGerador> = [];
  total: number = 0;
  
  @ViewChild(SisjurTable, { static: true }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: true }) paginator: SisjurPaginator;
  constructor(private service: FatoGeradorService, private dialog: DialogService) { }

  
  buscarDescricaoFormControl : FormControl = new FormControl(null);

  ngAfterViewInit(): void {
      this.buscarTabela();
  }
  
  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "nome" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};    

    this.service.obter(sort, page, this.buscarDescricaoFormControl.value).pipe(
              catchError(err => {
                console.log(err);
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            ).subscribe(data => {
                  this.dataSource = data.data;
                  this.total = data.total;
                });;
  }

  exportar() {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
   
    this.service.exportar(sort,this.buscarDescricaoFormControl.value);
  }

  async incluir() {
    const teveAlteracao: boolean = await FatoGeradorModalComponent.exibeModalDeIncluir();
   if (teveAlteracao) {
     this.buscarTabela();
   }
 }

  async alterar(fato : FatoGerador) {
     const teveAlteracao: boolean = await FatoGeradorModalComponent.exibeModalDeAlterar(fato);
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async remover(fato: FatoGerador) {

    const excluir: boolean = await this.dialog.confirm(
      'Excluir Fato Gerador',
      `Deseja excluir o Fato Gerador <br><b>${fato.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(fato.id);
      await this.dialog.alert(`Exclusão realizada com sucesso`, `Fato Gerador excluido!`);
      this.buscarTabela();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionado')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));

    }
  }

  onClearInputPesquisar(){ if(!this.buscarDescricaoFormControl.value){ this.buscarTabela()  } }

}
