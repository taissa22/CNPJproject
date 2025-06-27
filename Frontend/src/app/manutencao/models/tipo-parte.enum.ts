export class TipoParte {
    constructor(valor: string, descricao: string) {
        this.valor = valor;
        this.descricao = descricao;
    }
    readonly valor: string;
    readonly descricao: string;

    static fromJson(p: TipoParte): TipoParte {
        return new TipoParte(p.valor, p.descricao);
    }

    static readonly NAO_DEFINIDO: TipoParte = new TipoParte('', 'Não Definido');
    static readonly PESSOA_FISICA: TipoParte = new TipoParte('F', 'Pessoa Física');
    static readonly PESSOA_JURIDICA: TipoParte = new TipoParte('J', 'Pessoa Jurídica');


    static readonly Todos: Array<TipoParte> = [
        TipoParte.NAO_DEFINIDO,
        TipoParte.PESSOA_FISICA,
        TipoParte.PESSOA_JURIDICA,

    ];
}
