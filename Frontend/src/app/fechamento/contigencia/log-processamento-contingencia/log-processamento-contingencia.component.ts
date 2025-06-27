import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { PermissoesAlteracaoProcessoBlocoWebService } from '@core/services/alteracao-processo-bloco/permissoes-alteracao-processo-bloco.service';
import { SisjurPaginator } from '@libs/sisjur/sisjur-paginator/sisjur-paginator.component';
import { SisjurTable } from '@libs/sisjur/sisjur-table/sisjur-table.component';
import { Permissoes } from '@permissoes';
import { HelperAngular } from '@shared/helpers/helper-angular';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Page } from '@shared/types/page';
import { SortOf } from '@shared/types/sort';
import { LogProcessamentoContigenciaModel } from '../../models/contigencia/log-processamento-contingencia.model';
import { LogProcessamentoContingenciaService } from '../../services/log-processamento-contingencia.service';

@Component({
  selector: 'app-log-processamento-contingencia',
  templateUrl: './log-processamento-contingencia.component.html',
  styleUrls: ['./log-processamento-contingencia.component.scss']
})
export class LogProcessamentoContingenciaComponent implements OnInit, AfterViewInit {

  @ViewChild(SisjurTable, { static: false }) table: SisjurTable;
  @ViewChild(SisjurPaginator, { static: false }) paginator: SisjurPaginator;

  constructor(
    private messageService: HelperAngular,
    private service: LogProcessamentoContingenciaService,
    private permissao: PermissoesAlteracaoProcessoBlocoWebService,
    private breadcrumbsService: BreadcrumbsService) { }

  ngOnInit() {
    this.validarPermissao();
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_CONSULTAR_LOGS_FECHAMENTO_CONTINGENCIA);
    this.obterDados();
  }

  //#region Variaveis
  breadcrumb: string;
  dropdownList = [];
  selectedItems = [];
  dataSelecionada = [];
  dataInicio = new Date();
  dataFim = new Date();
  dataAtual = new Date();
  dadosLog: Array<LogProcessamentoContigenciaModel>;
  totalLog: number = 0;
  //#endregion

  validarPermissao() {
    let logar = false;

    if (this.permissao.verificarPermissao('f_SolicitarConsultarFechContingCC') || this.permissao.verificarPermissao('f_SolicitarConsultarFechContingCCMedia')) {
      this.dropdownList.push({ id: 1, nome: 'Cível Consumidor' })
      this.selectedItems.push(1);
      logar = true;
    }
    if (this.permissao.verificarPermissao('f_SolicitarConsultarFechContingCE')) {
      this.dropdownList.push({ id: 6, nome: 'Cível Estratégico' })
      this.selectedItems.push(6);
      logar = true;
    }
    if (this.permissao.verificarPermissao('f_SolicitarConsultarFechContingJEC')) {
      this.dropdownList.push({ id: 49, nome: 'Juizado' })
      this.selectedItems.push(49);
      logar = true;
    }
    if (this.permissao.verificarPermissao('f_SolicitarConsultarFechContingTRAB')) {
      this.dropdownList.push({ id: 7, nome: 'Trabalhista' })
      this.selectedItems.push(7);
      logar = true;
    }
    if (this.permissao.verificarPermissao('f_SolicitarConsultarFechContingPEX')) {
      this.dropdownList.push({ id: 51, nome: 'Pex' })
      this.selectedItems.push(51);
      logar = true;
    }
    !logar ? this.messageService.MsgBox2('', 'Usuário não tem nenhuma permissão para os módulos', 'warning', 'Ok') : null;
  }

  obterDados() {
    let sort: SortOf<any> = { column: this.iniciaPaginator().sortColumn, direction: this.iniciaPaginator().sortDirection };
    const page: Page = { index: this.iniciaPaginator().pageIndex, size: this.iniciaPaginator().pageSize };
    let filtro = [];
    if (!this.selectedItems.find(x => x == 1 || x == 6 || x == 49 || x == 7 || x == 51)) {
      this.dadosLog = [];
      this.totalLog = 0
      return this.messageService.MsgBox2('', 'Nenhum tipo de processo foi selecionado.', 'warning', 'Ok')
    }
    let datas = this.validarData()
    if(!this.selectedItems.find(x => x == 11)){
      this.checkOpt();
    }
    for(let i = 0; i < this.selectedItems.length; i++) {
      filtro.push(this.selectedItems[i]);
    }
    if(this.dataSelecionada.find(x => x == 10)){
      filtro.push(10)
    }
    console.log(filtro)
    this.service.obterlog(filtro, sort.column, datas.dtIni, datas.dtFim, sort.direction == "asc" ? true : false, page.index - 1, page.size).subscribe(result => {
      this.totalLog = result.total.result
      this.dadosLog = result.log
    });
  }

  excluirFechamento(id) {
    this.messageService.MsgBox2('Deseja excluir o agendamento?', 'Excluir Agendamento',
      'question', 'Sim', 'Não').then(resposta => {
        if (resposta.value) {
          this.service.excluirProcesso(id).subscribe(() => {
            this.messageService.MsgBox2('', 'Agendamento excluido com sucesso', 'success', 'Ok');
            this.obterDados();
          })
        }
      }
      );
  }

  iniciaPaginator() {
    return {
      sortColumn: this.table === undefined || !this.table.sortColumn ? "tipo" : this.table.sortColumn,
      sortDirection: this.table === undefined || !this.table.sortDirection ? "asc" : this.table.sortDirection,
      pageIndex: this.paginator === undefined ? 1 : this.paginator.pageIndex + 1,
      pageSize: this.paginator === undefined ? 8 : this.paginator.pageSize,
    }
  }

  checkOpt() {
    let check = (document.getElementById('11') as HTMLInputElement).checked;

    if (check) {
      this.selectedItems.push(11)
    }
    else {
      var index = this.selectedItems.indexOf(11);
      if (index > -1)
        this.selectedItems.splice(index, 1);
    }
  }

  validarData() {
    let dtIni, dtFim = "";

    if (this.dataInicio && this.dataFim) {
      dtIni = this.dataInicio.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      dtFim = this.dataFim.toLocaleDateString("pt-BR").split('/').reverse().join('-');
      let inserir = this.dataSelecionada.find(x => x == 10);
      if (!inserir)
        this.dataSelecionada.push(10)
    }
    else {
      var index = this.dataSelecionada.indexOf(10);
      if (index > -1)
        this.dataSelecionada.splice(index, 1);
    }
    return { dtIni, dtFim }
  }

  mostrarErro(msg: string){
    this.messageService.MsgBox2(msg, 'Erro', 'warning', 'Ok')
  }
}
