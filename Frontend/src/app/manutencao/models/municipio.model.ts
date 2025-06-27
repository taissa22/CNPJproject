export class Municipio {
    constructor(id: number, estadoId: string, nome: string) {
        this.id = id;
        this.estadoId = estadoId;
        this.nome = nome;
    }

    id: number = 0;
    estadoId: string = '';
    nome: string = '';

    public static fromObj(obj: any) {
        if (!obj) return;
        return new Municipio(obj.id,obj.estadoId,obj.nome);
    }
}