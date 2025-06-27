import { Component } from '@angular/core';

import { DialogService } from '@shared/services/dialog.service';

import { HttpErrorResult } from '../../../core/http/http-error-result';
import { EmpresaDoGrupoService } from '@manutencao/services/empresa-do-grupo.service';
import { ManterComponent } from './components/manter/manter.component';
import { ColunaGenerica } from '../../models/coluna-generica.model';
import { EmpresaDoGrupo } from '../../models/empresa-do-grupo.model';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  templateUrl: './empresas-do-grupo.component.html',
  styleUrls: ['./empresas-do-grupo.component.scss']
})
export class EmpresaDoGrupoComponent {

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;
  breadcrumb: string

  ordenacaoColuna: 'nome' | 'cnpj' | 'empresacentralizadora' | 'estado' | 'centrosap' = 'nome';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  modal: any = ManterComponent;
  registros: Array<EmpresaDoGrupo> = [];
  totalDeRegistros: number = 0;

  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('CNPJ', 'cnpj', true, '16%', 'cnpj'),
    new ColunaGenerica('Razão Social', 'nome', true, '28%', 'nomeDataTitle'),
    new ColunaGenerica('Empresa Centralizadora', 'empresaCentralizadora', true, '25%', 'empresaCentralizadora'),
    new ColunaGenerica('Estado', 'estado', true, '13%', 'estado'),
    new ColunaGenerica('Centro SAP', 'codCentroSap', true, '10%', 'codCentroSap'),
    new ColunaGenerica('Emp Recuperanda', 'empRecuperanda', true, '20%', 'empRecuperanda'),
  ];

  constructor(
    private empresaDoGrupoService: EmpresaDoGrupoService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  private ultimoFiltro: { cnpj: string, codCentroSap: string, nome: string };

  async ngOnInit() {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_EMPRESA_DO_GRUPO);
  }

  async obter(filtro?: { cnpj: string, codCentroSap: string, nome: string }): Promise<void> {
    this.ultimoFiltro = filtro;
    try {
      const result = await this.empresaDoGrupoService
        .obterPaginado(
          this.pagina,
          this.totalDeRegistrosPorPagina,
          this.ordenacaoColuna,
          this.ordenacaoDirecao,
          filtro.nome,
          filtro.cnpj.replace(/[^0-9]+/g, ''),
          filtro.codCentroSap);

      this.totalDeRegistros = result.total;
      this.registros = result.data;

    } catch (error) {
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.empresaDoGrupoService.exportar(this.ordenacaoColuna, this.ordenacaoDirecao,
        this.ultimoFiltro ? this.ultimoFiltro.nome : '',
        this.ultimoFiltro ? this.ultimoFiltro.cnpj.replace(/[^0-9]+/g, '') : '',
        this.ultimoFiltro ? this.ultimoFiltro.codCentroSap : '');
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    try {
      this.modal.exibeModalDeCriar().then( async (result: any) => {
        if (result !== 'cancel' && !!this.ultimoFiltro) {
          await this.obter(this.ultimoFiltro);
        }
      });
    } catch (error) {
      console.log(error);
    }
  }

  async abrirDialogoDeAtualizar(empresa: EmpresaDoGrupo): Promise<void> {
    try {
      const entidade = this.registros.filter(r => {
        return r.cnpjFormatado === empresa.cnpj;
      })[0];

      this.modal.exibeModalDeAlterar(entidade).then( async (result: any) => {
        if (result !== 'cancel' && !!this.ultimoFiltro) {
          await this.obter(this.ultimoFiltro);
        }
      });
    } catch (error) {
      console.log(error);
    }
  }

  async remover(id: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Exclusão de Empresa do Grupo', 'Deseja excluir a Empresa do Grupo?');
    if (confirm) {
      try {
        await this.empresaDoGrupoService.remover(id);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluido do sistema.');
      } catch (error) {
        const messages = (error as HttpErrorResult).messages.join('\n') + '.';
        const title = messages.startsWith('Não será possível') ? 'Exclusão não permitida' : 'Exclusão não realizada';

        this.dialog.showErr(title, messages);
      }
      this.obter(this.ultimoFiltro);
    }
  }

  get obterResultados(): Array<object> {
    const view = [];
    this.registros.forEach(x => {
      view.push({
        id: x.id,
        nomeDataTitle: x.nome,
        nome: x.nomeTratado,
        cnpj: x.cnpjFormatado,
        empresaCentralizadora: x.empresaCentralizadora ? x.empresaCentralizadora.nome : '',
        estado: x.estado ? x.estado.id : '',
        codCentroSap: x.codCentroSap,
        empRecuperanda: x.empRecuperanda ? x.empRecuperanda = 'Sim' : 'Não'
      });
    });
    return view;
  }

  trocarPagina() {
    this.obter(this.ultimoFiltro);
  }
}
