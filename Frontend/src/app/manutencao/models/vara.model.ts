import { Comarca } from "./comarca.model";
import { OrgaoBB } from "./orgao_bb.model";
import { TipoDeVara } from "./tipo-de-vara";

export class Vara{

    constructor(
        varaId: number,
        comarcaId : number,
        tipoVaraId : number,
        tipoVara : TipoDeVara,
        endereco : string,
        orgaoBBId : number,
        orgaoBB : OrgaoBB,
    ){
        this.varaId = varaId
        this.comarcaId = comarcaId
        this.tipoVaraId = tipoVaraId
        this.tipoVara = tipoVara
        this.endereco = endereco
        this.orgaoBBId = orgaoBBId
        this.orgaoBB =  orgaoBB
    }

    public varaId: number;
    public comarcaId : number;
    public tipoVaraId : number;
    public tipoVara : TipoDeVara;
    public endereco : string;
    public orgaoBBId : number;
    public orgaoBB : OrgaoBB;

    static fromObj(obj: any){
       if(!obj) return null;
       return new Vara(obj.varaId,obj.comarcaId,obj.tipoVaraId,TipoDeVara.fromObj(obj.tipoVara),obj.endereco,obj.orgaoBBId,OrgaoBB.fromObj(obj.orgaoBB));
    }
}
