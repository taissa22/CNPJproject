export interface ITipoDePendencia {
    id: number;
    descricao: string;

}

export class TipoDePendencia implements ITipoDePendencia {
    private constructor(
        id: number,
        descricao: string
    ) {
        this.id = id;
        this.descricao = descricao;
    }

    readonly id: number;
    readonly descricao: string;

    static fromObj(obj: any): TipoDePendencia {
        return ({
            id: obj.id,
            descricao: obj.descricao
        });
    }
}