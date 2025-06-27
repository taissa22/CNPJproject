import moment from "moment";

export class CotacaoIndiceTrabalhista {

    private constructor(
        dataCorrecao: Date,
        dataBase: Date,
        valorCotacao: number

    ) {
        this.dataCorrecao = dataCorrecao;
        this.dataBase = dataBase;
        this.valorCotacao = valorCotacao;
    }

    public dataCorrecao: Date;
    public dataBase: Date;
    public valorCotacao: number;

    static fromObj(obj: any): CotacaoIndiceTrabalhista {
        return new CotacaoIndiceTrabalhista(moment(obj.dataCorrecao).toDate(),
            moment(obj.dataBase).toDate(),
            obj.valorCotacao);
    }

}