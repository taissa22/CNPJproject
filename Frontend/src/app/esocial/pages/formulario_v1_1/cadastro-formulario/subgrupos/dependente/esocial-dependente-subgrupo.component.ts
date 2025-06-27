import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild
} from '@angular/core';
import { FormControl } from '@angular/forms';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Dependente } from '@esocial/models/subgrupos/dependente';
import { DependenteService } from '@esocial/services/formulario_v1_1/subgrupos/dependente.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { DependenteModalComponent } from '../../subgrupos-modals/dependente/dependente-modal.component';


@Component({
  selector: 'app-esocial-dependente-subgrupo',
  templateUrl: './esocial-dependente-subgrupo.component.html',
  styleUrls: ['./esocial-dependente-subgrupo.component.css']
})
export class EsocialDependenteSubgrupoComponent implements AfterViewInit {
  breadcrumb: string;
  constructor(
    private service: DependenteService,
    private dialog: DialogService
  ) {}

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  @Input() formularioId: number;
  @Input() temPermissaoEsocialBlocoABCDEFHI: boolean;

  @Input() cpfTrabalhador: string;

  @Output() carregado = new EventEmitter();
  @Output() aoAbrirPopup = new EventEmitter();

  total: number = 0;
  dataSource: Array<Dependente> = [];

  buscarDescricaoFormControl: FormControl = new FormControl(null);
  codigoFormularioFormControl: FormControl = new FormControl(null);

  async ngAfterViewInit(): Promise<void> {
    this.codigoFormularioFormControl.setValue(this.formularioId);
    await this.buscarTabela();

  }

  iniciaValoresDaView() {
    let sort: any =
      this.table === undefined || !this.table.sortColumn
        ? 'descricao'
        : this.table.sortColumn;
    return {
      sortColumn: sort,
      sortDirection:
        this.table === undefined || !this.table.sortDirection
          ? 'asc'
          : this.table.sortDirection,
      pageIndex:
        this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize
    };
  }

  buscarTabela() {
    if (this.codigoFormularioFormControl.value > 0) {
      let view = this.iniciaValoresDaView();
      this.service
        .obterPaginado(
          this.codigoFormularioFormControl.value,
          view.pageIndex,
          view.sortColumn,
          view.sortDirection == 'asc'
        )
        .then(data => {
          this.total = data.total;
          this.dataSource = data.lista;
          this.carregado.emit(true);
        })
        .catch((error) => {
          const mensagem = ErrorLib.ConverteMensagemErro(error);
          this.dialog.err(
            'Informações não carregadas',
            mensagem
          );
        });
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await DependenteModalComponent.exibeModal(
      null,
      this.formularioId,
      this.cpfTrabalhador,
      true
    );

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  async alterar(item: Dependente): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await DependenteModalComponent.exibeModal(
      item,
      this.formularioId,
      this.cpfTrabalhador,
      this.temPermissaoEsocialBlocoABCDEFHI
    );

    if (teveAlteracao) {
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
    }
  }

  private emitAoAbrirPopup() {
    if (this.temPermissaoEsocialBlocoABCDEFHI) {
      this.aoAbrirPopup.emit();
    }
  }

  async excluir(item: Dependente): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Dependente',
      `Deseja excluir o Dependente<br><b>${item.dependenteCpfdep}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(item.idEsF2500Dependente, item.idF2500);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Dependente excluído!'
      );
      this.buscarDescricaoFormControl.setValue('');
      this.buscarTabela();
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

  onClearInputPesquisar() {
    if (!this.buscarDescricaoFormControl.value) {
      this.buscarTabela();
    }
  }
}

