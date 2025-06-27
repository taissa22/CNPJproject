import { GrupoEmpresaContabilSapService } from './../../../relatorios/services/grupo-empresa-contabil-sap.service';
import { HttpErrorResult } from './../../../core/http/http-error-result';
import { GrupoEmpresaContabilSapModel } from './../../../relatorios/models/grupo-empresa-contabil-sap.model';
import { DialogService } from './../../../shared/services/dialog.service';
import { EmpresaModel } from './../../../relatorios/models/empresa.model';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BreadcrumbsService } from '@shared/services/breadcrumbs.service';
import { Permissoes } from '@permissoes';

@Component({
  selector: 'app-grupo-empresa-contabil-sap-novo',
  templateUrl: './grupo-empresa-contabil-sap-novo.component.html',
  styleUrls: ['./grupo-empresa-contabil-sap-novo.component.scss']
})
export class GrupoEmpresaContabilSapNovoComponent implements OnInit {
  empresasXGrupoPesquisa: Array<any>;
  empresasDisponiveis: Array<EmpresaModel>;
  grupos: Array<GrupoEmpresaContabilSapModel>;
  empresasDisponiveisInicial: Array<EmpresaModel>;
  gruposInicial: Array<GrupoEmpresaContabilSapModel>;
  empresasGrupo: Array<EmpresaModel>;
  registrosAlterados: Array<GrupoEmpresaContabilSapModel>;
  removendoEmpresa: boolean = false;
  adicionandoEmpresa: boolean = false;
  novoGrupo: boolean = false;
  editandoGrupo: boolean = false;
  grupoSelecionado: GrupoEmpresaContabilSapModel = null;
  selectedIndex: number = null;
  nomeEmpresaFiltro = '';
  nomeEmpresaFiltroBusca = '';
  controle = false;

  nomeGrupoEditado: string = '';
  empresaRecuperandaEditada: boolean = false;
  nomeGrupoAdcionado: string = '';
  empresaRecuperandaAdcionada: boolean = false;
  breadcrumb: string;

  @ViewChild('inputNovoGrupo', { static: false }) inputNovoGrupo: ElementRef;
  @ViewChild('modalNovoGrupo', { static: false }) modalNovoGrupo: ElementRef;
  @ViewChild('modalEditaGrupo', { static: false }) modalEditaGrupo: ElementRef;

  constructor(
    private grpEmpContabilSapService: GrupoEmpresaContabilSapService,
    private dialog: DialogService,
    private breadcrumbsService: BreadcrumbsService
  ) {}

  async ngOnInit(): Promise<void> {
    this.registrosAlterados = new Array<GrupoEmpresaContabilSapModel>();
    this.empresasDisponiveisInicial = new Array<EmpresaModel>();
    this.gruposInicial = new Array<GrupoEmpresaContabilSapModel>();
    await this.obter();
    this.iniciarCopiaArraysTemporarios();
    this.breadcrumb = await this.breadcrumbsService.nomeBreadcrumb(Permissoes.ACESSAR_GRUPO_EMPRESA_CONTABIL_SAP);
  }

  async obter(): Promise<void> {
    try {
      const retorno = await this.grpEmpContabilSapService.obter();
      this.empresasDisponiveis = retorno.result.data.empresasDisponiveis;
      this.grupos = retorno.result.data.grupoXEmpresas;
    } catch (error) {
      console.error(error);
      this.dialog.alert(
        'Não foi possível carregar as informações',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  public carregarEmpresasGrupo(grupoId: number, nome: string): void {
    var grupoClicado = this.grupos.find(
      x => (grupoId > 0 && x.id === grupoId) || x.nome === nome
    );
    this.empresasGrupo = [];
    this.empresasGrupo = grupoClicado.empresasGrupo;

    if (!grupoClicado.persistido && grupoClicado.empresasGrupo.length < 1)
      this.habilitaAdicionarEmpresas();
  }

  public adicionarNovoGrupo(nome: string, recuperanda: boolean): void {
    this.novoGrupo = true;
    setTimeout(this.posicionaCursorInsercaoGrupo, 0);
    setTimeout(this.selecionaUltimaDivGrupo, 0);

    if (nome && !this.controle) {
      if (this.validarNomeGrupo(nome)) {
        var grupoAdicionado: GrupoEmpresaContabilSapModel = {
          id: 0,
          empresasGrupo: [],
          qtdEmpresasIniciaisControle: 0,
          nome: nome.toUpperCase().trim(),
          excluido: false,
          persistido: false,
          nomeAnterior: nome.toUpperCase().trim(),
          recuperanda: recuperanda,
          recuperandaAnterior: recuperanda
        };
        this.registrosAlterados.push(grupoAdicionado);
        this.grupos.push(grupoAdicionado);
        this.novoGrupo = false;
        this.empresasGrupo = null;
        this.modalNovoGrupo.nativeElement.click();
        this.nomeGrupoAdcionado = '';
      }
    } else if (nome === '') {
      this.novoGrupo = false;
    }
  }

  public editarNomeGrupo(novoNome: string, recuperanda: boolean) {
    this.habilitaAdicionarEmpresas();
    if (novoNome) {
      if (this.validarNomeGrupo(novoNome)) {
        this.editandoGrupo = false;
        var grupoAtualizado = this.grupos.find(
          x => x.nome === this.grupoSelecionado.nome
        );
        grupoAtualizado.nome = novoNome.toUpperCase();
        grupoAtualizado.recuperanda = recuperanda;

        this.registrosAlterados = this.registrosAlterados.filter(
          x => x.nome !== this.grupoSelecionado.nome
        );
        this.registrosAlterados.push(grupoAtualizado);
        this.editandoGrupo = false;
        this.modalEditaGrupo.nativeElement.click();
      }
    } else {
      this.editandoGrupo = true;
    }
  }

  public carregaDadosGrupo() {
    this.nomeGrupoEditado = this.grupoSelecionado.nome;
    this.empresaRecuperandaEditada = this.grupoSelecionado.recuperanda;
  }

  public async removerGrupo() {
    const confirm = await this.dialog.confirm(
      'Todas as empresas associadas a esse Grupo Empresa passarão a estar disponíveis para inclusão em novos Grupos Empresas ou nos já cadastrados.',
      'Deseja continuar?'
    );
    if (confirm) {
      var grupoRemovido = this.grupos.find(
        x => x.nome === this.grupoSelecionado.nome
      );

      if (grupoRemovido && grupoRemovido.empresasGrupo) {
        for (let i = 0; i < grupoRemovido.empresasGrupo.length; i++) {
          this.empresasDisponiveis.push(grupoRemovido.empresasGrupo[i]);
        }
      }

      this.grupos = this.grupos.filter(x => x.nome !== grupoRemovido.nome);
      grupoRemovido.excluido = true;
      grupoRemovido.nomeAnterior = grupoRemovido.nome;
      this.empresasGrupo = [];
      this.registrosAlterados = this.registrosAlterados.filter(
        x => x.nome !== this.grupoSelecionado.nome
      );

      if (grupoRemovido.persistido || grupoRemovido.id > 0)
        this.registrosAlterados.push(grupoRemovido);

      this.empresasDisponiveis.sort((a, b) => a.nome.localeCompare(b.nome));
      this.salvar('Grupo excluído com sucesso');
    }
  }

  public selecionaUltimaDivGrupo() {
    let ultimaDivGrupo = document.getElementsByName('divGrupo').length - 1;
    document.getElementsByName('divGrupo')[ultimaDivGrupo].click();
  }

  public adicionarNovaEmpresa(empresa: EmpresaModel): void {
    if (empresa) {
      empresa.persistido = false;
      var grupoAtualizado = this.grupos.find(
        x =>
          (this.grupoSelecionado.id > 0 && x.id === this.grupoSelecionado.id) ||
          x.nome === this.grupoSelecionado.nome
      );
      grupoAtualizado.excluido = false;
      if (grupoAtualizado.empresasGrupo) {
        grupoAtualizado.empresasGrupo.push(empresa);
        this.empresasDisponiveis = this.empresasDisponiveis.filter(
          x => x.id !== empresa.id
        );
      } else {
        grupoAtualizado.empresasGrupo = new Array<EmpresaModel>();
        grupoAtualizado.empresasGrupo.push(empresa);
      }

      this.registrosAlterados = this.registrosAlterados.filter(
        x => x.nome !== this.grupoSelecionado.nome
      );
      this.registrosAlterados.push(grupoAtualizado);
      this.carregarEmpresasGrupo(null, grupoAtualizado.nome);
    }
  }

  public async removerEmpresa(empresa: EmpresaModel) {
    var grupo = this.grupos.find(
      x =>
        (this.grupoSelecionado.id > 0 && x.id === this.grupoSelecionado.id) ||
        x.nome === this.grupoSelecionado.nome
    );
    if (empresa) {
      if (grupo.empresasGrupo.filter(x => x.id !== empresa.id).length < 1) {
        const confirm = await this.dialog.confirm(
          'Grupos sem empresas serão excluídos',
          'Deseja continuar?'
        );
        if (confirm) {
          this.removerGrupo();
        }
      } else {
        this.empresasDisponiveis.push(empresa);
        grupo.empresasGrupo = grupo.empresasGrupo.filter(
          x => x.id !== empresa.id
        );
        this.empresasGrupo = grupo.empresasGrupo.filter(
          x => x.id !== empresa.id
        );
        this.registrosAlterados = this.registrosAlterados.filter(
          x => x.nome !== this.grupoSelecionado.nome
        );
        grupo.excluido = false;
        this.registrosAlterados.push(grupo);
        this.empresasDisponiveis.sort((a, b) => a.nome.localeCompare(b.nome));
      }
    }
  }

  public async exportar(): Promise<void> {
    try {
      await this.grpEmpContabilSapService.exportar();
    } catch (error) {
      console.error(error);
      this.dialog.err(
        'Não foi possível exportar as informações',
        (error as HttpErrorResult).messages.join('\n')
      );
    }
  }

  public async salvar(mensagem: string): Promise<void> {
    var grpsSemEmpresa = this.validarTodosOsGrupos();
    if (grpsSemEmpresa) {
      this.dialog.err('Existem empresas sem grupos associados');
    } else {
      try {
        // Para manter a consistência com o back e verificar se algum outro usuário adicionou algo.
        if (this.registrosAlterados.length > 0) {
          await this.grpEmpContabilSapService.atualizar(
            this.registrosAlterados
          );
          this.indicarGruposPersistidos();
        }
        await this.dialog.alert(
          mensagem,
          'Os grupos foram atualizados no sistema.'
        );
      } catch (errors) {
        await this.dialog.err(
          'Operação não realizada',
          (errors as HttpErrorResult).messages.join('\n')
        );
        this.obter();
      }
      this.desfazerAlteracoes();
    }
  }

  // Métodos auxiliares

  private validarTodosOsGrupos(): Boolean {
    return this.registrosAlterados.some(x => x.empresasGrupo.length < 1);
  }

  private validarNomeGrupo(nome: string): boolean {
    var erro = true;
    var existeGrupoComNome;
    if (this.grupoSelecionado && this.grupoSelecionado.nome === nome)
      existeGrupoComNome = this.grupos
        .filter(y => y.nome !== this.grupoSelecionado.nome)
        .find(x => x.nome === nome.toUpperCase().trim());
    else
      existeGrupoComNome = this.grupos.find(
        x => x.nome === nome.toUpperCase().trim()
      );

    if (existeGrupoComNome) {
      this.dialog.err(
        'Já existe um grupo com o nome informado.',
        'O registro não foi incluído'
      );
      erro = false;
    }
    if (nome.length > 50) {
      this.dialog.err(
        'O nome da empresa deve possuir no máximo 50 caracteres.',
        'O registro não foi incluído'
      );
      erro = false;
    }
    this.novoGrupo = false;
    return erro;
  }

  public habilitaRemoverEmpresas() {
    this.removendoEmpresa = true;
    this.adicionandoEmpresa = false;
  }

  public habilitaAdicionarEmpresas() {
    this.removendoEmpresa = false;
    this.adicionandoEmpresa = true;
  }

  public linhaSelecionada(index: number, grupo: GrupoEmpresaContabilSapModel) {
    this.selectedIndex = index;
    this.grupoSelecionado = grupo;
  }

  private posicionaCursorInsercaoGrupo() {
    var grupoGrid = document.getElementById('gridGrupos');
    grupoGrid.scrollTop = grupoGrid.scrollHeight;
  }

  private indicarGruposPersistidos() {
    for (let i = 0; i < this.grupos.length; i++) {
      this.grupos[i].persistido = true;
      this.grupos[i].qtdEmpresasIniciaisControle =
        this.grupos[i].empresasGrupo.length;
      for (let k = 0; k < this.grupos[i].empresasGrupo.length; k++) {
        this.grupos[i].empresasGrupo[k].persistido = true;
        this.grupos[i].nomeAnterior = this.grupos[i].nome;
        this.grupos[i].recuperandaAnterior = this.grupos[i].recuperanda;
      }
    }

    this.iniciarCopiaArraysTemporarios();
  }

  private iniciarCopiaArraysTemporarios() {
    this.gruposInicial = JSON.parse(JSON.stringify(this.grupos));
    this.empresasDisponiveisInicial = JSON.parse(
      JSON.stringify(this.empresasDisponiveis)
    );

    //Não funciona para o IE 7
    // window.localStorage.setItem('grupos', JSON.stringify(this.grupos))
    // window.localStorage.setItem('empresasDisponiveis', JSON.stringify(this.empresasDisponiveis))
  }

  public desfazerAlteracoes() {
    this.novoGrupo = false;
    this.removendoEmpresa = false;
    this.adicionandoEmpresa = false;
    this.editandoGrupo = false;
    this.registrosAlterados = [];
    this.grupos = this.gruposInicial;
    this.empresasDisponiveis = this.empresasDisponiveisInicial;
    this.empresasGrupo = [];
    this.grupoSelecionado = null;
    this.selectedIndex = null;

    this.iniciarCopiaArraysTemporarios();

    //Não funcina para o IE 7
    // this.grupos = JSON.parse(window.localStorage.getItem('grupos'));
    // this.empresasDisponiveis = JSON.parse(window.localStorage.getItem('empresasDisponiveis'));
  }

  public async limparBuscaGrupoPorEmpresa() {
    this.nomeEmpresaFiltroBusca = '';
  }

  // Fim métodos auxiliares
}
