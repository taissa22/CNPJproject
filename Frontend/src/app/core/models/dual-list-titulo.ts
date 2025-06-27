export interface DualListTitulo {

    id: number;
    titulo: string;
    selecionado: boolean;
    marcado: boolean;
    somenteLeitura: boolean;
    ativo?:boolean;
    dados: [{
        id:number;
        descricao: string;
        ativo?: boolean;
        marcado: boolean;
        selecionado: boolean;
        somenteLeitura: boolean;
        idPai:number;
    }]


}
