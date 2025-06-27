import { Usuario } from "./usuario";

export class UsuarioOperacaoRetroativa {
    private constructor(
        codUsuario: string,
        limiteAlteracao: number,
        tipoProcesso : number,
        usuario : Usuario

    ) {
        this.codUsuario = codUsuario;
        this.limiteAlteracao = limiteAlteracao;
        this.tipoProcesso = tipoProcesso;
        this.usuario = usuario;
    }

    readonly codUsuario: string;
    readonly limiteAlteracao: number;
    readonly tipoProcesso : number;
    readonly usuario : Usuario;

    static fromObj(obj: UsuarioOperacaoRetroativaBack): UsuarioOperacaoRetroativa {
        return ({
            codUsuario: obj.codUsuario,
            limiteAlteracao: obj.limiteAlteracao,
            tipoProcesso: obj.tipoProcesso,
            usuario: obj.usuario
        });
    }
}

export class UsuarioOperacaoRetroativaBack {     
    readonly codUsuario: string;
    readonly limiteAlteracao: number;
    readonly tipoProcesso : number;
    readonly usuario : Usuario;
}