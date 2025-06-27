import { Component, OnInit } from '@angular/core';

import { DialogService } from '@shared/services/dialog.service';

import { HttpErrorResult } from '@core/http';

import { Permissoes, PermissoesService } from '@permissoes';

import { OrgaosModalComponent } from './orgaos-modal/orgaos-modal.component';
import { OrgaosService } from '@manutencao/services/orgaos.service';

import { Orgao, TipoOrgao } from '@manutencao/models';
import { ColunaGenerica } from '@manutencao/models/coluna-generica.model';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  templateUrl: './orgaos.component.html',
  styleUrls: ['./orgaos.component.scss']
})
export class OrgaosComponent implements OnInit {

  private tipoOrgao: TipoOrgao = TipoOrgao.NAO_DEFINIDO;
 // tslint:disable-next-line: no-inferrable-types
  pagina: number = 1;
  // tslint:disable-next-line: no-inferrable-types
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: 'nome' | 'telefone' | 'competencia' | '' = 'nome';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';
  tiposOrgao: Array<TipoOrgao> = [];

  registros: Array<Orgao> = [];
   // tslint:disable-next-line: no-inferrable-types
  totalDeRegistros: number = 0;

  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Nome', 'nomeTratado', true, '27%', 'nome'),
    new ColunaGenerica('DDD / Telefone', 'telefone', true, '15%', 'telefone'),
    new ColunaGenerica('Competências', 'competencias', true, '50%', 'competencias', {}, true)
  ];
  breadcrumb: string;

  constructor(
    private orgaosService: OrgaosService, 
    private dialog: DialogService, 
    private permisoesService: PermissoesService,
    private breadcrumbsService: BreadcrumbsService
    ) { }

  ngOnInit(): void {
    if (this.permisoesService.temPermissaoPara(Permissoes.ALTERAR_ORGAO_CIVEL_ADMINISTRATIVO)) {
      this.tiposOrgao.push(TipoOrgao.CIVEL_ADMINISTRATIVO);
    }

    if (this.permisoesService.temPermissaoPara(Permissoes.ALTERAR_ORGAO_CRIMINAL_ADMINISTRATIVO)) {
      this.tiposOrgao.push(TipoOrgao.CRIMINAL_ADMINISTRATIVO);
    }

    if (this.permisoesService.temPermissaoPara(Permissoes.ALTERAR_ORGAO_DEMAIS_TIPOS)) {
      this.tiposOrgao.push(TipoOrgao.DEMAIS_TIPOS);
    }
  }

  async ngAfterViewInit(): Promise<void> {
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ORGAO);
  }

  alteraTipoOrgao(tipoOrgao: TipoOrgao): void {
    this.tipoOrgao = tipoOrgao;
    this.registros = [];
    this.totalDeRegistros = 0;
    if (this.tipoOrgao === TipoOrgao.CIVEL_ADMINISTRATIVO) {
      this.colunas[2].titulo = 'Procuradorias';
    } else {
      this.colunas[2].titulo = 'Competências';
    }
  }

  async obter(): Promise<void> {
    if (this.tipoOrgao === TipoOrgao.NAO_DEFINIDO) {
      return;
    }

    try {
      const result = await this.orgaosService
        .obterPaginado(
          this.pagina, this.totalDeRegistrosPorPagina,
          this.tipoOrgao, this.ordenacaoColuna, this.ordenacaoDirecao);
      this.totalDeRegistros = result.total;
      this.registros = result.data;
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    if (this.tipoOrgao === TipoOrgao.NAO_DEFINIDO) {
      this.dialog.showErr('Tipo de processo não selecionado', 'Para adicionar um novo órgão é necessário selecionar o tipo');
      return;
    }
    try {
      OrgaosModalComponent.exibeModalDeCriar(this.tipoOrgao).then( async (result: any) => {
        if (result !== 'cancel') {
          await this.obter();
        }
      });
    } catch (error) {
      console.error(error);
    }
  }

  async abrirDialogoDeAtualizar(orgaoId: number): Promise<void> {
    const orgao: Orgao = this.registros.find(x => x.id === orgaoId);
    try {
      OrgaosModalComponent.exibeModalDeAlterar(orgao).then( async (result: any) => {
        if (result !== 'cancel') {
          await this.obter();
        }
      });
    } catch (error) {
      console.error(error);
    }
  }

  async remover(id: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Excluir Órgão', 'Deseja excluir o Órgão?');
    if (confirm) {
      try {
        await this.orgaosService.remover(id);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
      } catch (error) {
        this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      }
      this.obter();
    }
  }

  get obterResultados(): Array<object> {
    const view = [];
    this.registros.forEach((p: Orgao) => {
      view.push({
        id: p.id,
        nome: p.nome,
        nomeTratado: p.nome.length > 30 ? `${p.nome.substring(0, 30)}...` : p.nome,
        telefone: p.telefoneCompleto,
        competencias: p.nomesCompetencias
      });
    });
    return view;
  }

  trocarPagina() {
    this.obter();
  }

  async exportar(): Promise<void> {
    try {
      await this.orgaosService.exportar(this.tipoOrgao, this.ordenacaoColuna, this.ordenacaoDirecao);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
