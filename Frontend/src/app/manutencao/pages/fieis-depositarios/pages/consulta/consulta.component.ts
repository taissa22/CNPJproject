import { ManterFielDepositarioComponent } from './../manter/manter.component';
import { Component, OnInit } from '@angular/core';

import { FielDepositario } from '@manutencao/models/fiel-depositario.model';
import { FieisDepositariosService } from '@manutencao/services/fieis-depositarios.service';

import { ColunaGenerica } from './../../../../models/coluna-generica.model';
import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
@Component({
  templateUrl: './consulta.component.html'
})

export class ConsultaFielDepositarioComponent implements OnInit {

  titulo: string = 'Fiel Depositário';
  breadcrumb: string;
  tituloLista: string = 'Fiel Depositário';
  tituloAdicionar: string = 'Adicionar Fiel Depositário';

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: string = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  registros: Array<FielDepositario> = [];
  totalDeRegistros: number = 0;

  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Código', 'id', true, '10%', 'id'),
    new ColunaGenerica('CPF', 'CPF', true, '30%', 'CPF'),
    new ColunaGenerica('Nome', 'nomeTratado', true, '55%', 'nome')
  ];

  constructor(
    private service: FieisDepositariosService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit() {
    this.obter();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_FIEL_DEPOSITARIO);
  }

  async obter(): Promise<void> {
    const ordenacao = this.ordenacaoColuna === 'ativoEmTexto' ? 'ativo' : this.ordenacaoColuna;
    try {
      const resultado = await this.service
        .obterPaginado(this.pagina, this.totalDeRegistrosPorPagina, ordenacao, this.ordenacaoDirecao);
      this.totalDeRegistros = resultado.total;
      this.registros = resultado.data;
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    try {
      const resultado = await ManterFielDepositarioComponent.exibeModalDeCriar();
      console.log(resultado);
    } catch (error) {
      console.error(error);
    }
    await this.obter();
  }

  async abrirDialogoDeAtualizar(id: number): Promise<void> {
    const entidade: FielDepositario = this.registros.find(x => x.id === id);
    try {
      await ManterFielDepositarioComponent.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.error(error);
    }
    await this.obter();
  }

  async remover(id: number): Promise<void> {
    const confirm = this.dialog.showConfirm('Excluir Fiel Depositário', 'Deseja excluir o Fiel Depositário?');
    if (confirm) {
      try {
        await this.service.remover(id);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
      } catch (error) {
        this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      }
      await this.obter();
    }
  }

  async exportar() {
    try {
      const ordenacao = this.ordenacaoColuna === 'ativoEmTexto' ? 'ativo' : this.ordenacaoColuna;
      await this.service.exportar(ordenacao, this.ordenacaoDirecao);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
