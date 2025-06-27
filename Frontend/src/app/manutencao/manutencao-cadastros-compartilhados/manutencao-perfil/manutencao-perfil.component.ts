import { OnInit } from '@angular/core';
import { DualListModel } from '@core/models/dual-list.model';
import { Component, AfterViewInit } from '@angular/core';
import { Gestores, Perfil, Permissao, UsuarioPerfil } from '@manutencao/models/perfil.model';
import { PefilService } from '@manutencao/services/perfil.service';
import { HttpErrorResult } from '@core/http';
import { ActivatedRoute, Router } from '@angular/router';
import { DialogService } from '@shared/services/dialog.service';

@Component({
  selector: 'app-manutencao-perfil',
  templateUrl: './manutencao-perfil.component.html',
  styleUrls: ['./manutencao-perfil.component.scss']
})


export class ManutencaoPerfilComponent implements OnInit, AfterViewInit {

  constructor(private dialog: DialogService,
    private perfilService: PefilService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
  }
  //Atributos publicos utilizados no html
  public gestorSelecionadoId: string;
  public perfil: Perfil = new Perfil();
  public listaPermissoes: DualListModel[] = [];
  public listaUsuarios: DualListModel[] = [];
  public listaGestores: Gestores[];
  public filterPermissoesNaoSelecionados: string = '';
  public filterPermissoesSelecionados: string = '';
  public filterUsuariosNaoSelecionados: string = '';
  public filterUsuariosSelecionados: string = '';
  public perfilInicial: Perfil = new Perfil();
  public permissoesSelecionadasCount: number;
  public permissoesNaoSelecionadasCount: number;
  public usuariosSelecionadasCount: number;
  public usuariosNaoSelecionadasCount: number;
  public permissoesSelecionadasInicialCount: number;
  public permissoesNaoSelecionadasInicialCount: number;
  public usuariosSelecionadasInicialCount: number;
  public usuariosNaoSelecionadasInicialCount: number;
  public permissoesAssociadasVazio: boolean = false;
  public usuariossAssociadasVaio: boolean = false;
  public permissoesDisponiveisVazio: boolean = false;
  public usuariosDisponiveisVazio: boolean = false;
  public usuariosAssossiadosVazio: boolean;
  public finalDualListPermissoes: DualListModel[] = [];
  public finalDualListUsuarios: DualListModel[] = [];
  public value: string;
  public disable: boolean = true;
  public titulo: string;
  public caminho: string;
  public usuarios: { lista: Array<UsuarioPerfil>, qtdeAssociados: number, qtdeNaoAssociados: number };

  //Atributos privados utilizados na classe
  private idPerfil: string;
  private gestorDefaultSelecionado = new Gestores();
  private finalPermissoesSelecionadas: DualListModel[] = [];
  private finalPermissoesNaoSelecionadas: DualListModel[] = [];
  private finalUsuariosSelecionados: DualListModel[] = [];
  private finalUsuariosNaoSelecionados: DualListModel[] = [];
  private permissoesIniciaisSelecionadas: DualListModel[] = [];
  private permissoesIniciaisNaoSelecionadas: DualListModel[] = [];
  private usuariosIniciaisSelecionados: DualListModel[] = [];
  private usuariosIniciaisNaoSelecionados: DualListModel[] = [];
  private permissoesAlteradas: Permissao[] = []
  private usuariosAlterados: UsuarioPerfil[] = []
  private listaFinalAlteradosPermissoes: Permissao[] = [];
  private listaFinalAlteradosUsuarios: UsuarioPerfil[] = [];
  private listaPermissoesInicial: DualListModel[] = [];
  private listaUsuariosInicial: DualListModel[] = [];
  private tabAtual: boolean = true;
  private nomeIdInicial: string;
  private descricaoInicial: string;
  private gestorDefaultIdInicial: string;
  private gestorDefaultInicial: string;
  private statusInicial: boolean;
  private tipoUsuarioInicial: number;
  private perfilWebInicial: boolean;
  private restritoInicial: boolean;
  ehPerfilWeb: boolean;


  ngOnInit() {
    this.activatedRoute.params.subscribe(params => { this.idPerfil = atob(params.codigoPerfil) });
  }

  async ngAfterViewInit() {
    let count = 0;
    
    this.perfil = await this.perfilService.obter(this.idPerfil);
    this.usuarios = await this.perfilService.obterUsuarios(this.idPerfil);
    this.ehPerfilWeb = this.perfil.perfilWeb;

    if (!this.ehPerfilWeb) {
      (document.getElementsByClassName('bt-select-all')[0] as HTMLButtonElement).disabled = true;
      (document.getElementsByClassName('bt-select')[0] as HTMLButtonElement).disabled = true;
      (document.getElementsByClassName('bt-deselect')[0] as HTMLButtonElement).disabled = true;
      (document.getElementsByClassName('bt-deselect-all')[0] as HTMLButtonElement).disabled = true;
    }

    if (this.perfil.nomeId == "") {
      this.disable = false;
      this.perfil.tipoQuery = false;
      this.titulo = "Incluir Perfil";
      this.caminho = "Controle de acesso > Perfis > Novo"
    } else {
      this.disable = true;
      this.perfil.tipoQuery = true;
      this.titulo = "Alterar Perfil";
      this.caminho = "Controle de acesso > Perfis > Editar"
    }
    this.obterListaGestores();
    this.gestorSelecionadoId = this.perfil.gestorDefaultId;
    this.gestorDefaultSelecionado.value = this.perfil.gestorDefault
    this.gestorDefaultSelecionado.key = this.perfil.gestorDefaultId
    this.listaPermissoes = this.perfil.permissoes.map(permissao => {

      return {
        id: permissao.codigo,
        idCount: ++count,
        label: permissao.descricao,
        caminho: permissao.caminho,
        selecionado: permissao.associado,
        codigoMenu: permissao.codigoMenu,
        janela: permissao.janela
      }
    });

    count = 0;
    this.listaUsuarios = this.usuarios.lista.map(usuarioPerfil => {
      return {
        id: usuarioPerfil.codigo,
        idCount: ++count,
        label: usuarioPerfil.nome,
        selecionado: usuarioPerfil.associado,
      }
    });
    this.permissoesSelecionadasCount = this.perfil.quantidadeAssociadaPermissao;
    this.permissoesNaoSelecionadasCount = this.perfil.quantidadeDisponivelPermissao;
    this.usuariosSelecionadasCount = this.usuarios.qtdeAssociados;
    this.usuariosNaoSelecionadasCount = this.usuarios.qtdeNaoAssociados;
    this.salvarEstadoInicial();
    this.ajustarAviso(this.tabAtual);
  }

  voltar() {
    if (this.existeAlteracao()) {
      this.dialog.confirm(
        'Confirmação de uma operação',
        'Existem alterações que não foram salvas. Deseja realmente voltar?'
      ).then((result) => {
        if (result) {
          this.reiniciarListas();
          this.router.navigateByUrl(`/manutencao/perfil`)
          // history.back();
        }
      });
    } else {
      this.router.navigateByUrl(`/manutencao/perfil`)
    }
  }
  private async reiniciarCampos() {
    
    if (this.perfil.nomeId != this.nomeIdInicial)
      this.perfil.nomeId = this.nomeIdInicial

    if (this.perfil.descricao != this.descricaoInicial)
      this.perfil.descricao = this.descricaoInicial;

    if (this.gestorDefaultSelecionado.key != this.gestorDefaultIdInicial)
      this.perfil.gestorDefaultId = this.gestorDefaultIdInicial; {
      this.gestorSelecionadoId = this.gestorDefaultIdInicial
      this.gestorDefaultSelecionado.value = this.gestorDefaultInicial
      this.gestorDefaultSelecionado.key = this.gestorDefaultIdInicial
    }

    if (this.perfil.ativo != this.statusInicial)
      this.perfil.ativo = this.statusInicial;

    if (this.perfil.tipoUsuario != this.tipoUsuarioInicial)
      this.perfil.tipoUsuario = this.tipoUsuarioInicial

    if (this.perfil.perfilWeb != this.perfilWebInicial)
      this.perfil.perfilWeb = this.perfilWebInicial;

    if (this.perfil.restrito != this.restritoInicial)
      this.perfil.restrito = this.restritoInicial;

    this.listaPermissoes = [];
    this.listaUsuarios = [];

    this.listaPermissoes = this.listaPermissoesInicial.map(permissao => {
      return {
        id: permissao.id,
        idCount: permissao.idCount,
        label: permissao.label,
        caminho: permissao.caminho,
        selecionado: permissao.selecionado,
        codigoMenu: permissao['codigoMenu'],
        janela: permissao['janela']
      }
    });

    this.listaUsuarios = this.listaUsuariosInicial.map(usuario => {
      return {
        id: usuario.id,
        idCount: usuario.idCount,
        label: usuario.label,
        selecionado: usuario.selecionado,
      }
    });

    this.filterPermissoesNaoSelecionados = '';
    this.filterPermissoesSelecionados = '';
    this.filterUsuariosNaoSelecionados = '';
    this.filterUsuariosSelecionados = '';

    this.reiniciarListas();

    this.permissoesSelecionadasCount = this.listarPermissoesAssociadasInicial().length;
    this.permissoesNaoSelecionadasCount = this.listarPermissoesDisponiveisInicial().length;
    this.usuariosSelecionadasCount = this.listarUsuariosAssociadosInicial().length;
    this.usuariosNaoSelecionadasCount = this.listarUsuariosDisponiveisInicial().length;

    this.reiniciarListas()
    this.ajustarAviso(this.tabAtual);
  }

  public async cancelar(): Promise<void> {
    this.value = '';

    this.dialog.confirm(
      'Confirmação de uma operação',
      'Deseja realmente cancelar?'
    ).then((result) => {
      if (result) {
        if (this.existeAlteracao()) {
          this.reiniciarCampos();
          this.dialog.alert('Cancelado', 'Modificações não salvas foram canceladas.')
        } else {
          this.dialog.info('Atenção', 'Não existem modificações para serem canceladas.')
        }
      }
    });
  }

  private async obterListaGestores() {
    if (!this.listaGestores || this.listaGestores.length < 0)
      this.listaGestores = await this.perfilService.obterGestores();
  }

  public async exportar(ehPermissao: boolean): Promise<void> {
    try {
      await this.perfilService.exportar(this.perfil.nomeId, ehPermissao);
    } catch (error) {
      this.dialog.err('Erro', 'Não foi possível realizar a exportação dos usuários associados ao perfil') + (error as HttpErrorResult).messages.join('\n')
    }
  }

  public async salvar(): Promise<void> {
    if (this.perfil.nomeId == "") {
      this.dialog.err('Erro', 'Não é possível salvar um novo perfil com o campo "Nome" vazio. Preencha o campo e tente salvar novamente.');
      return;
    }

    if (this.perfil.descricao == "") {
      this.dialog.err('Erro', 'Não é possível salvar um novo perfil com o campo "Descrição" vazio. Preencha o campo e tente salvar novamente.');
      return;
    }


    if (!this.existeAlteracao()) {
      this.dialog.info('Atenção', 'Não existem alterações para serem salvas.')
    } else {
      try {
        this.perfil.nomeId = this.perfil.nomeId.trim()
        this.perfil.descricao = this.perfil.descricao.trim()
        this.perfil.permissoes = this.extrairListaPermissoesAlteradas();
        this.perfil.mudancasPermissoes = this.perfil.permissoes.length > 0;
        this.perfil.usuariosPerfil = this.extrairListaUsuariosAlterados();
        this.perfil.mudancasUsuario = this.perfil.usuariosPerfil.length > 0;
        this.perfil.mudancasPerfilUsuario = this.existeAlteracaoCabecalho();
        this.perfil.gestorDefaultId = this.gestorSelecionadoId;
        this.perfil.gestorDefault = '';
        await this.perfilService.atualizar(this.perfil);
        this.salvarNovoEstadoInicial();
        this.reiniciarListas();

        if (this.perfil.tipoQuery) { this.dialog.alert('Sucesso', 'Perfil atualizado.') }
        else {
          this.dialog.alert('Sucesso', 'Novo perfil salvo.').then(function () {
            this.router.navigateByUrl(`/manutencao/perfil`)
          });
        }

        this.perfil.tipoQuery = true;
        this.disable = true;
        this.disable = true;
        this.perfil.tipoQuery = true;
        this.titulo = "Alterar Perfil";
        this.caminho = "Controle de acesso > Perfis > Editar"

      } catch (error) {
        this.dialog.err('Erro', 'Existe um perfil com esse nome. ') + (error as HttpErrorResult).messages.join('\n')
      }
    }
  }


  private reiniciarListas() {
    this.finalUsuariosNaoSelecionados = [];
    this.finalUsuariosSelecionados = [];

    this.usuariosAlterados = [];
    this.permissoesAlteradas = [];

    this.listaFinalAlteradosUsuarios = [];
    this.listaFinalAlteradosPermissoes = [];

    this.finalPermissoesNaoSelecionadas = [];
    this.finalPermissoesSelecionadas = [];

    this.finalDualListPermissoes = [];
    this.finalDualListUsuarios = [];
  }

  private extrairListaUsuariosAlterados(): UsuarioPerfil[] {
    this.finalUsuariosNaoSelecionados = this.encontrarListaDiferenca(this.listarUsuariosDisponiveis(), this.usuariosIniciaisNaoSelecionados);
    this.finalUsuariosSelecionados = this.encontrarListaDiferenca(this.listarUsuariosAssociados(), this.usuariosIniciaisSelecionados);
    this.finalDualListUsuarios = [].concat(this.finalUsuariosNaoSelecionados, this.finalUsuariosSelecionados);
    this.usuariosAlterados = this.finalUsuariosNaoSelecionados.map(usuario => {
      return {
        codigo: usuario.id,
        nome: usuario.label,
        associado: usuario.selecionado
      };
    });
    this.listaFinalAlteradosUsuarios = this.usuariosAlterados;
    this.usuariosAlterados = this.finalUsuariosSelecionados.map(usuario => {
      return {
        codigo: usuario.id,
        nome: usuario.label,
        associado: usuario.selecionado
      };
    });
    return [].concat(this.listaFinalAlteradosUsuarios, this.usuariosAlterados);
  }

  private extrairListaPermissoesAlteradas(): Permissao[] {
    this.finalPermissoesNaoSelecionadas = this.encontrarListaDiferenca(this.listarPermissoesDisponiveis(), this.permissoesIniciaisNaoSelecionadas);
    this.finalPermissoesSelecionadas = this.encontrarListaDiferenca(this.listarPermissoesAssociadas(), this.permissoesIniciaisSelecionadas);
    this.finalDualListPermissoes = [].concat(this.finalPermissoesNaoSelecionadas, this.finalPermissoesSelecionadas);
    this.permissoesAlteradas = this.finalPermissoesNaoSelecionadas.map(permissao => {
      return {
        codigo: this.perfil.nomeId,
        descricao: permissao.label,
        caminho: permissao.caminho,
        associado: permissao.selecionado,
        codigoMenu: permissao['codigoMenu'],
        janela: permissao['janela']
      };
    });
    this.listaFinalAlteradosPermissoes = this.permissoesAlteradas;
    this.permissoesAlteradas = this.finalPermissoesSelecionadas.map(permissao => {
      return {
        codigo: this.perfil.nomeId,
        descricao: permissao.label,
        caminho: permissao.caminho,
        associado: permissao.selecionado,
        codigoMenu: permissao['codigoMenu'],
        janela: permissao['janela']
      };
    });
    return [].concat(this.listaFinalAlteradosPermissoes, this.permissoesAlteradas);
  }

  private encontrarListaDiferenca(listaInicial: DualListModel[], listaFinal: DualListModel[]): DualListModel[] {
    let arrayResultante: DualListModel[] = [];
    var pertence = false;
    for (let x = 0; x < listaInicial.length; x++) {
      for (let y = 0; y < listaFinal.length; y++) {
        if (listaInicial[x].idCount == listaFinal[y].idCount) {
          pertence = true;
          break;
        }
      }
      if (!pertence) {
        arrayResultante.push(listaInicial[x]);
      } else {
        pertence = false;
      }
    }
    return arrayResultante;
  }


  private existeAlteracaoCabecalho(): boolean {
    let retorno: boolean = false;

    if (this.perfil.nomeId != this.nomeIdInicial)
      retorno = true;

    if (this.perfil.descricao != this.descricaoInicial)
      retorno = true;

    if (this.gestorDefaultSelecionado.key != this.gestorDefaultIdInicial)
      retorno = true;

    if (this.perfil.ativo != this.statusInicial)
      retorno = true;

    if (this.perfil.tipoUsuario != this.tipoUsuarioInicial)
      retorno = true;

    if (this.perfil.perfilWeb != this.perfilWebInicial)
      retorno = true;

    if (this.perfil.restrito != this.restritoInicial)
      retorno = true;

    return retorno;

  }

  private existeAlteracao(): Boolean {
    if (this.perfil.nomeId != this.nomeIdInicial)
      return true
    if (this.perfil.descricao != this.descricaoInicial)
      return true;

    if (this.gestorDefaultSelecionado.key != this.gestorDefaultIdInicial)
      return true;

    if (this.perfil.ativo != this.statusInicial)
      return true;

    if (this.perfil.tipoUsuario != this.tipoUsuarioInicial)
      return true;

    if (this.perfil.perfilWeb != this.perfilWebInicial)
      return true;

    if (this.perfil.restrito != this.restritoInicial)
      return true;

    if (this.extrairListaPermissoesAlteradas().length > 0) {
      this.reiniciarListas();
      return true;
    }
    if (this.extrairListaUsuariosAlterados().length > 0) {
      this.reiniciarListas();
      return true;
    }
    this.reiniciarListas();
    return false;
  }

  private salvarNovoEstadoInicial() {
    
    this.nomeIdInicial = JSON.parse(JSON.stringify(this.perfil.nomeId));
    this.descricaoInicial = JSON.parse(JSON.stringify(this.perfil.descricao));
    this.gestorDefaultIdInicial = JSON.parse(JSON.stringify(this.perfil.gestorDefaultId));
    this.gestorDefaultInicial = JSON.parse(JSON.stringify(this.perfil.gestorDefault));
    this.statusInicial = JSON.parse(JSON.stringify(this.perfil.ativo));
    this.tipoUsuarioInicial = JSON.parse(JSON.stringify(this.perfil.tipoUsuario));
    this.perfilWebInicial = JSON.parse(JSON.stringify(this.perfil.perfilWeb));
    this.restritoInicial = JSON.parse(JSON.stringify(this.perfil.restrito));
    this.listaPermissoesInicial = this.listaPermissoes.map(permissao => {
      return {
        id: permissao.id,
        idCount: permissao.idCount,
        label: permissao.label,
        caminho: permissao.caminho,
        selecionado: permissao.selecionado,
        codigoMenu: permissao['codigoMenu'],
        janela: permissao['janela']
      }
    });
    this.listaUsuariosInicial = this.listaUsuarios.map(usuario => {
      return {
        id: usuario.id,
        idCount: usuario.idCount,
        label: usuario.label,
        selecionado: usuario.selecionado,
      }
    });
    this.permissoesIniciaisSelecionadas = this.listarPermissoesAssociadasInicial();
    this.permissoesIniciaisNaoSelecionadas = this.listarPermissoesDisponiveisInicial();

    this.usuariosIniciaisSelecionados = this.listarUsuariosAssociadosInicial();
    this.usuariosIniciaisNaoSelecionados = this.listarUsuariosDisponiveisInicial();
  }

  private salvarEstadoInicial() {

    this.nomeIdInicial = JSON.parse(JSON.stringify(this.perfil.nomeId));
    this.descricaoInicial = JSON.parse(JSON.stringify(this.perfil.descricao));
    this.gestorDefaultIdInicial = JSON.parse(JSON.stringify(this.perfil.gestorDefaultId));
    this.gestorDefaultInicial = JSON.parse(JSON.stringify(this.perfil.gestorDefault));
    this.statusInicial = JSON.parse(JSON.stringify(this.perfil.ativo));
    this.tipoUsuarioInicial = JSON.parse(JSON.stringify(this.perfil.tipoUsuario));
    this.perfilWebInicial = JSON.parse(JSON.stringify(this.perfil.perfilWeb));
    this.restritoInicial = JSON.parse(JSON.stringify(this.perfil.restrito));
    var count = 0;
    this.perfilInicial = JSON.parse(JSON.stringify(this.perfil));
    this.listaPermissoesInicial = this.perfil.permissoes.map(permissao => {
      return {
        id: permissao.codigo,
        idCount: ++count,
        label: permissao.descricao,
        caminho: permissao.caminho,
        selecionado: permissao.associado,
        codigoMenu: permissao.codigoMenu,
        janela: permissao.janela
      }
    });
    count = 0;
    this.listaUsuariosInicial = this.perfil.usuariosPerfil.map(usuarioPerfil => {
      return {
        id: usuarioPerfil.codigo,
        idCount: ++count,
        label: usuarioPerfil.nome,
        selecionado: usuarioPerfil.associado,
      }
    });
    this.permissoesIniciaisSelecionadas = this.listaPermissoesInicial.filter(x => x.selecionado);
    this.permissoesIniciaisNaoSelecionadas = this.listaPermissoesInicial.filter(x => !x.selecionado);
    this.usuariosIniciaisSelecionados = this.listaUsuarios.filter(x => x.selecionado);
    this.usuariosIniciaisNaoSelecionados = this.listaUsuarios.filter(x => !x.selecionado);
    
  }

  mostrarPermissoesSelecionadas(event) {
    this.permissoesSelecionadasCount = this.listarPermissoesAssociadas().length;
    this.permissoesNaoSelecionadasCount = this.listarPermissoesDisponiveis().length;
    this.permissoesAssociadasVazio = this.permissoesSelecionadasCount == 0 ? true : false;
    this.permissoesDisponiveisVazio = this.permissoesNaoSelecionadasCount == 0 ? true : false;

  }
  mostrarUsuariosSelecionados(event) {
    this.usuariosSelecionadasCount = this.listarUsuariosAssociados().length;
    this.usuariosNaoSelecionadasCount = this.listarUsuariosDisponiveis().length;
    this.usuariosAssossiadosVazio = this.usuariosSelecionadasCount == 0 ? true : false;
    this.usuariosDisponiveisVazio = this.usuariosNaoSelecionadasCount == 0 ? true : false;
  }

  private listarPermissoesDisponiveis() {
    return this.listaPermissoes.filter(permissao => !permissao.selecionado);
  }
  private listarPermissoesAssociadas() {
    return this.listaPermissoes.filter(permissao => permissao.selecionado);
  }

  private listarUsuariosDisponiveis() {
    return this.listaUsuarios.filter(usuario => !usuario.selecionado);
  }

  private listarUsuariosAssociados() {
    return this.listaUsuarios.filter(usuario => usuario.selecionado);
  }

  private listarPermissoesDisponiveisInicial() {
    return this.listaPermissoesInicial.filter(permissao => !permissao.selecionado);
  }
  private listarPermissoesAssociadasInicial() {
    return this.listaPermissoesInicial.filter(permissao => permissao.selecionado);
  }

  private listarUsuariosDisponiveisInicial() {
    return this.listaUsuariosInicial.filter(usuario => !usuario.selecionado);
  }

  private listarUsuariosAssociadosInicial() {
    return this.listaUsuariosInicial.filter(usuario => usuario.selecionado);
  }

  public ajustarAviso(tabAtual): boolean {
    this.tabAtual = tabAtual;
    if (tabAtual) {
      this.usuariosAssossiadosVazio = false;
      this.usuariosDisponiveisVazio = false;

      this.permissoesAssociadasVazio = this.listarPermissoesAssociadas().length == 0 ? true : false;
      this.permissoesDisponiveisVazio = this.listarPermissoesDisponiveis().length == 0 ? true : false;
    } else {
      this.permissoesAssociadasVazio = false;
      this.permissoesDisponiveisVazio = false;

      this.usuariosAssossiadosVazio = this.listarUsuariosAssociados().length == 0 ? true : false;
      this.usuariosDisponiveisVazio = this.listarUsuariosDisponiveis().length == 0 ? true : false;
    }
    return true;
  }

  public ajustarDropdownAoSelecionar(selecionado) {
    
    if (selecionado === null || selecionado === undefined) {
      this.perfil.gestorDefault = null;
      this.perfil.gestorDefaultId = null;
      this.gestorDefaultSelecionado.key = null;
      this.gestorDefaultSelecionado.value = null;
      this.gestorSelecionadoId = null;
    } else {
      this.perfil.gestorDefault = selecionado.value;
      this.perfil.gestorDefaultId = selecionado.key;
      this.gestorDefaultSelecionado.key = selecionado.key;
      this.gestorDefaultSelecionado.value = selecionado.value;
      this.gestorSelecionadoId = selecionado.key;
    }

  }
}
