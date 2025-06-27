export class TipoDeOrientacaoJuridica {
    private constructor(
        codigo: number,
        descricao: string
    ) {
        this.codigo = codigo;
        this.descricao = descricao;
    }

    readonly codigo: number;
    readonly descricao: string;

    static fromObj(obj: TipoDeOrientacaoJuridicaBack): TipoDeOrientacaoJuridica {
        return ({
            codigo: obj.id,
            descricao: obj.descricao
        });
    }
}

export class TipoDeOrientacaoJuridicaBack {
    readonly id: number;
    readonly descricao: string;
}
