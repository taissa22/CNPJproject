import { DialogService } from './../../../../../shared/services/dialog.service';
import { ColunaGenerica } from './../../../../models/coluna-generica.model';
import { Assunto } from '@manutencao/models';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AssuntoService } from '@manutencao/services';
import { ManterAssuntoCivelConsumidorComponent } from '../manter-assunto-civel-consumidor/manter-assunto-civel-consumidor.component';
import { ManterAssuntoCivelEstrategicoComponent } from '../manter-assunto-civel-estrategico/manter-assunto-civel-estrategico.component';
import { HttpErrorResult } from '@core/http/http-error-result';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({ templateUrl: './consulta.component.html' })
export class ConsultaAssuntosManutencaoComponent implements OnInit {

  titulo: string = 'Manutenção - Assuntos Cível Estratégico';
  breadcrumb: string = 'Manutenção > Cadastros Específicos > Cível Estratégico > Assunto';
  tituloLista: string = 'Assunto';
  tituloAdicionar: string = 'Adicionar Assunto';
  tipoProcesso: string;

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: string = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  registros: Array<Assunto> = [];
  totalDeRegistros: number = 0;

  modal: any;
  colunas: Array<ColunaGenerica> = [];

    
  textopesquisa : string ='';
  nomeCampoPesquisado : string = 'Descrição';


  constructor(
    private activatedRoute: ActivatedRoute,
    private service: AssuntoService,
    private dialog: DialogService,
    private breadcrumbService: BreadcrumbsService) { }

  ngOnInit() {
    this.activatedRoute.url.subscribe(value => {
      this.tipoProcesso = value[0].toString();
      this.initializeForm(this.tipoProcesso);
      this.obter();
    });
  }

  async obter(): Promise<void> {
    const ordenacao = this.ajustaOrdenacao(this.ordenacaoColuna);
    try {
      const { total, data } = await this.service.obterPaginado(this.textopesquisa,
        this.tipoProcesso,
        this.pagina,
        this.totalDeRegistrosPorPagina,
        ordenacao,
        this.ordenacaoDirecao
      );

      this.totalDeRegistros = total;
      this.registros = data;
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  // async trocarPagina(pagina: number): Promise<void> {
  //   this.pagina = pagina;
  //   const ordenacao = this.verificaOrdenacao(this.ordenacaoColuna);

  //   const { total, data } = await this.service.obterPaginado(
  //     this.tipoProcesso,
  //     this.pagina,
  //     this.totalDeRegistrosPorPagina,
  //     ordenacao,
  //     this.ordenacaoDirecao
  //   );

  //   if (data.length === 0) {
  //     this.pagina = 1;
  //     await this.obter();
  //   } else {
  //     this.totalDeRegistros = total;
  //     this.registros = data;
  //   }
  // }

  async exportar(): Promise<void> {
    try {
      await this.service.exportar(this.textopesquisa, this.tipoProcesso, this.ordenacaoColuna, this.ordenacaoDirecao);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar(): Promise<void> {
    try {
      await this.modal.exibeModalDeCriar();
    } catch (error) {
      console.error(error);
    }
    await this.obter();
  }

  async abrirDialogoDeAtualizar(assuntoId: number): Promise<void> {
    const entidade: Assunto = this.registros.find(x => x.id === assuntoId);
    try {
      await this.modal.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.error(error);
    }
    await this.obter();
  }


  async remover(id: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Excluir Assunto', 'Deseja excluir o assunto?');
    if (confirm) {
      try {
        await this.service.remover(this.tipoProcesso, id);
        this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
      } catch (error) {
        this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
      }
      this.obter();
    }
  }

  async initializeForm(tipoProcesso: string) {
    switch (tipoProcesso) {
      case 'civel-consumidor':
        this.titulo = 'Manutenção - Assuntos Cível Consumidor';
        this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_ASSUNTO_CIVEL_CONSUMIDOR);
        this.modal = ManterAssuntoCivelConsumidorComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '5%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '25%', 'descricao'),
          new ColunaGenerica('Proposta', 'propostaTratada', true, '5%', 'proposta'),
          new ColunaGenerica('Negociação', 'negociacaoTratada', true, '5%', 'negociacao'),
          new ColunaGenerica('Cálculo da Contingência', 'calculoContingenciaTratada', true, '15%', 'calculoContingencia'),          
          new ColunaGenerica('Correspondente Cível Estratégico(DExPARA Migração de Processo)', 'descricaoMigracao', true, '45%', 'descricaoMigracao')
          
        ];

        break;

      default:
        this.titulo = 'Manutenção - Assuntos Cível Estratégico';
        this.breadcrumb = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_ASSUNTO_CIVEL_ESTRATEGICO);
        this.modal = ManterAssuntoCivelEstrategicoComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '7%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '20%', 'descricao'),
          new ColunaGenerica('Ativo', 'ativoEmTexto', true, '20%', 'ativoEmTexto'),
          // new ColunaGenerica('Cálculo da Contingência', 'calculoContingenciaTratada', true, '35%', 'calculoContingencia')
          new ColunaGenerica('Correspondente Cível Consumidor(DExPARA Migração de Processo)', 'descricaoMigracao', true, '40%', 'descricaoMigracao')
        ];

        break;
    }
  }

  ajustaOrdenacao(ordenacao: string) {
    switch (ordenacao) {
      case 'ativoEmTexto':
        return 'ativo';

      case 'propostaTratada':
        return 'proposta';

      case 'negociacaoTratada':
        return  'negociacao';

      case 'descricaoTratada':
        return  'descricao';

      case 'calculoContingenciaTratada':
        return 'codTipoContingencia';


      default:
        return ordenacao;
    }
  }
}
