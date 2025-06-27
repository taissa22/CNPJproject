export class ComarcaBB{

    constructor(id:number, estadoId:string, codigo:number, nome:string){
        this.id = id ;
        this.estadoId = estadoId ;
        this.codigo  = codigo;
        this.nome = nome;
    }

    readonly id: number = 0;
    readonly estadoId : string = "";
    readonly codigo : number = 0;
    readonly nome : string = ""

    static fromObj(obj: any): ComarcaBB {
        if(!obj) return null;
        return new ComarcaBB(obj.id, obj.estadoId, obj.codigo, obj.nome);
    }
}