export class Usuario {
    private constructor(id : string, nome : string, ativo: boolean) {this.id = id; this.nome = nome; this.ativo = ativo; this.nomeCompleto = `${nome} ${!ativo ? '[Inativo]' : ''}` }

    readonly id: string;
    readonly nome: string;
    readonly ativo: boolean;
    readonly nomeCompleto: string;

    static fromObj(obj: Usuario): Usuario { return ({id: obj.id,nome: obj.nome , ativo: obj.ativo, nomeCompleto: `${obj.nome} ${!obj.ativo ? '[Inativo]' : ''}` }); }
}

export class UsuarioBack {readonly id: string; readonly nome: string; readonly ativo: boolean}
