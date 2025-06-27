export class Perfil {
  nomeId: string;
  descricao: string;
  perfilWeb: boolean;
  tipoUsuario: number;
  restrito: boolean;
  ativo: boolean;
  gestorDefault: string;
  gestorDefaultId: string;
  modulo: string;
  permissoes: Array<Permissao>;
  usuariosPerfil: Array<UsuarioPerfil>;
  quantidadeAssociadaUsuario: number;
  quantidadeDisponivelUsuario: number;
  quantidadeAssociadaPermissao: number;
  quantidadeDisponivelPermissao: number;
  tipoQuery: boolean;
  mudancasUsuario: boolean;
  mudancasPermissoes: boolean;
  mudancasPerfilUsuario: boolean;

  static fromJson(a: Perfil): Perfil {
    const perfil = new Perfil();
    perfil.usuariosPerfil = new Array<UsuarioPerfil>();
    perfil.permissoes = new Array<Permissao>();

    perfil.nomeId = a.nomeId;
    perfil.descricao = a.descricao;
    perfil.perfilWeb = a.perfilWeb;
    perfil.tipoUsuario = a.tipoUsuario;
    perfil.restrito = a.restrito;
    perfil.ativo = a.ativo;
    perfil.gestorDefault = a.gestorDefault;
    perfil.gestorDefaultId = a.gestorDefaultId;
    perfil.modulo = a.modulo;
    perfil.permissoes = a.permissoes;
    perfil.usuariosPerfil = a.usuariosPerfil;
    perfil.tipoQuery = a.tipoQuery;
    perfil.mudancasPerfilUsuario = a.mudancasPerfilUsuario;
    perfil.mudancasPermissoes = a.mudancasPermissoes;
    perfil.mudancasUsuario = a.mudancasUsuario;


    return perfil;
  }
}

export class Permissao {
  codigo: string;
  descricao: string;
  janela: string;
  associado: boolean;
  codigoMenu: string;
  caminho: string
}

export class UsuarioPerfil {
  codigo: string;
  nome: string;
  associado: boolean;
}

export class Gestores {
  key: string;
  value: string;
}
