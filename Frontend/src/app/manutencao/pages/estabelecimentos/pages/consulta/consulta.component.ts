import { ManterEstabelecimentosComponent } from '../manter/manter.component';
import { Component, OnInit } from '@angular/core';
import { Estabelecimento } from '@manutencao/models/estabelecimento.model';
import { ColunaGenerica } from './../../../../models/coluna-generica.model';
import { EstabelecimentosService } from '../../../../services/estabelecimentos.service';
import { HttpErrorResult } from '@core/http/http-error-result';
import { DialogService } from '@shared/services/dialog.service';
import { Permissoes } from '@permissoes';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';

@Component({
  templateUrl: './consulta.component.html'
})
export class ConsultaEstabelecimentosManutencaoComponent implements OnInit {

  titulo: string = 'Estabelecimento';
  breadcrumb: string;
  tituloLista: string = 'Estabelecimento';
  tituloAdicionar: string = 'Adicionar Estabelecimento';
  
  pagina: number = 1;
  ordenacaoColuna: string = '';
  ordenacaoDirecao: 'asc' | 'desc' = 'asc';
  totalDeRegistrosPorPagina: number = 8;
  registros: Array<Estabelecimento>;
  totalDeRegistros: number;
  modal: any = ManterEstabelecimentosComponent;  
  colunas: Array<ColunaGenerica> = [
    new ColunaGenerica('Nome', 'nomeTratado', true, '58%', 'nome'),
    new ColunaGenerica('Estado', 'estado', true, '7%', 'estado'),
    new ColunaGenerica('CNPJ', 'cnpj', true, '27%', 'cnpj')
  ];

  
  textopesquisa : string ='';
  nomeCampoPesquisado : string = 'Nome';


  constructor(
    private service: EstabelecimentosService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) { }

  async ngOnInit() {
    this.obter();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_ESTABELECIMENTO);
  }

  async obter() {
    let coluna = this.ordenacaoColuna === 'idEstado' ? 'estado' : this.ordenacaoColuna;
    if (this.ordenacaoColuna === 'nomeTratado') { coluna = 'nome'; }
    
    try {
      const resultado = await this.service
        .obterPaginado(this.pagina, this.totalDeRegistrosPorPagina, coluna, this.ordenacaoDirecao,this.textopesquisa);

      this.totalDeRegistros = resultado.total;
      this.registros = resultado.data;

    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async exportar(): Promise<void> {
    try {
      await this.service.exportar(this.textopesquisa, this.ordenacaoColuna, this.ordenacaoDirecao);
    } catch (error) {
      console.error(error);
      this.dialog.showErr('Não foi possível exportar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  async abrirDialogoDeCriar() {
    try {
      await this.modal.exibeModalDeCriar();
    } catch (error) {
      console.log(error);
    }
    await this.obter();
  }

  async abrirDialogoDeAtualizar(id: number) {
    try {
      const entidade = await this.registros.filter((r) => r.id === id)[0];
      await this.modal.exibeModalDeAlterar(entidade);
    } catch (error) {
      console.log(error);
    }
    await this.obter();
  }

  async remover(id: number): Promise<void> {
    try {
      const result = this.dialog.showConfirm('Excluir Estabelecimento', 'Deseja excluir o estabelecimento?');

      if (result) {
        try {
          await this.service.remover(id);
          this.dialog.showAlert('Exclusão realizada com sucesso', 'O registro foi excluído do sistema.');
        } catch (error) {
          this.dialog.showErr('Exclusão não realizada', (error as HttpErrorResult).messages.join('\n'));
        }
        this.obter();
      }
    } catch {
      this.dialog.showErr('Ocorreu um erro interno', 'Tente novamente mais tarde.');
    }
  }

  get obterResultados(): Array<object> {
    const view = [];
    if (this.registros) {
      this.registros.forEach((p: Estabelecimento) => {
        view.push({
          id: p.id,
          nomeTratado: p.nomeTratado,
          nome: p.nome,
          estado: p.estado.id,
          cnpj: p.CNPJ,
        });
      });
      return view;
    }
    return null;
  }
}
