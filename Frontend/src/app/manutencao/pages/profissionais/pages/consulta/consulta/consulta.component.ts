import { isNullOrUndefined } from 'util';
import { DialogService } from '@shared/services/dialog.service';
import { ColunaGenerica } from './../../../../../models/coluna-generica.model';

import { Profissional } from '@manutencao/models';
import { Component } from '@angular/core';
import { ProfissionaisService } from '@manutencao/services';

import { ManterProfissionalComponent } from './../../manter/manter/manter.component';
import { HttpErrorResult } from '@core/http/http-error-result';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  selector: 'app-consulta',
  templateUrl: './consulta.component.html'
})
export class ConsultaProfissionaisComponent {

  // tslint:disable-next-line: no-inferrable-types
  pagina: number = 1;
  // tslint:disable-next-line: no-inferrable-types
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: 'nome' | 'documento' | 'estado' | 'advogadoDoAutor' | 'registroOAB' | 'estadoOAB' = 'nome';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  registros: Array<Profissional> = [];
  // tslint:disable-next-line: no-inferrable-types
  totalDeRegistros: number = 0;
  breadcrumb: string;

  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Nome', 'nomeTratado', true, '29%', 'nome'),
    new ColunaGenerica('CNPJ/CPF', 'documento', true, '14%', 'documento'),
    new ColunaGenerica('Estado', 'estado', true, '10%', 'estado'),
    new ColunaGenerica('Advogado do Autor', 'ehAdvogadoAutor', true, '16%', 'ehAdvogadoAutor'),
    new ColunaGenerica('Registro OAB Advogado', 'numeroOAB', true, '12%', 'numeroOAB'),
    new ColunaGenerica('Estado OAB', 'estadoOAB', true, '10%', 'estadoOAB'),
  ];

  constructor(
    private profissionalService: ProfissionaisService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_PROFISSIONAL);
  }

  private ultimoFiltro: { nome: string, documento: string, tipoPessoa: 'F' | 'J' | '', advogadoDoAutor: true | false | '' };

  async obter(filtro?: { nome: string, documento: string, tipoPessoa: 'F' | 'J' | '', advogadoDoAutor: true | false | '' }): Promise<void> {
    this.ultimoFiltro = isNullOrUndefined(filtro) ? this.ultimoFiltro : filtro;

    if ((isNullOrUndefined(this.ultimoFiltro.nome) || this.ultimoFiltro.nome === '') &&
      (isNullOrUndefined(this.ultimoFiltro.documento) || this.ultimoFiltro.documento === '') &&
      (isNullOrUndefined(this.ultimoFiltro.tipoPessoa) || this.ultimoFiltro.tipoPessoa === '') &&
      (isNullOrUndefined(this.ultimoFiltro.advogadoDoAutor) || this.ultimoFiltro.advogadoDoAutor === '')) {
      this.dialog.showErr('Filtro não preenchido', 'Ao menos um dos três filtros devem estar preenchidos: Nome, Tipo, CPF/CNPJ.');
      return;
    }

    try {
      const result = await this.profissionalService.obterPaginado(
        this.pagina, this.totalDeRegistrosPorPagina,
        this.ordenacaoColuna, this.ordenacaoDirecao,
        filtro.nome, filtro.documento.replace(/[^0-9]+/g, ''), filtro.tipoPessoa, filtro.advogadoDoAutor);
      this.totalDeRegistros = result.total;
      this.registros = result.data;
    } catch (error) {
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    try {
      await ManterProfissionalComponent.exibeModalDeCriar();
    } catch (error) {
      console.error(error);
    }
    // TODO: Verificar como proceder com esse if
    // if (!Object.keys(this.ultimoFiltro).some(x => this.ultimoFiltro[x] !== void 0)) {
    if (this.temFiltro()) {
      await this.obter(this.ultimoFiltro);
    }
  }

  async abrirDialogoDeAtualizar(profissionalId: number): Promise<void> {
    const entidade: Profissional = this.registros.find(x => x.id === profissionalId);
    try {
      await ManterProfissionalComponent.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.error(error);
    }
    if (this.temFiltro()) {
      await this.obter(this.ultimoFiltro);
    }
  }

  async remover(id: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Excluir Profissional', 'Deseja excluir o Profissional?');
    if (confirm) {
      try {
        await this.profissionalService.remover(id);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
      } catch (error) {
        this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      }
      this.obter(this.ultimoFiltro);
    }
  }

  get obterResultados(): Array<object> {
    const view = [];
    this.registros.forEach(p => {
      view.push({
        id: p.id,
        nome: p.nome,
        nomeTratado: p.nomeTratado,
        documento: p.documento,
        estado: p.estado ? p.estado.id : null,
        ehAdvogadoAutor: p.ehAdvogado ? 'Sim' : 'Não',
        numeroOAB: p.registroOAB,
        estadoOAB: p.estadoOAB ? p.estadoOAB.id : null
      });
    });
    return view;
  }

  trocarPagina() {
    this.obter(this.ultimoFiltro);
  }

  async exportar(): Promise<void> {
    try {
      await this.profissionalService.exportar(
        this.ordenacaoColuna, this.ordenacaoDirecao,
        this.ultimoFiltro ? this.ultimoFiltro.nome : '', this.ultimoFiltro ? this.ultimoFiltro.documento.replace(/[^0-9]+/g, '') : '',
        this.ultimoFiltro ? this.ultimoFiltro.tipoPessoa : '', this.ultimoFiltro ? this.ultimoFiltro.advogadoDoAutor : ''
      );
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  temFiltro() {
    return this.ultimoFiltro !== undefined && (this.ultimoFiltro.nome !== '' || this.ultimoFiltro.documento !== ''
      || this.ultimoFiltro.tipoPessoa !== '' || this.ultimoFiltro.advogadoDoAutor !== '');
  }
}
