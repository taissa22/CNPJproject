export class Permissao {
    private constructor(
        permissaoId: string,
        descricao: string,
        caminho: string,
        modulos: string[]
    ) {
        this.permissaoId = permissaoId;
        this.descricao = descricao;
        this.caminho = caminho;
        this.modulos = modulos;
    }

    readonly permissaoId: string;
    readonly descricao?: string;
    readonly caminho?: string;
    readonly modulos?: string[]

    static fromJson(o: any): Permissao {
        return ({
            permissaoId: o.permissaoId,
            descricao: o.descricao,
            caminho: o.caminho,
            modulos: o.modulos,
        });
    }

}

export class PermissaoBack {
    permissaoId: string;
    descricao: string;
    caminho: string;
    listaModulos: Array<number>;
}