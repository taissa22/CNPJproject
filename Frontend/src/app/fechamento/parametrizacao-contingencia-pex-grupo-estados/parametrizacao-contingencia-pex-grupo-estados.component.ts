import { Sort } from '@shared/types/sort';
import { listaTipoProcesso } from './../../sap/manutenção/manutencao-categoria-pagamento/categoria-pagamento.constant';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { HttpErrorResult } from '../../core/http';
import { DialogService } from '../../shared/services/dialog.service';
import { GrupoDeEstadosModel } from '../models/parametrizacao-contingencia-pex-grupo-estados.model';
import { EstadosModel } from '../models/estados.model';
import { GrupoDeEstadosService } from '../services/parametrizacao-contingencia-pex-grupo-estados.service';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-parametrizacao-contingencia-pex-grupo-estados',
  templateUrl: './parametrizacao-contingencia-pex-grupo-estados.component.html',
  styleUrls: ['./parametrizacao-contingencia-pex-grupo-estados.component.scss']
})
export class GrupoDeEstadosComponent implements OnInit {

  empresasXGrupoPesquisa: Array<any>;
  estadosDisponiveis: Array<EstadosModel>;
  grupos: Array<GrupoDeEstadosModel>;
  estadosDisponiveisInicial: Array<EstadosModel>;
  gruposInicial: Array<GrupoDeEstadosModel>;
  listaEstados: Array<EstadosModel>;
  registrosAlterados: Array<GrupoDeEstadosModel>;
  removendoEstados: boolean = false;
  adicionandoEstados: boolean = false;
  novoGrupo: boolean = false;
  editandoGrupo: boolean = false;
  grupoSelecionado: GrupoDeEstadosModel = null;
  selectedIndex: number = null;
  nomeEstadoFiltro = '';
  nomeEstadoFiltroBusca = '';
  controle = false;

  @ViewChild("inputNovoGrupo", {static: false}) inputNovoGrupo: ElementRef;
  breadcrumb: string;

  constructor(
    private grupoDeEstadosService: GrupoDeEstadosService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService) { }

  async ngOnInit(): Promise<void> {
    this.registrosAlterados = new Array<GrupoDeEstadosModel>();
    this.estadosDisponiveisInicial = new Array<EstadosModel>();
    this.gruposInicial = new Array<GrupoDeEstadosModel>();
    await this.obter();
    this.iniciarCopiaArraysTemporarios();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_GRUPO_DE_ESTADOS);
  }

  async obter(): Promise<void> {
    try {
      const retorno = await this.grupoDeEstadosService.obter();
      this.estadosDisponiveis = retorno.result.data.estadosDisponiveis.sort((a,b) => a.descricao.localeCompare(b.descricao));
      this.grupos = retorno.result.data.gruposEstados;
    } catch (error) {
      console.error(error);
      this.dialog.alert('Não foi possível carregar as informações', (error as HttpErrorResult).messages.join('\n'));
    }
  }

  public carregarEstadosGrupo(grupoId: number, nome: string): void {

      var  grupoClicado = this.grupos.find(x => (grupoId > 0 && x.id === grupoId) || x.nome === nome);
      this.listaEstados = [];
      this.listaEstados = grupoClicado.estadosGrupo.sort((a,b) => a.descricao.localeCompare(b.descricao));

      if (!grupoClicado.persistido && grupoClicado.estadosGrupo.length < 1)
          this.habilitaAdicionarEstados();

  }

  public adicionarNovoGrupo(nome: string): void {

    this.novoGrupo = true;
    setTimeout(this.posicionaCursorInsercaoGrupo, 0);
    setTimeout(() => this.inputNovoGrupo.nativeElement.focus(), 0);

    if (nome && !this.controle) {
      if (this.validarNomeGrupo(nome)) {
        var grupoAdicionado: GrupoDeEstadosModel = {
          id: 0,
          estadosGrupo: [],
          numeroEstadoIniciais: 0,
          nome: nome.toUpperCase().trim(),
          excluido: false,
          persistido: false,
          nomeAnterior: nome.toUpperCase().trim()
        };
        this.registrosAlterados.push(grupoAdicionado);
        this.grupos.push(grupoAdicionado);
        this.novoGrupo = false;
        this.listaEstados = null;
      }
    }
    else if (nome === '') {
      this.novoGrupo = false;
    }
  }

  public editarNomeGrupo(novoNome: string) {
    if (novoNome) {
      if (this.validarNomeGrupo(novoNome)) {
      this.editandoGrupo = false;
      var grupoAtualizado = this.grupos.find(x => x.nome === this.grupoSelecionado.nome)
      grupoAtualizado.nome = novoNome.toUpperCase();

      this.registrosAlterados = this.registrosAlterados.filter(x => x.nome !== this.grupoSelecionado.nome);
      this.registrosAlterados.push(grupoAtualizado);
      this.editandoGrupo = false;
      }
   }
    else {
       this.editandoGrupo = true;

    }
  }

  public async removerGrupo() {
    const confirm =  await this.dialog.confirm('Todos os estados associados a esse grupo passarão a estar disponíveis para inclusão em novos grupos ou nos já cadastrados.', 'Deseja continuar?');
    if (confirm) {
      var grupoRemovido = this.grupos.find(x => x.nome === this.grupoSelecionado.nome)

    if (grupoRemovido && grupoRemovido.estadosGrupo) {
      for (let i = 0; i < grupoRemovido.estadosGrupo.length; i++) {
        this.estadosDisponiveis.push(grupoRemovido.estadosGrupo[i]);
      }
    }

      this.grupos = this.grupos.filter(x => x.nome !== grupoRemovido.nome);
      grupoRemovido.excluido = true;
      grupoRemovido.nomeAnterior = grupoRemovido.nome;
      this.listaEstados = [];
      this.registrosAlterados = this.registrosAlterados.filter(x => x.nome !== this.grupoSelecionado.nome);

      if (grupoRemovido.persistido || grupoRemovido.id > 0)
        this.registrosAlterados.push(grupoRemovido);

      this.estadosDisponiveis.sort((a,b) => a.descricao.localeCompare(b.descricao));
    }
  }

  public adicionarNovoEstado(empresa: EstadosModel): void {
    if (empresa) {
      empresa.persistido = false;
      var grupoAtualizado = this.grupos.find(x => (this.grupoSelecionado.id > 0 && x.id === this.grupoSelecionado.id) || x.nome === this.grupoSelecionado.nome);
      grupoAtualizado.excluido = false;
      if (grupoAtualizado.estadosGrupo)
      {
        grupoAtualizado.estadosGrupo.push(empresa);
        this.estadosDisponiveis = this.estadosDisponiveis.filter(x => x.id !== empresa.id);
      }
      else
      {
        grupoAtualizado.estadosGrupo = new Array<EstadosModel>();
        grupoAtualizado.estadosGrupo.push(empresa);
      }

      this.registrosAlterados = this.registrosAlterados.filter(x => x.nome !== this.grupoSelecionado.nome);
      this.registrosAlterados.push(grupoAtualizado);
      this.carregarEstadosGrupo(null, grupoAtualizado.nome);

    }
  }

  public async removerEstado(empresa: EstadosModel) {
    var grupo = this.grupos.find(x => (this.grupoSelecionado.id > 0 && x.id === this.grupoSelecionado.id) || x.nome === this.grupoSelecionado.nome)
    if (empresa) {
        if (grupo.estadosGrupo.filter(x => x.id !== empresa.id).length < 1) {
          const confirm = await this.dialog.confirm('Grupos sem estados serão excluídos', 'Deseja continuar?');
          if (confirm) {
            this.removerGrupo();
          }
        }
        else {

          this.estadosDisponiveis.push(empresa);
          grupo.estadosGrupo = grupo.estadosGrupo.filter(x => x.id !== empresa.id);
          this.listaEstados = grupo.estadosGrupo.filter(x => x.id !== empresa.id);
          this.registrosAlterados = this.registrosAlterados.filter(x => x.nome !== this.grupoSelecionado.nome);
          grupo.excluido = false;
          this.registrosAlterados.push(grupo);
          this.estadosDisponiveis.sort((a,b) => a.descricao.localeCompare(b.descricao));
        }
    }
  }

  public async exportar(): Promise<void> {
    try {
      await this.grupoDeEstadosService.exportar();
    } catch (error) {
      console.error(error);
      this.dialog.err('Não foi possível exportar as informações',
      (error as HttpErrorResult).messages.join('\n'));
    }
  }

  public async salvar(): Promise<void> {
    var erro = false;
    var grupoSemEstado = this.validarTodosOsGrupos();

    if (grupoSemEstado.length > 0){
      this.dialog.err('Existem grupos sem estados associados');
    } else if(this.estadosDisponiveis.length > 0)
    {
      this.dialog.err('Existem estados não associados a um grupo');
    }
    else {
      try {
        await this.grupoDeEstadosService.atualizar(this.registrosAlterados);
        await this.dialog.alert('Cadastro realizado com sucesso', 'Os grupos foram atualizados no sistema.');
        this.indicarGruposPersistidos();
      } catch (errors) {
        await this.dialog.err('Operação não realizada', (errors as HttpErrorResult).messages.join('\n'));
        erro = true;
        this.obter();
      }
      if (!erro) {
        this.listaEstados = this.grupoSelecionado.estadosGrupo;
      }
      this.desfazerAlteracoes();
    }
  }

  private validarTodosOsGrupos() : Array<GrupoDeEstadosModel> {
    var grupoSemEstados = [];
    if (this.registrosAlterados) {
      for (let i = 0; i < this.registrosAlterados.length; i++) {
        if (this.registrosAlterados[i].estadosGrupo.length < 1) {
            grupoSemEstados.push(this.registrosAlterados[i]);
        }
      }
    }
    return grupoSemEstados;
  }

  private validarNomeGrupo(nome: string): boolean {
      var erro =  true;
      var existeGrupoComNome;
      if (this.grupoSelecionado && this.grupoSelecionado.nome === nome)
        existeGrupoComNome = this.grupos.filter(y => y.nome !== this.grupoSelecionado.nome).find(x => x.nome === nome.toUpperCase().trim());
      else
        existeGrupoComNome = this.grupos.find(x => x.nome === nome.toUpperCase().trim());

      if (existeGrupoComNome) {
        this.dialog.err('Já existe um grupo com o nome informado.', 'O registro não foi incluído');
        erro = false;
      }
      if (nome.length > 60) {
        this.dialog.err('O nome do grupo deve possuir no máximo 60 caracteres.', 'O registro não foi incluído');
        erro = false;
      }
      this.novoGrupo = false;
      return erro;
  }

  public habilitaRemoverEstados() {
    this.removendoEstados =  true;
    this.adicionandoEstados = false;
  }

  public habilitaAdicionarEstados() {
    this.removendoEstados =  false;
    this.adicionandoEstados = true;
  }

  public linhaSelecionada(index: number, grupo: GrupoDeEstadosModel) {
    this.selectedIndex = index;
    this.grupoSelecionado = grupo;
  }

  private posicionaCursorInsercaoGrupo() {
    var grupoGrid = document.getElementById("gridGrupos");
    grupoGrid.scrollTop = grupoGrid.scrollHeight;
  }

  private indicarGruposPersistidos(){

      for (let i = 0; i < this.grupos.length; i++) {
         this.grupos[i].persistido = true;
         this.grupos[i].numeroEstadoIniciais = this.grupos[i].estadosGrupo.length
         for (let k = 0; k < this.grupos[i].estadosGrupo.length; k++) {
          this.grupos[i].estadosGrupo[k].persistido = true;
          this.grupos[i].nomeAnterior = this.grupos[i].nome;
         }
      }

      this.iniciarCopiaArraysTemporarios();
  }

  private iniciarCopiaArraysTemporarios() {
    this.gruposInicial =  JSON.parse(JSON.stringify(this.grupos));
    this.estadosDisponiveisInicial = JSON.parse(JSON.stringify(this.estadosDisponiveis));

    //Não funciona para o IE 7
    // window.localStorage.setItem('grupos', JSON.stringify(this.grupos))
    // window.localStorage.setItem('empresasDisponiveis', JSON.stringify(this.empresasDisponiveis))
  }

  public desfazerAlteracoes(){
    this.novoGrupo = false;
    this.removendoEstados = false;
    this.adicionandoEstados = false;
    this.editandoGrupo = false;
    this.registrosAlterados = [];
    this.grupos = this.gruposInicial;
    this.estadosDisponiveis = this.estadosDisponiveisInicial;
    this.listaEstados = []
    this.grupoSelecionado = null;
    this.selectedIndex = null;

    this.iniciarCopiaArraysTemporarios();

    //Não funcina para o IE 7
    // this.grupos = JSON.parse(window.localStorage.getItem('grupos'));
    // this.empresasDisponiveis = JSON.parse(window.localStorage.getItem('empresasDisponiveis'));
  }

  public async limparBuscaGrupoPorEstados() {
    this.nomeEstadoFiltroBusca = '';
  }

  // Fim métodos auxiliares
}
