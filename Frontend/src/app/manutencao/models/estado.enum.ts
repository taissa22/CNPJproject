export class EstadoEnum {
    constructor(id: string, descricao: string) {
        this.id = id;
        this.descricao = descricao;
    }
    readonly id: string;
    readonly descricao: string;

    static fromJson(e: EstadoEnum): EstadoEnum {
        return new EstadoEnum(e.id, e.descricao);
    }

    static readonly NAO_DEFINIDO: EstadoEnum = new EstadoEnum(undefined, 'Não Definido');
    static readonly AC: EstadoEnum = new EstadoEnum('AC', 'Acre');
    static readonly AL: EstadoEnum = new EstadoEnum('AL', 'Alagoas');
    static readonly AM: EstadoEnum = new EstadoEnum('AM', 'Amazonas');
    static readonly AP: EstadoEnum = new EstadoEnum('AP', 'Amapá');
    static readonly BA: EstadoEnum = new EstadoEnum('BA', 'Bahia');
    static readonly CE: EstadoEnum = new EstadoEnum('CE', 'Ceará');
    static readonly DF: EstadoEnum = new EstadoEnum('DF', 'Distrito Federal');
    static readonly ES: EstadoEnum = new EstadoEnum('ES', 'Espírito Santo');
    static readonly GO: EstadoEnum = new EstadoEnum('GO', 'Goiás');
    static readonly MA: EstadoEnum = new EstadoEnum('MA', 'Maranhão');
    static readonly MG: EstadoEnum = new EstadoEnum('MG', 'Minas Gerais');
    static readonly MS: EstadoEnum = new EstadoEnum('MS', 'Mato Grosso do Sul');
    static readonly MT: EstadoEnum = new EstadoEnum('MT', 'Mato Grosso');
    static readonly PA: EstadoEnum = new EstadoEnum('PA', 'Pará');
    static readonly PB: EstadoEnum = new EstadoEnum('PB', 'Paraíba');
    static readonly PE: EstadoEnum = new EstadoEnum('PE', 'Pernambuco');
    static readonly PI: EstadoEnum = new EstadoEnum('PI', 'Piauí');
    static readonly PR: EstadoEnum = new EstadoEnum('PR', 'Paraná');
    static readonly RJ: EstadoEnum = new EstadoEnum('RJ', 'Rio de Janeiro');
    static readonly RN: EstadoEnum = new EstadoEnum('RN', 'Rio Grande do Norte');
    static readonly RO: EstadoEnum = new EstadoEnum('RO', 'Rondônia');
    static readonly RR: EstadoEnum = new EstadoEnum('RR', 'Roraima');
    static readonly RS: EstadoEnum = new EstadoEnum('RS', 'Rio Grande do Sul');
    static readonly SC: EstadoEnum = new EstadoEnum('SC', 'Santa Catarina');
    static readonly SE: EstadoEnum = new EstadoEnum('SE', 'Sergipe');
    static readonly SP: EstadoEnum = new EstadoEnum('SP', 'São Paulo');
    static readonly TO: EstadoEnum = new EstadoEnum('TO', 'Tocantins');

    static readonly Todos: Array<EstadoEnum> = [
        EstadoEnum.AC,
        EstadoEnum.AL,
        EstadoEnum.AM,
        EstadoEnum.AP,
        EstadoEnum.BA,
        EstadoEnum.CE,
        EstadoEnum.DF,
        EstadoEnum.ES,
        EstadoEnum.GO,
        EstadoEnum.MA,
        EstadoEnum.MG,
        EstadoEnum.MS,
        EstadoEnum.MT,
        EstadoEnum.PA,
        EstadoEnum.PB,
        EstadoEnum.PE,
        EstadoEnum.PI,
        EstadoEnum.PR,
        EstadoEnum.RJ,
        EstadoEnum.RN,
        EstadoEnum.RO,
        EstadoEnum.RR,
        EstadoEnum.RS,
        EstadoEnum.SC,
        EstadoEnum.SE,
        EstadoEnum.SP,
        EstadoEnum.TO

    ];
}
