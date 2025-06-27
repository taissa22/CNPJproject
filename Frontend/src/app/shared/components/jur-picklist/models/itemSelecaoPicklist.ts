export class itemSelecaoPicklist{
   id: string;
   descricao: string; 
   marcado : boolean;
   hide: boolean;
   principal: boolean;
   constructor(id:string,descricao:string,principal:boolean){
    this.id = id.toString();
    this.descricao = descricao;
    this.principal = principal;
   }
}