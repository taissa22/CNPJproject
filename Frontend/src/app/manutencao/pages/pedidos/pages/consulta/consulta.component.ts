import { DialogService } from '@shared/services/dialog.service';
import { ColunaGenerica } from './../../../../models/coluna-generica.model';
import { Pedido } from '@manutencao/models';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PedidosService } from '@manutencao/services';
import { ManterPedidoEstrategicoManutencaoComponent } from '../manter-civel-estrategico/manter-civel-estrategico.component';
import { ManterTrabalhistaComponent } from '../manter-trabalhista/manter-trabalhista.component';
import { HttpErrorResult } from '@core/http/http-error-result';
import { ManterPedidoConsumidorManutencaoComponent } from '../manter-civel-consumidor/manter-civel-consumidor.component';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({ templateUrl: './consulta.component.html' })
export class ConsultaPedidosManutencaoComponent implements OnInit {

  titulo: string = 'Manutenção - Pedido Cível Estratégico';
  breadcrump: string = 'Manutenção > Cível Estratégico > Estratégico > Pedido';
  tituloLista: string = 'Pedido';
  tituloAdicionar: string = 'Adicionar Pedido';
  tipoProcesso: string;

  pagina: number = 1;
  totalDeRegistrosPorPagina: number = 8;

  ordenacaoColuna: string = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';

  registros: Array<Pedido> = [];
  totalDeRegistros: number = 0;

  modal: any;
  colunas: Array<ColunaGenerica> = [];

  textopesquisa : string ='';
  nomeCampoPesquisado : string = 'Descrição';

  constructor(
    private activatedRoute: ActivatedRoute,
    private service: PedidosService,
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
    let ordenacao = this.ordenacaoColuna;
    switch (this.ordenacaoColuna) {
      case 'ativoEmTexto':
        ordenacao = 'ativo';
        break;
		
	  case 'audienciaEmTexto':
		ordenacao = 'ativo';
		break;

      case 'riscoPerdaEmTexto':
        ordenacao = 'riscoPerda';
        break;

      case 'provavelZeroEmTexto':
        ordenacao = 'provavelZero';
        break;

      case 'proprioTerceiroEmTexto':
        ordenacao = 'proprioTerceiro';
        break;
    }

    try {
      const result = await this.service.obterPaginado(this.textopesquisa,this.tipoProcesso, this.pagina,
          this.totalDeRegistrosPorPagina, ordenacao, this.ordenacaoDirecao);
      this.totalDeRegistros = result.total;
      this.registros = result.data;
      console.log(result.data);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
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

  async abrirDialogoDeAtualizar(pedidoId: number): Promise<void> {
    const entidade: Pedido = this.registros.find(x => x.id === pedidoId);
    try {
      await this.modal.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.error(error);
    }
    await this.obter();
  }

  async remover(id: number): Promise<void> {
    const confirm = await this.dialog.showConfirm('Excluir Pedido', 'Deseja excluir o pedido?');
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
      case 'trabalhista':
        this.titulo = 'Manutenção - Pedido Trabalhista';
        this.breadcrump = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_PEDIDO_TRABALHISTA);
        this.modal = ManterTrabalhistaComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '6%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '25%', 'descricao'),
          new ColunaGenerica('Risco de Perda Potencial Inicial (Trabalhista)', 'riscoPerdaEmTexto', true, '25%', 'riscoPerdaEmTexto'),
          new ColunaGenerica('Provável Zero', 'provavelZeroEmTexto', true, '14%', 'provavelZeroEmTexto'),
          new ColunaGenerica('Próprio/Terceiro', 'proprioTerceiroEmTexto', true, '14%', 'proprioTerceiroEmTexto'),
          new ColunaGenerica('Ativo', 'ativoEmTexto', true, '6%', 'ativoEmTexto')
        ];

        break;

        case 'civel-consumidor':
          this.titulo = 'Manutenção - Pedido Cível Consumidor';
          this.breadcrump = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_PEDIDO_CIVEL_CONSUMIDOR);
          this.modal = ManterPedidoConsumidorManutencaoComponent;

          this.colunas = [
            new ColunaGenerica('Código', 'id', true, '5%', 'id'),
            new ColunaGenerica('Descrição', 'descricaoTratada', true, '25%', 'descricao'),
            new ColunaGenerica('Necessita atualização de débito próximo à audiência', 'audienciaEmTexto', true, '20%', 'audienciaEmTexto'),
            new ColunaGenerica('Ativo', 'ativoEmTexto', true, '10%', 'ativoEmTexto'),
            new ColunaGenerica('Correspondente Cível Estratégico (DE x PARA Migração de Processo)', 'descricaoPara', true, '40%', 'descricaoPara')
          ];

          break;

      default:
        this.titulo = 'Manutenção - Pedido Cível Estratégico';
        this.breadcrump = await this.breadcrumbService.nomeBreadcrumb(Permissoes.ACESSAR_PEDIDO_CIVEL_ESTRATEGICO);
        this.modal = ManterPedidoEstrategicoManutencaoComponent;

        this.colunas = [
          new ColunaGenerica('Código', 'id', true, '7%', 'id'),
          new ColunaGenerica('Descrição', 'descricaoTratada', true, '43%', 'descricao'),
          new ColunaGenerica('Ativo', 'ativoEmTexto', true, '10%', 'ativoEmTexto'),
          new ColunaGenerica('Correspondente Cível Consumidor (DE x PARA Migração de Processo)', 'descricaoPara', true, '40%', 'descricaoPara')
        ];

        break;
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.service.exportar(this.textopesquisa, this.tipoProcesso, this.ordenacaoColuna, this.ordenacaoDirecao);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }
}
