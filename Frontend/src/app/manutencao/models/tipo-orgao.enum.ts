export class TipoOrgao {
    constructor(valor: string, descricao: string) {
        this.valor = valor;
        this.descricao = descricao;
    }
    readonly valor: string;
    readonly descricao: string;

    static fromJson(t: TipoOrgao): TipoOrgao {
        return new TipoOrgao(t.valor, t.descricao);
    }

    static readonly NAO_DEFINIDO: TipoOrgao = new TipoOrgao('', 'Não Definido');
    static readonly CIVEL_ADMINISTRATIVO: TipoOrgao = new TipoOrgao('1', 'Cível Administrativo');
    static readonly CRIMINAL_ADMINISTRATIVO: TipoOrgao = new TipoOrgao('2', 'Criminal Administrativo');
    static readonly DEMAIS_TIPOS: TipoOrgao = new TipoOrgao('O', 'Demais Tipos');

    static readonly Todos: Array<TipoOrgao> = [
        TipoOrgao.NAO_DEFINIDO,
        TipoOrgao.CIVEL_ADMINISTRATIVO,
        TipoOrgao.CRIMINAL_ADMINISTRATIVO,
        TipoOrgao.DEMAIS_TIPOS,
    ];
}
