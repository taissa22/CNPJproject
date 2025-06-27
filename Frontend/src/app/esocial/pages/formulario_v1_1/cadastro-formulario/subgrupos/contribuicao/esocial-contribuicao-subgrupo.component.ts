import { AfterViewInit, Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { ErrorLib } from '@esocial/libs/error-lib';
import { Contribuicao } from '@esocial/models/subgrupos/contribuicao';
import { ContribuicaoService } from '@esocial/services/formulario_v1_1/subgrupos/contribuicao.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { DialogService } from '@shared/services/dialog.service';
import { ContribuicaoModalComponent } from '../../subgrupos-modals/contribuicao/contribuicao-modal.component';

@Component({
  selector: 'app-esocial-contribuicao-subgrupo',
  templateUrl: './esocial-contribuicao-subgrupo.component.html',
  styleUrls: ['./esocial-contribuicao-subgrupo.component.scss']
})
export class EsocialContribuicaoSubgrupoComponent implements AfterViewInit {

  @Input() codF2501: number;
  @Input() codCalTrib: number;
  @Input() temPermissaoBlocoCeDeE: boolean;

  @Output() aoAbrirPopup = new EventEmitter();

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  total: number = 0;
  dataSource: Array<Contribuicao>;
  temDados: boolean = false;

  constructor(
    private service: ContribuicaoService,
    private dialog: DialogService
  ) { }

  ngAfterViewInit(): void {
    this.buscarTabela();
  }

 async buscarTabela() {
    let view = this.iniciaValoresDaView();
    try {
    const data = await this.service
      .obterPaginado(
        this.codF2501,
        this.codCalTrib,
        view.pageIndex,
        view.sortColumn,
        view.sortDirection == 'asc'
      );

      this.total = data.total;
      this.temDados = this.total > 0;
      this.dataSource = data.lista;

    }catch (error) {
      console.log(error)
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      this.dialog.err(
        'Informações não carregadas',
        'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
      );
      this.dataSource = []; // ou atribua o valor desejado em caso de erro
    }
  }

  async incluir(): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ContribuicaoModalComponent.exibeModalIncluir(this.codF2501, this.codCalTrib);

    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  async alterar(codInfoContrib: number): Promise<void> {
    this.emitAoAbrirPopup();
    const teveAlteracao: boolean = await ContribuicaoModalComponent.exibeModalAlterar(this.codF2501, this.codCalTrib, codInfoContrib, this.temPermissaoBlocoCeDeE);
    if (teveAlteracao) {
      this.buscarTabela();
    }
  }

  private emitAoAbrirPopup() {
    if (this.temPermissaoBlocoCeDeE) {
      this.aoAbrirPopup.emit();
    }
  }

  async excluir(codInfoContrib: number, descInfoContrib : string): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Contribuição Social',
      `Deseja excluir o Código receita (CR) contrib.
      sociais<br><b>${descInfoContrib.substring(0,100)}</b>?`
    );

    if (!excluir) {
      return;
    }

    try {
      await this.service.excluir(this.codF2501, this.codCalTrib, codInfoContrib);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Código receita (CR) contrib. sociais!'
      );
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

  async exportar() {
    try {
      let view = this.iniciaValoresDaView();
      return await this.service.exportarListaAsync(this.codF2501,this.codCalTrib, view.sortDirection == 'asc');
    } catch (error) {
      const mensagem = ErrorLib.ConverteMensagemErro(error);
      return await this.dialog.err("Erro ao exportar lista", mensagem);
    }
  }
}
