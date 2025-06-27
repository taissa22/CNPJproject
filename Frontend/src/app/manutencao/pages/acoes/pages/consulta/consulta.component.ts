import { DialogService } from './../../../../../shared/services/dialog.service';
import { ManterTributarioJudicialComponent } from './../manter-tributario-judicial/manter-tributario-judicial.component';
import { AfterViewInit, Component, OnInit } from '@angular/core';

import { Acao } from '@manutencao/models/acao.model';
import { AcoesService } from '@manutencao/services/acoes.service';

import { ColunaGenerica } from './../../../../models/coluna-generica.model';
import { ActivatedRoute } from '@angular/router';
import { ManterAcaoManutencaoComponent } from '../manter-civel-estrategico/manter-civel-estrategico.component';
import { ManterCivelConsumidorComponent } from '../manter-civel-consumidor/manter-civel-consumidor.component';
import { ManterAcaoTrabalhistaComponent } from '../manter-trabalhista/manter-trabalhista.component';
import { HttpErrorResult } from '@core/http/http-error-result';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  templateUrl: './consulta.component.html'
})

export class ConsultaAcoesManutencaoComponent implements OnInit {
  titulo: string = 'Manutenção - Ações Cível Estratégico';
  breadcrumb: string = 'Manutenção > Cadastros Específicos > Cível Estratégico > Ação';
  tituloLista: string = 'Ação';
  tituloAdicionar: string = 'Adicionar Ação';
  tipoProcesso: string;
  pagina: number = 1;
  ordenacaoColuna: string = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';
  totalDeRegistrosPorPagina: number = 8;
  registros: Array<Acao>;
  totalDeRegistros: number;
  modal: any;
  colunas: Array<ColunaGenerica> = [];


  textopesquisa : string ='';
  nomeCampoPesquisado : string = 'Descrição';

  constructor(
    private activatedRoute: ActivatedRoute,
    private service: AcoesService,
    private dialog: DialogService,
    private breadcrumbService: BreadcrumbsService) { }



    ngOnInit(): void {
    this.activatedRoute.url.subscribe(value => {
        this.tipoProcesso = value[0].toString();
        this.initializeForm(this.tipoProcesso);
        this.obter();
      });
  }

  async abrirDialogoDeCriar(): Promise<void> {
    try {
      await this.modal.exibeModalDeCriar();
    } catch (error) {
      console.log(error);
    }
    await this.obter();
  }

  async abrirDialogoDeAtualizar(id: number): Promise<void> {
    try {
      const entidade: Acao = this.registros.find(x => x.id === id);
      await this.modal.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.log(error);
    }
    await this.obter();
  }

  async remover(id: number): Promise<void> {
    try {
      const result = await this.dialog.showConfirm('Excluir Ação', 'Deseja excluir a ação?');

      if (result) {
        try {
          await this.service.remover(this.tipoProcesso, id);
          this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
        } catch (error) {
          this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
        }
        await this.obter();
      }
    } catch {
      this.dialog.showErr('Ocorreu um erro interno', 'Tente novamente mais tarde.');
    }
  }

  async obter(): Promise<void> {
    const ordenacao = this.ordenacaoColuna === 'ativoEmTexto' ? 'ativo' : this.ordenacaoColuna;

    try {
      const resultado = await this.service
        .obterPaginado(this.textopesquisa, this.tipoProcesso, this.pagina, this.totalDeRegistrosPorPagina, ordenacao, this.ordenacaoDirecao);

      this.totalDeRegistros = resultado.total;
      this.registros = resultado.data;

    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.service.exportar(this.textopesquisa,this.tipoProcesso, this.ordenacaoColuna, this.ordenacaoDirecao);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async initializeForm(tipoProcesso: string) {
    switch (tipoProcesso) {
      case 'civel-consumidor':
        this.titulo = 'Manutenção - Ações Cível Consumidor';
        this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_ACAO_CIVEL_CONSUMIDOR);
        this.modal = ManterCivelConsumidorComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '5%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '20%', 'descricao'),
          new ColunaGenerica('Natureza da Ação BB', 'nomeNatureza', true, '26%', 'nomeNatureza'),
          new ColunaGenerica('Correspondente Cível Estratégico(DExPARA Migração de Processo)', 'descricaoMigracao', true, '30%', 'descricaoMigracao'),
          new ColunaGenerica('Enviar para o App Preposto', 'enviarAppPreposto', true, '15%', 'enviarAppPreposto')
        ];

        break;

      case 'trabalhista':
        this.titulo = 'Manutenção - Ações Trabalhista';
        this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_ACAO_TRABALHISTA);
        this.modal = ManterAcaoTrabalhistaComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '7%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '83%', 'descricao')
        ];

        break;

      case 'tributario-judicial':
        this.titulo = 'Manutenção - Ações Tributária Judicial';
        this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_ACAO_TRIBUTARIA_JUDICIAL);
        this.modal = ManterTributarioJudicialComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '7%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '83%', 'descricao')
        ];

        break;

      default:
        this.titulo = 'Manutenção - Ações Cível Estratégico';
        this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_ACAO_CIVEL_ESTRATEGICO);
        this.modal = ManterAcaoManutencaoComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '7%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '43%', 'descricao'),
          new ColunaGenerica('Ativo', 'ativoEmTexto', true, '10%', 'ativoEmTexto'),
          new ColunaGenerica('Correspondente Cível Consumidor(DExPARA Migração de Processo)', 'descricaoMigracao', true, '40%', 'descricaoMigracao')
        ];

        break;
    }

  }
}
