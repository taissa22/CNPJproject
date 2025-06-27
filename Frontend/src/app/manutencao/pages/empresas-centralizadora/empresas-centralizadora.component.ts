import { Component, AfterViewInit } from '@angular/core';

import { List } from 'linqts';

import { DialogService } from '@shared/services/dialog.service';
import { SortEvent } from '@shared/directive/sortable.directive';

import { HttpErrorResult } from '@core/http';

import { EmpresaCentralizadoraModalComponent } from './empresa-centralizadora-modal/empresa-centralizadora-modal.component';
import { EmpresaCentralizadoraService } from '@manutencao/services/empresa-centralizadora.service';

import { EmpresaCentralizadora } from '@manutencao/models';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';
import { FormControl } from '@angular/forms';

import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  templateUrl: './empresas-centralizadora.component.html',
  styleUrls: ['./empresas-centralizadora.component.scss']
})
export class EmpresaCentralizadoraComponent implements AfterViewInit {

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: 'nome' | 'ordem' | 'codigo' | '' = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  registros: Array<EmpresaCentralizadora> = [];
  totalDeRegistros: number = 0;
  breadcrumb: string;

  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Ordem', 'ordem', true, '15%', 'ordem', { width: '16.3%', display: 'inline-block' }),
    new ColunaGenerica('Código', 'codigo', true, '25%', 'codigo', { width: '27.3%', display: 'inline-block' }),
    new ColunaGenerica('Empresa Centralizadora', 'nome', true, '57%', 'nome', { display: 'inline-block' })
  ];

  private ultimoFiltro: { ordem: number | '', codigo: number | '', nome: string } = { nome: '', codigo: '', ordem: '' };

  private openList: List<number> = new List<number>();

  buscarDescricaoFormControl : FormControl = new FormControl(null);
  
  constructor(
    private empresaCentralizadoraService: EmpresaCentralizadoraService, 
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }  

  onSort(sort: SortEvent): void {
    this.ordenacaoColuna = sort.column as 'nome' | 'ordem' | 'codigo' | '';
    this.ordenacaoDirecao = sort.direction as 'asc' | 'desc';

    if (sort.direction === '') {
      this.ordenacaoColuna = '';
    }

    this.obter();
  }

  isOpen(index: number): boolean {
    return this.openList.Contains(index);
  }

  toogle(index: number): void {
    if (this.isOpen(index)) {
      this.openList.Remove(index);
      return;
    }

    if (this.openList.Count() > 1) {
      this.openList.RemoveAt(0);
    }

    this.openList.Add(index);
  }

  private closeAll(): void {
    while (this.openList.Any()) {
      this.openList.RemoveAt(0);
    }
  }

  async ngAfterViewInit(): Promise<void> {
    await this.obter();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_EMPRESA_CENTRALIZADORA)
  }

  async obter(filtro?: { ordem: number | '', codigo: number | '', nome: string }): Promise<void> {
    if (filtro) {
      this.ultimoFiltro = filtro;
    }
    try {
      const result = await this.empresaCentralizadoraService.obterPaginado(
        this.pagina, this.totalDeRegistrosPorPagina,
        this.ordenacaoColuna, this.ordenacaoDirecao,
        this.buscarDescricaoFormControl.value, this.ultimoFiltro.ordem, this.ultimoFiltro.codigo);
      this.totalDeRegistros = result.total;
      this.registros = result.data;
    } catch (error) {
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.empresaCentralizadoraService.exportar(this.ordenacaoDirecao,this.buscarDescricaoFormControl.value);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    this.closeAll();
    try {
      await EmpresaCentralizadoraModalComponent.exibeModalDeCriar();
    } catch (error) {
      console.log(error);
    }
    if (!!this.ultimoFiltro) {
      await this.obter(this.ultimoFiltro);
    }
  }

  async abrirDialogoDeAtualizar(empresa: EmpresaCentralizadora): Promise<void> {
    this.closeAll();
    try {
      await EmpresaCentralizadoraModalComponent.exibeModalDeAlterar(empresa);
    } catch (error) {
      console.log(error);
    }
    if (!!this.ultimoFiltro) {
      await this.obter(this.ultimoFiltro);
    }
  }

  async remover(codigo: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Excluir Empresa Centralizadora', 'Deseja excluir a Empresa Centralizadora?');
    if (confirm) {
      try {
        await this.empresaCentralizadoraService.remover(codigo);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluido do sistema.');
      } catch (error) {
        const messages = (error as HttpErrorResult).messages.join('\n') + '.';
        const title = messages.startsWith('Não será possível') ? 'Exclusão não permitida' : 'Exclusão não realizada';

        this.dialog.showErr(title, messages);
      }
      this.obter(this.ultimoFiltro);
    }
  }

  exibirPaginacao() {
    return this.totalDeRegistros > this.totalDeRegistrosPorPagina;
  }

  
  onClearInputPesquisar(){
    this.obter();   
  }
}
