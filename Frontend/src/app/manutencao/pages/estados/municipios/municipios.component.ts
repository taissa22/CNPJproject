import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { HttpErrorResult } from '@core/http';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { MunicipioModalComponent } from '@manutencao/modals/municipio-modal/municipio-modal.component';
import { Estado } from '@manutencao/models/estado.model';
import { Municipio } from '@manutencao/models/municipio.model';
import { MunicipioService } from '@manutencao/services/municipio.service';
import { DialogService } from '@shared/services/dialog.service';


@Component({
  selector: 'municipios',
  templateUrl: './municipios.component.html',
  styleUrls: ['./municipios.component.scss']
})
export class MunicipiosComponent{
  @Input() estado: Estado;
  municipioPesquisadoFormControl: FormControl = new FormControl();
  total: number = 0;
  municipios: Array<Municipio> = [];
  municipiosSelect: Array<Municipio> = [];

  @ViewChild(SisjurTable, { static: true }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: true }) paginator: SisjurPaginator;
  constructor(private service: MunicipioService, private dialog: DialogService) { }

  pesquisar() {
    let view = this.iniciaValoresDaView();
    this.service.obterPaginado(view.pageIndex, view.pageSize, this.estado.id, this.municipioPesquisadoFormControl.value ? this.municipioPesquisadoFormControl.value : "", view.sortColumn, view.sortDirection)
      .subscribe(resultado => {
        this.municipios = resultado.data;
        this.total = resultado.total;
      });
  }

  iniciaValoresDaView() {
    let sort: any = this.table === undefined || !this.table.sortColumn ? "id" : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  async alterar(municipio: Municipio) {
    const teveAlteracao: boolean = await MunicipioModalComponent.exibeModalDeAlterar(municipio);
    if (teveAlteracao) this.pesquisar(); 
  }

  async criar() {
    const teveAlteracao: boolean = await MunicipioModalComponent.exibeModalDeCriar(this.estado);
    if (teveAlteracao) this.pesquisar(); 
  }

  async remover(municipio: Municipio) {

    const excluir: boolean = await this.dialog.confirm(
      'Excluir Município',
      `Deseja excluir o município <br><b>${municipio.nome}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.remover(municipio.id);
      await this.dialog.alert(`Exclusão realizada com sucesso`, `Município excluido!`);
      this.pesquisar();

    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('relacionado')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));

    }
  }
}
