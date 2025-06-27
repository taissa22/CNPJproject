export class TribunalBB{

    constructor(
        id : number,
        nome : string,
        indInstanciaDesignada : string 
    ){
        this.id = id;
        this.nome = nome ;
        this.indInstanciaDesignada = indInstanciaDesignada ;
    }

    public id : number = 0;
    public nome : string = "";
    public indInstanciaDesignada : string = "";

    static fromObj(obj : any){
       if(!obj) return null;
       return new TribunalBB(
        obj.id,
        obj.nome,
        obj.indInstanciaDesignada 
        );
    }
}