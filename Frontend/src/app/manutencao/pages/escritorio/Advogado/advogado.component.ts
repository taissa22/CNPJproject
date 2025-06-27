import { Permissoes } from 'src/app/permissoes/permissoes';
// angular
import { AfterViewInit, Component, ViewChild, OnDestroy, OnInit, ValueProvider, Input } from '@angular/core';

// 3rd party
import { BehaviorSubject, EMPTY, merge, Observable } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';

// core & shared imports
import { DialogService } from '@shared/services/dialog.service';
import { Page } from '@shared/types/page';
import { Sort, SortOf } from '@shared/types/sort';

// local imports
import { FormControl } from '@angular/forms';
import { PermissoesService } from '@permissoes';
import { HttpErrorResult } from '@core/http';
import { faWindowRestore } from '@fortawesome/free-solid-svg-icons';
import { TiposProcesso } from '@manutencao/models/tipos-de-processos';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';
import { AdvogadoService } from '@manutencao/services/advogado.service';
import { Advogado } from '@manutencao/models/advogado.model';
import { AdvogadoModalComponent } from './advogado-modal/advogado-modal.component';
import { Escritorio } from 'src/app/processos/agendaDeAudienciasDoCivelEstrategico/models';
import { Estados } from '@core/models';

@Component({
  selector: 'app-advogado',
  templateUrl: './advogado.component.html',
  styleUrls: ['./advogado.component.scss']
})
export class AdvogadoComponent implements OnInit {
  breadcrumb: string;
  constructor(
    private service: AdvogadoService,
    private dialog: DialogService,
    private permisoesService: PermissoesService
  ) {}


  advogados: Array<Advogado> =[];
  escritorioNome : string = "";
  estados: Array<Estados> = Estados.obterUfs();

  @Input() escritorio : Escritorio;

  total: number = 0;

  NomeFormControl : FormControl = new FormControl(null);
  estadoFormControl : FormControl = new FormControl(null);

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;


  ngOnInit(): void {
    if (this.escritorio){
      this.escritorioNome = this.escritorio.nome;
    }
  }


  buscarTabela(): void {
    let sort: SortOf<any> = { column: this.iniciaValoresDaView().sortColumn , direction: this.iniciaValoresDaView().sortDirection};
    const page: Page = { index: this.iniciaValoresDaView().pageIndex, size: this.iniciaValoresDaView().pageSize};

    this.service.obter(this.escritorio.id,sort,page,this.estadoFormControl.value,this.NomeFormControl.value).pipe(
              catchError(err => {
                console.log(err);
                this.dialog.err(
                  'Informações não carregadas',
                  'Não foi possível carregar as informações, tente novamente.<br>Se o erro persistir, contate o suporte'
                );
                return EMPTY;
              })
            ).subscribe(data => {
                  this.advogados = data.data;
                  this.total = data.total;
                });;
  }

  iniciaValoresDaView() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "Nome" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  selecionarTipoProcesso() {
    this.NomeFormControl.setValue('') ;

    this.buscarTabela() ;
  }

  async incluirAdvogado(podeIncluir : boolean = false): Promise<void> {
    
    if (podeIncluir){
      const advogado = this.advogados.find(x => x.ehContato && x.id != this.escritorio.id);
      const temContato = advogado ? true : false;

      const teveAlteracao: boolean = await AdvogadoModalComponent.exibeModalDeIncluir(this.escritorio.id,temContato);
      this.NomeFormControl.setValue('');
      if (teveAlteracao) { this.buscarTabela()}
    }
  }

 async alterar(item: Advogado): Promise<void> {
    const advogado = this.advogados.find(x => x.ehContato && x.id != this.escritorio.id);
    const temContato = advogado ? true : false;

    const teveAlteracao: boolean = await AdvogadoModalComponent.exibeModalDeAlterar(item,this.escritorio.id,temContato);
    this.NomeFormControl.setValue('');
    if (teveAlteracao) { this.buscarTabela()}
  }



  async excluir(item: Advogado): Promise<void> {
    const excluir: boolean = await this.dialog.confirm(
      'Excluir Advogado',
      `Deseja excluir o Advogado<br><b>${item.nome}</b>?`
    );

    if (!excluir) { return; }

    try {
      await this.service.excluir(item.id,this.escritorio.id);
      await this.dialog.alert(
        'Exclusão realizada com sucesso',
        'Advogado excluído!'
      );
      this.NomeFormControl.setValue('');
      this.buscarTabela();
    } catch (error) {
      if ((error as HttpErrorResult).messages.join().includes('registrado')) {
        await this.dialog.info(`Exclusão não permitida`, (error as HttpErrorResult).messages.join('\n'));
        return;
      }
      await this.dialog.err(`Exclusão não realizada`, (error as HttpErrorResult).messages.join('\n'));
    }
  }

  exportar( ){
    let sort: SortOf<any>;
    sort = { column: 'descricao', direction: 'asc' };
    sort = { column: this.iniciaValoresDaView().sortColumn, direction: this.iniciaValoresDaView().sortDirection };
    const page: Page = { index: this.paginator.pageIndex, size: this.paginator.pageSize};
    this.service.exportar(sort,page,this.estadoFormControl.value , this.NomeFormControl.value);
  }


  onClearInputPesquisar(){ if(!this.NomeFormControl.value){ this.buscarTabela()  } }
}
