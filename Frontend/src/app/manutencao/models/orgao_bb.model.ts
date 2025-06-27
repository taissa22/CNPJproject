import { ComarcaBB } from "@manutencao/models/comarca-bb.model";
import { TribunalBB } from "./tribunal-bb.model";

export class OrgaoBB {

    constructor(
        id: number,
        nome: string,
        bbTribunalId: number,
        TribunalBB: TribunalBB,
        comarcaBBId: number,
        comarcaBB: ComarcaBB
    ) {
        this.id = id;
        this.nome = nome;
        this.tribunalBBId = bbTribunalId;
        this.tribunalBB = TribunalBB;
        this.comarcaBBId = comarcaBBId;
        this.comarcaBB = comarcaBB;
    }

    public id: number = 0;
    public nome: string = "";
    public tribunalBBId: number = 0;
    public tribunalBB: TribunalBB = null;
    public comarcaBBId: number = 0;
    public comarcaBB: ComarcaBB = null;

    static fromObj(obj: any): OrgaoBB {
        if(!obj) return null;
        return new OrgaoBB(obj.id,obj.nome,obj.tribunalBBId,TribunalBB.fromObj(obj.tribunalBB),obj.comarcaBBId,ComarcaBB.fromObj(obj.comarcaBB));
    }

}
