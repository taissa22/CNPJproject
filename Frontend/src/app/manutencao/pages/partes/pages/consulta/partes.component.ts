import { Permissoes } from './../../../../../permissoes/permissoes';
import { UserService } from './../../../../../core/services/user.service';
import { DialogService } from '@shared/services/dialog.service';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';

import { Parte, TipoParte } from '@manutencao/models';
import { Component, OnInit } from '@angular/core';
import { PartesService } from '@manutencao/services';

import { ManterParteComponent } from '../manter/manter.component';
import { HttpErrorResult } from '@core/http';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  templateUrl: './partes.component.html',
  styleUrls: ['./partes.component.scss']
})
export class PartesComponent implements OnInit {

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: 'nome' | 'tipoParte' | 'documento' | 'carteiraTrabalho' | '' = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  registros: Array<Parte> = [];
  totalDeRegistros: number = 0;
  breadcrumb: string;

  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Tipo', 'tipoParte', true, '12%', 'tipoParte'),
    new ColunaGenerica('Nome', 'nomeTratado', true, '33%', 'nome'),
    new ColunaGenerica('Estado', 'estado', true, '15%', 'estado'),
    new ColunaGenerica('CPF / CNPJ', 'documento', true, '17%', 'documento'),
    new ColunaGenerica('Carteira de Trabalho', 'carteiraTrabalho', true, '15%', 'carteiraTrabalho')
  ];

  private temPermissaoAlteraCartaFianca: boolean = true;
  // tslint:disable-next-line: max-line-length
  private ultimoFiltro: {
    nome: string,
    tipo: 'F' | 'J' | '',
    documento: string,
    carteiraTrabalho: string
  } = {
    nome: '',
    tipo: '',
    documento: '',
    carteiraTrabalho: ''
  };

  constructor(
    private parteService: PartesService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PARTE);
  }

  async obter(filtro?: { nome: string, tipo: 'F' | 'J' | '', documento: string, carteiraTrabalho: string }): Promise<void> {
    this.ultimoFiltro = filtro;

    if ((this.ultimoFiltro.nome == null || this.ultimoFiltro.nome === undefined || this.ultimoFiltro.nome === '') &&
      (this.ultimoFiltro.documento == null || this.ultimoFiltro.documento === undefined || this.ultimoFiltro.documento === '') &&
      (this.ultimoFiltro.tipo == null || this.ultimoFiltro.tipo === undefined || this.ultimoFiltro.tipo === '') &&
      (this.ultimoFiltro.carteiraTrabalho == null || this.ultimoFiltro.carteiraTrabalho === undefined || this.ultimoFiltro.carteiraTrabalho === '')) {
      this.dialog.showErr('Filtro não preenchido', 'Ao menos um dos 4 filtros deve estar preenchido: Nome, Tipo, CPF/CNPJ, Carteira de Trabalho');
      return;
    }

    try {
      const result = await this.parteService.obterPaginado(
        this.pagina, this.totalDeRegistrosPorPagina,
        this.ordenacaoColuna, this.ordenacaoDirecao,
        this.ultimoFiltro.nome, this.ultimoFiltro.documento,
        this.ultimoFiltro.carteiraTrabalho, TipoParte.Todos.find(x => x.valor === this.ultimoFiltro.tipo)
      );
      this.totalDeRegistros = result.total;
      this.registros = result.data;
    } catch (error) {
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    try {
      await ManterParteComponent.exibeModalDeCriar();
    } catch (error) {
      console.error(error);
    }
    if (!Object.keys(this.ultimoFiltro).some(x => this.ultimoFiltro[x] !== void 0))  {
      await this.obter(this.ultimoFiltro);
    }
  }

  async abrirDialogoDeAtualizar(parteId: number): Promise<void> {
    const entidade: Parte = this.registros.find(x => x.id === parteId);
    try {
      await ManterParteComponent.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.error(error);
    }
    if (!!this.ultimoFiltro) {
      await this.obter(this.ultimoFiltro);
    }
  }

  async remover(id: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Exclusão de Parte', 'Deseja excluir a Parte?');
    if (confirm) {
      try {
        await this.parteService.remover(id);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
      } catch (error) {
        this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      }
      this.obter(this.ultimoFiltro);
    }
  }

  get obterResultados(): Array<object> {
    const view = [];
    this.registros.forEach((p: Parte) => {
      view.push({
        id: p.id,
        tipoParte: p.tipoParte.descricao,
        nomeTratado: p.nomeTratado,
        nome: p.nome,
        estado: p.estado.id,
        documento: p.cpf ? p.CPF : p.CNPJ,
        carteiraTrabalho: p.carteiraDeTrabalho,
      });
    });
    return view;
  }

  trocarPagina() {
    this.obter(this.ultimoFiltro);
  }

  async exportar() {
    try {
      await this.parteService.exportar(
        this.ordenacaoColuna, this.ordenacaoDirecao,
        this.ultimoFiltro ? this.ultimoFiltro.nome : '',
        this.ultimoFiltro ? this.ultimoFiltro.documento.replace(/[^0-9]+/g, '') : '',
        this.ultimoFiltro ? this.ultimoFiltro.carteiraTrabalho : '',
        this.ultimoFiltro ? TipoParte.Todos.find(x => x.valor === this.ultimoFiltro.tipo) : TipoParte.NAO_DEFINIDO
      );
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
